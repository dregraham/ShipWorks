using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsRatingService : PostalRatingService
    {
        private const int MinNumberOfDaysBeforeShowingUspsPromo = 14;

        private readonly UspsShipmentType shipmentType;
        private ICarrierAccountRepository<UspsAccountEntity> accountRepository;
        private ICertificateInspector certificateInspector;
        private readonly ICachedRatesService cachedRatesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatingService"/> class.
        /// </summary>
        public UspsRatingService(
            UspsShipmentType shipmentType,
            ICarrierAccountRepository<UspsAccountEntity> accountRepository,
            ICertificateInspector certificateInspector, 
            ICachedRatesService cachedRatesService) : base(shipmentType)
        {
            this.shipmentType = shipmentType;
            this.accountRepository = accountRepository;
            this.certificateInspector = certificateInspector;
            this.cachedRatesService = cachedRatesService;
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            // Take this opportunity to try to update contract type of the account
            UspsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
            shipmentType.UpdateContractType(account);

            // Get counter rates if we don't have any Endicia accounts, letting the Postal shipment type take care of caching
            // since it should be using a different cache key
            return accountRepository.Accounts.Any() ?
                GetRatesInternal(shipment) :
                GetCounterRates(shipment);
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected virtual RateGroup GetRatesInternal(ShipmentEntity shipment)
        {
            // Start getting Express1 rates if necessary so that they should hopefully be ready when we need them
            Task<RateGroup> express1RateTask = GetExpress1RatesIfNecessary(shipment);

            RateGroup rateGroup = shipment.Postal.Usps.RateShop ?
                GetRatesForAllAccounts(shipment) :
                cachedRatesService.GetCachedRates<UspsException>(shipment, GetRatesForSpecifiedAccount);

            return new UspsExpress1RateConsolidator().Consolidate(rateGroup, express1RateTask);
        }

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            // We're going to be temporarily swapping these out to get counter rates, so 
            // make a note of the original values
            ICarrierAccountRepository<UspsAccountEntity> originalAccountRepository = accountRepository;
            ICertificateInspector originalCertificateInspector = certificateInspector;

            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                accountRepository = new UspsCounterRateAccountRepository(TangoCredentialStore.Instance);
                certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.UspsCertificateVerificationData);

                // Fetch the rates now that we're setup to use counter rates
                return cachedRatesService.GetCachedRates<UspsException>(shipment, GetRates);

            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(shipmentType));

                return errorRates;
            }
            finally
            {
                // Set everything back to normal
                accountRepository = originalAccountRepository;
                certificateInspector = originalCertificateInspector;
            }
        }

        /// <summary>
        /// Start getting Express1 rates if necessary
        /// </summary>
        private Task<RateGroup> GetExpress1RatesIfNecessary(ShipmentEntity shipment)
        {
            UspsAccountEntity express1AutoRouteAccount = UspsShipmentType.GetExpress1AutoRouteAccount((PostalPackagingType)shipment.Postal.PackagingType);

            return shipmentType.ShouldRetrieveExpress1Rates && express1AutoRouteAccount != null && !shipment.Postal.NoPostage ?
                BeginRetrievingExpress1Rates(shipment, express1AutoRouteAccount) :
                CreateEmptyExpress1RatesTask();
        }

        /// <summary>
        /// Get rates for the account specified in the shipment
        /// </summary>
        private RateGroup GetRatesForSpecifiedAccount(ShipmentEntity shipment)
        {
            List<RateResult> uspsRates = shipmentType.CreateWebClient().GetRates(shipment);
            uspsRates.ForEach(r => r.ShipmentType = shipmentType.ShipmentTypeCode);

            RateGroup rateGroup = new RateGroup(shipmentType.FilterRatesByExcludedServices(shipment, uspsRates));
            AddUspsRatePromotionFootnote(shipment, rateGroup);

            return rateGroup;
        }

        /// <summary>
        /// Get rates for all available accounts
        /// </summary>
        private RateGroup GetRatesForAllAccounts(ShipmentEntity shipment)
        {
            List<UspsAccountEntity> uspsAccounts = accountRepository.Accounts.ToList();

            // We are creating a new shipment type here so we can call get rates and not call Express1 Rates.
            // We thought of just turning off ShouldRetrieveExpress1Rates, but worried that might cause unexpected behavior
            // in a multi-threaded situation.
            UspsShipmentType uspsShipmentTypeWithNoExpress1 = new UspsShipmentType() { ShouldRetrieveExpress1Rates = false };

            try
            {
                List<Task<RateGroup>> tasks = uspsAccounts.Select(accountToCopy => CreateShipmentCopy(accountToCopy, shipment))
                    .Select(shipmentWithAccount => Task.Factory.StartNew(() => uspsShipmentTypeWithNoExpress1.GetRates(shipmentWithAccount)))
                    .ToList();

                foreach (Task<RateGroup> task in tasks)
                {
                    task.Wait();
                }

                return new UspsRateGroupConsolidator().Consolidate(tasks.Select(task => task.Result).ToList());
            }
            catch (AggregateException ex)
            {
                // Try to rethrow the first api exception we got
                UspsApiException apiException = ex.InnerExceptions.OfType<UspsApiException>().FirstOrDefault();
                if (apiException != null)
                {
                    throw apiException;
                }

                // If there are no api exceptions, just rethrow the first exception
                Exception exception = ex.InnerExceptions.FirstOrDefault();
                if (exception != null)
                {
                    throw exception;
                }

                // If there were no exceptions in the aggregate exception, just rethrow it
                throw;
            }
        }

        /// <summary>
        /// Start retrieving Express1 rates
        /// </summary>
        private Task<RateGroup> BeginRetrievingExpress1Rates(ShipmentEntity shipment, UspsAccountEntity express1AutoRouteAccount)
        {
            // Start getting rates from Express1
            ShipmentEntity express1Shipment = CreateShipmentCopy(express1AutoRouteAccount, shipment);

            return Task.Factory.StartNew(() =>
            {
                RateGroup rateGroup = new Express1UspsShipmentType().GetRates(express1Shipment);
                foreach (RateResult rate in rateGroup.Rates)
                {
                    PostalRateSelection tag = rate.Tag as PostalRateSelection;
                    if (tag != null)
                    {
                        rate.Tag = new UspsPostalRateSelection(tag.ServiceType, tag.ConfirmationType, express1AutoRouteAccount);
                    }
                }
                return rateGroup;
            });
        }

        /// <summary>
        /// Create a copy of the shipment, using the specified account
        /// </summary>
        private ShipmentEntity CreateShipmentCopy(UspsAccountEntity account, ShipmentEntity shipment)
        {
            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);

            shipmentType.UseAccountForShipment(account, clonedShipment);

            clonedShipment.Postal.Usps.RateShop = false;

            return clonedShipment;
        }

        /// <summary>
        /// Create a task that will return an empty list of rates
        /// </summary>
        private static Task<RateGroup> CreateEmptyExpress1RatesTask()
        {
            // Create a dummy task that will return an empty result
            TaskCompletionSource<RateGroup> completionSource = new TaskCompletionSource<RateGroup>();
            completionSource.SetResult(new RateGroup(new List<RateResult>()));
            return completionSource.Task;
        }

        /// <summary>
        /// Conditionally adds the usps rate promotion footnote based on the contract type of the account associated with the shipment
        /// and whether the shipping account conversion feature is restricted.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="rateGroup">The rate group.</param>
        protected void AddUspsRatePromotionFootnote(ShipmentEntity shipment, RateGroup rateGroup)
        {
            UspsAccountContractType contractType = (UspsAccountContractType)accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID).ContractType;
            UspsAccountEntity uspsAccount = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);

            // We may not want to show the conversion promotion for multi-user USPS accounts due 
            // to a limitation on USPS' side. (Tango will send these to ShipWorks via data contained
            // in ShipmentTypeFunctionality
            bool accountConversionRestricted = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.ShippingAccountConversion, shipmentType.ShipmentTypeCode).Level == EditionRestrictionLevel.Forbidden;
            TimeSpan accountCreatedTimespan = DateTime.UtcNow - uspsAccount.CreatedDate;

            if (contractType == UspsAccountContractType.Commercial &&
                (InterapptiveOnly.MagicKeysDown || accountCreatedTimespan.TotalDays >= MinNumberOfDaysBeforeShowingUspsPromo) &&
                !accountConversionRestricted)
            {
                // Show the promotional footer for discounted rates 
                rateGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(shipmentType, shipment, false));
            }
        }
    }

    public abstract class PostalRatingService : IRatingService
    {
        private readonly PostalShipmentType shipmentType;

        public PostalRatingService(PostalShipmentType shipmentType)
        {
            this.shipmentType = shipmentType;
        }

        public abstract RateGroup GetRates(ShipmentEntity shipment);

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected virtual RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(shipmentType));
                return errorRates;
            }

            RateGroup rates = new RateGroup(new List<RateResult>());

            if (!shipmentType.IsShipmentTypeRestricted)
            {
                // Only get counter rates if the shipment type has not been restricted
                rates = new PostalWebShipmentType().GetRates(shipment);
                rates.Rates.ForEach(x =>
                {
                    if (x.ProviderLogo != null)
                    {
                        // Only change existing logos; don't set logos for rates that don't have them
                        x.ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType);
                    }
                });
            }

            return rates;
        }
    }
}
