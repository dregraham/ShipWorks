﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Rating service for the Usps carrier
    /// </summary>
    [Component(Service = typeof(IUspsRatingService))]
    public class UspsRatingService : PostalRatingService, ISupportExpress1Rates, IUspsRatingService
    {
        private const int MinNumberOfDaysBeforeShowingUspsPromo = 14;

        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ICachedRatesService cachedRatesService;
        private readonly IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory;
        protected ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository;
        private bool shouldRetrieveExpress1Rates;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsRatingService(IDateTimeProvider dateTimeProvider,
            ICachedRatesService cachedRatesService,
            IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory,
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository)
            : base(ratingServiceFactory, shipmentTypeFactory)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.cachedRatesService = cachedRatesService;
            this.ratingServiceFactory = ratingServiceFactory;
            this.accountRepository = accountRepository;

            // Default to true so that non-Best Rate calls will get Express1 rates if auto-route is enabled.
            shouldRetrieveExpress1Rates = true;
        }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment) => GetRates(shipment, null);

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment, TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            // Take this opportunity to try to update contract type of the account
            UspsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
            UpdateContractType(account);

            // Get counter rates if we don't have any Endicia accounts, letting the Postal shipment type take care of caching
            // since it should be using a different cache key
            try
            {
                RateGroup rates = accountRepository.Accounts.Any(a => a.PendingInitialAccount != (int) UspsPendingAccountType.Create) ?
                    GetRatesInternal(shipment, telemetricResult) :
                    GetCounterRates(shipment);

                SortRateGroup(rates);

                return rates;
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get rates includes Express1 rates if specified
        /// </summary>
        /// <param name="shipment">The shipment to get rates for</param>
        /// <param name="retrieveExpress1Rates">should we retrieve express1 rates</param>
        public RateGroup GetRates(ShipmentEntity shipment, bool retrieveExpress1Rates)
        {
            shouldRetrieveExpress1Rates = retrieveExpress1Rates;

            // Check to see if the rate is cached, if not call the rating service
            return cachedRatesService.GetCachedRates<ShippingException>(shipment, GetRates);
        }

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            // We're going to be temporarily swapping these out to get counter rates, so
            // make a note of the original values
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> originalAccountRepository = accountRepository;

            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);
                accountRepository = new UspsCounterRateAccountRepository(TangoCredentialStore.Instance);

                // Fetch the rates now that we're setup to use counter rates
                return GetRates(shipment);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(Enumerable.Empty<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Usps));

                return errorRates;
            }
            finally
            {
                // Set everything back to normal
                accountRepository = originalAccountRepository;
            }
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        /// <param name="telemetricResult"></param>
        protected virtual RateGroup GetRatesInternal(ShipmentEntity shipment,
            TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            // Start getting Express1 rates if necessary so that they should hopefully be ready when we need them
            Task<RateGroup> express1RateTask = GetExpress1RatesIfNecessary(shipment);

            RateGroup rateGroup = shipment.Postal.Usps.RateShop ?
                GetRatesForAllAccounts(shipment, telemetricResult) :
                GetRatesForSpecifiedAccount(shipment, telemetricResult);

            return new UspsExpress1RateConsolidator().Consolidate(rateGroup, express1RateTask);
        }

        /// <summary>
        /// Start getting Express1 rates if necessary
        /// </summary>
        private Task<RateGroup> GetExpress1RatesIfNecessary(ShipmentEntity shipment)
        {
            UspsAccountEntity express1AutoRouteAccount = GetExpress1AutoRouteAccount((PostalPackagingType) shipment.Postal.PackagingType);

            return shouldRetrieveExpress1Rates && express1AutoRouteAccount != null && !shipment.Postal.NoPostage ?
                BeginRetrievingExpress1Rates(shipment, express1AutoRouteAccount) :
                CreateEmptyExpress1RatesTask();
        }

        /// <summary>
        /// Create a task that will return an empty list of rates
        /// </summary>
        private static Task<RateGroup> CreateEmptyExpress1RatesTask()
        {
            // Create a dummy task that will return an empty result
            return Task.FromResult(new RateGroup(Enumerable.Empty<RateResult>()));
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
                RateGroup rateGroup = ratingServiceFactory[ShipmentTypeCode.Express1Usps].GetRates(express1Shipment);
                foreach (RateResult rate in rateGroup.Rates)
                {
                    PostalRateSelection tag = rate.Tag as PostalRateSelection;
                    if (tag != null)
                    {
                        rate.Tag = new UspsPostalRateSelection(tag.ServiceType, express1AutoRouteAccount);
                    }
                }
                return rateGroup;
            });
        }

        /// <summary>
        /// Get the Express1 account that should be used for auto routing, or null if we should not auto route
        /// </summary>
        private static UspsAccountEntity GetExpress1AutoRouteAccount(PostalPackagingType packagingType)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            bool isExpress1Restricted = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Usps).IsShipmentTypeRestricted;
            bool shouldUseExpress1 = settings.UspsAutomaticExpress1 && !isExpress1Restricted &&
                                     Express1Utilities.IsValidPackagingType(null, packagingType);

            return shouldUseExpress1 ? UspsAccountManager.GetAccount(settings.UspsAutomaticExpress1Account) : null;
        }

        /// <summary>
        /// Get rates for all available accounts
        /// </summary>
        private RateGroup GetRatesForAllAccounts(ShipmentEntity shipment, TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            List<UspsAccountEntity> uspsAccounts = accountRepository.Accounts.ToList();
            shouldRetrieveExpress1Rates = false;

            try
            {
                List<RateGroup> rateGroupsToConsolidate = null;

                if (telemetricResult != null)
                {
                    telemetricResult.RunTimedEvent(TelemetricEventType.GetRates,
                        () => rateGroupsToConsolidate = GetRateGroupsForAllAccounts(shipment, uspsAccounts));
                }
                else
                {
                    rateGroupsToConsolidate = GetRateGroupsForAllAccounts(shipment, uspsAccounts);
                }

                return new UspsRateGroupConsolidator().Consolidate(rateGroupsToConsolidate);
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
            finally
            {
                shouldRetrieveExpress1Rates = true;
            }
        }

        /// <summary>
        /// Gets all rates groups for all accounts asynchornously
        /// </summary>
        private List<RateGroup> GetRateGroupsForAllAccounts(ShipmentEntity shipment, List<UspsAccountEntity> uspsAccounts)
        {
            List<Task<RateGroup>> tasks =
                uspsAccounts.Select(accountToCopy => CreateShipmentCopy(accountToCopy, shipment))
                    .Select(shipmentWithAccount => Task.Factory.StartNew(() => GetRates(shipmentWithAccount)))
                    .ToList();

            foreach (Task<RateGroup> task in tasks)
            {
                task.Wait();
            }

            List<RateGroup> rateGroupsToConsolidate = tasks.Select(task => task.Result).ToList();
            return rateGroupsToConsolidate;
        }

        /// <summary>
        /// Get rates for the account specified in the shipment
        /// </summary>
        private RateGroup GetRatesForSpecifiedAccount(ShipmentEntity shipment,
            TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            var results = telemetricResult != null ?
                telemetricResult.RunTimedEvent(TelemetricEventType.GetRates, () => CreateWebClient().GetRates(shipment)) :
                CreateWebClient().GetRates(shipment);

            var rates = results.rates
                .Do(r => r.ShipmentType = ShipmentTypeCode.Usps)
                .ToList();

            RateGroup rateGroup = new RateGroup(FilterRatesByExcludedServices(shipment, rates));
            AddUspsRatePromotionFootnote(shipment, rateGroup);

            foreach (var error in results.errors)
            {
                rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Usps, error));
            }

            return rateGroup;
        }

        /// <summary>
        /// Conditionally adds the usps rate promotion footnote based on the contract type of the account associated with the shipment
        /// and whether the shipping account conversion feature is restricted.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="rateGroup">The rate group.</param>
        private void AddUspsRatePromotionFootnote(ShipmentEntity shipment, RateGroup rateGroup)
        {
            UspsAccountContractType contractType = (UspsAccountContractType) accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID).ContractType;
            UspsAccountEntity uspsAccount = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);

            // We may not want to show the conversion promotion for multi-user USPS accounts due
            // to a limitation on USPS' side. (Tango will send these to ShipWorks via data contained
            // in ShipmentTypeFunctionality
            bool accountConversionRestricted = IsAccountConversionRestricted();
            TimeSpan accountCreatedTimespan = dateTimeProvider.UtcNow - uspsAccount.CreatedDate;

            if (contractType == UspsAccountContractType.Commercial &&
                (InterapptiveOnly.MagicKeysDown || accountCreatedTimespan.TotalDays >= MinNumberOfDaysBeforeShowingUspsPromo) &&
                !accountConversionRestricted)
            {
                // Show the promotional footer for discounted rates
                rateGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(shipmentTypeManager[ShipmentTypeCode.Usps], shipment, false));
            }
        }

        /// <summary>
        /// Uses the license service to determine whether account conversion is restricted.
        /// </summary>
        /// <returns><c>true</c> if [account is conversion restricted]; otherwise, <c>false</c>.</returns>
        private bool IsAccountConversionRestricted()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = scope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.ShippingAccountConversion, ShipmentTypeCode.Usps);

                return restrictionLevel == EditionRestrictionLevel.Forbidden;
            }
        }

        /// <summary>
        /// Create a copy of the shipment, using the specified account
        /// </summary>
        private static ShipmentEntity CreateShipmentCopy(UspsAccountEntity account, ShipmentEntity shipment)
        {
            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);

            UseAccountForShipment(account, clonedShipment);

            clonedShipment.Postal.Usps.RateShop = false;

            return clonedShipment;
        }

        /// <summary>
        /// Update the shipment to use the specified account
        /// </summary>
        private static void UseAccountForShipment(UspsAccountEntity account, ShipmentEntity shipment)
        {
            shipment.Postal.Usps.UspsAccountID = account.UspsAccountID;

            if (shipment.OriginOriginID == (int) ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(account, string.Empty, shipment, "Origin");
            }
        }

        /// <summary>
        /// Uses the USPS API to update the contract type of the account if it is unkown.
        /// </summary>
        /// <param name="account">The account.</param>
        private void UpdateContractType(UspsAccountEntity account)
        {
            if (account == null || account.PendingInitialAccount != (int) UspsPendingAccountType.None)
            {
                return;
            }

            // We want to update the contract if it's not in the cache (or dropped out) or if the contract type is unknown; the cache is used
            // so we don't have to perform this everytime, but does allow ShipWorks to handle cases where the contract type may have been
            // updated outside of ShipWorks.
            if (!UspsContractTypeCache.Contains(account.UspsAccountID) || UspsContractTypeCache.GetContractType(account.UspsAccountID) == UspsAccountContractType.Unknown)
            {
                try
                {
                    // Grab contract type from the USPS API
                    UspsAccountContractType contractType = CreateWebClient().GetContractType(account);

                    bool hasContractChanged = account.ContractType != (int) contractType;
                    account.ContractType = (int) contractType;

                    // Save the contract to the DB and update the cache
                    accountRepository.Save(account);
                    UspsContractTypeCache.Set(account.UspsAccountID, (UspsAccountContractType) account.ContractType);

                    if (hasContractChanged)
                    {
                        // Any cached rates are probably invalid now
                        RateCache.Instance.Clear();

                        // Only notify Tango of changes so it has the latest information (and cuts down on traffic)
                        using (var lifetimeScope = IoC.BeginLifetimeScope())
                        {
                            var tangoWebClient = lifetimeScope.Resolve<ITangoWebClient>();

                            tangoWebClient.LogUspsAccount(account);
                        }
                    }
                }
                catch (Exception exception)
                {
                    // Log the error
                    LogManager.GetLogger(GetType()).Error(
                        $"ShipWorks encountered an error when getting contract type for account {account.Username}.", exception);
                }
            }
        }

        /// <summary>
        /// Returns web client
        /// </summary>
        protected virtual IUspsWebClient CreateWebClient()
        {
            UspsShipmentType shipmentType = (UspsShipmentType) shipmentTypeManager[ShipmentTypeCode.Usps];
            shipmentType.AccountRepository = accountRepository;
            return shipmentType.CreateWebClient();
        }

        /// <summary>
        /// Returns the correct certificate inspector
        /// </summary>
        [SuppressMessage("SonarQube", "S3215: \"interface\" instances should not be cast to concrete types",
            Justification = "The cast is to detect whether we need to verify the SSL certificate")]
        protected ICertificateInspector CertificateInspector()
        {
            UspsCounterRateAccountRepository counterRateRepo = accountRepository as UspsCounterRateAccountRepository;

            if (counterRateRepo != null)
            {
                // The account repository is a CounterRate repo use a certificate inspector
                return new CertificateInspector(TangoCredentialStore.Instance.UspsCertificateVerificationData);
            }

            // we are not using the counter rate repo so
            // we can send back a trusting certificate inspector
            return new TrustingCertificateInspector();
        }

        /// <summary>
        /// Sort the rates in the rate group based on our preferences
        /// </summary>
        private void SortRateGroup(RateGroup rateGroup)
        {
            // Move all of the global post services to the top. Order by descending as we insert them to the
            // beginning, one at a time, so they will then be in alphabetical order which is what we want.
            IEnumerable<RateResult> globalPostPriority = rateGroup.Rates
                .Where(r => r.Description.Contains("GlobalPost"))
                .OrderByDescending(r => r.Description).ToList();

            foreach (RateResult rateResult in globalPostPriority)
            {
                rateGroup.Rates.Remove(rateResult);
                rateGroup.Rates.Insert(0, rateResult);
            }
        }
    }
}
