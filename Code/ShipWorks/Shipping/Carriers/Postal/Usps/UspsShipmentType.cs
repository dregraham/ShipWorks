using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A shipment type for the generic USPS shipment type in ShipWorks. This is actually a
    /// Stamps.com Expedited shipment type (which is why it derives from the StampsShipmentType)
    /// that gets presented as USPS to the end user.
    /// </summary>
    public class UspsShipmentType : StampsShipmentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsShipmentType"/> class.
        /// </summary>
        public UspsShipmentType()
        {
            ShouldRetrieveExpress1Rates = true;

            // Use the "live" versions by default
            AccountRepository = new UspsAccountRepository();
            LogEntryFactory = new LogEntryFactory();
        }

        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Usps; }
        }

        /// <summary>
        /// Gets the type of the reseller.
        /// </summary>
        public override UspsResellerType ResellerType
        {
            get { return UspsResellerType.StampsExpedited; }
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new UspsSettingsControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the Form used to do the setup for the Stamps.com API
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            EnsureAccountsHaveCurrentContractData();

            IRegistrationPromotion promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
            return new UspsSetupWizard(promotion, true);
        }

        /// <summary>
        /// Ensure that all USPS accounts have up to date contract information
        /// </summary>
        private void EnsureAccountsHaveCurrentContractData()
        {
            Task[] tasks = AccountRepository.Accounts
                .Select(account => Task.Factory.StartNew(() => UpdateContractType(account)))
                .ToArray();

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetRatesInternal(ShipmentEntity shipment)
        {
            // Start getting Express1 rates if necessary so that they should hopefully be ready when we need them
            var express1RateTask = GetExpress1RatesIfNecessary(shipment);

            RateGroup rateGroup = shipment.Postal.Usps.RateShop ?
                GetRatesForAllAccounts(shipment) :
                GetCachedRates<UspsException>(shipment, GetRatesForSpecifiedAccount);

            return new UspsExpress1RateConsolidator().Consolidate(rateGroup, express1RateTask);
        }

        /// <summary>
        /// Get rates for the account specified in the shipment
        /// </summary>
        private RateGroup GetRatesForSpecifiedAccount(ShipmentEntity shipment)
        {
            List<RateResult> stampsRates = CreateWebClient().GetRates(shipment);
            stampsRates.ForEach(r => r.ShipmentType = ShipmentTypeCode);

            RateGroup rateGroup = new RateGroup(stampsRates);
            AddUspsRatePromotionFootnote(shipment, rateGroup);
            return rateGroup;
        }

        /// <summary>
        /// Get rates for all available accounts
        /// </summary>
        private RateGroup GetRatesForAllAccounts(ShipmentEntity shipment)
        {
            List<UspsAccountEntity> uspsAccounts = AccountRepository.Accounts.ToList();

            // We are creating a new shipment type here so we can call get rates and not call Express1 Rates.
            // We thought of just turning off ShouldRetrieveExpress1Rates, but worried that might cause unexpected behavior
            //   in a multi-threaded situation.
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

                return new UspsRateGroupConsolidator().Consolidate(tasks.Select(task=>task.Result).ToList());
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
        /// Start getting Express1 rates if necessary
        /// </summary>
        private Task<RateGroup> GetExpress1RatesIfNecessary(ShipmentEntity shipment)
        {
            UspsAccountEntity express1AutoRouteAccount = GetExpress1AutoRouteAccount((PostalPackagingType)shipment.Postal.PackagingType);
            return ShouldRetrieveExpress1Rates && express1AutoRouteAccount != null ? 
                BeginRetrievingExpress1Rates(shipment, express1AutoRouteAccount) : 
                CreateEmptyExpress1RatesTask();
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
        /// Start retrieving Express1 rates
        /// </summary>
        private static Task<RateGroup> BeginRetrievingExpress1Rates(ShipmentEntity shipment, UspsAccountEntity express1AutoRouteAccount)
        {
            // Start getting rates from Express1
            ShipmentEntity express1Shipment = CreateShipmentCopy(express1AutoRouteAccount, shipment);
            express1Shipment.ShipmentType = (int) ShipmentTypeCode.Express1Stamps;

            return Task.Factory.StartNew(() =>
            {
                RateGroup rateGroup = new Express1StampsShipmentType().GetRates(express1Shipment);
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
        /// Get the Express1 account that should be used for auto routing, or null if we should not auto route
        /// </summary>
        private static UspsAccountEntity GetExpress1AutoRouteAccount(PostalPackagingType packagingType)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            bool isExpress1Restricted = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Stamps).IsShipmentTypeRestricted;
            bool shouldUseExpress1 = settings.UspsAutomaticExpress1 && !isExpress1Restricted &&
                                     Express1Utilities.IsValidPackagingType(null, packagingType);

            return shouldUseExpress1 ? UspsAccountManager.GetAccount(settings.UspsAutomaticExpress1Account) : null;
        }

        /// <summary>
        /// Creates the USPS service control.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new UspsServiceControl(ShipmentTypeCode, rateControl);
        }

        /// <summary>
        /// Allows the shipment type to run any pre-processing work that may need to be performed prior to
        /// actually processing the shipment. In most cases this is checking to see if an account exists
        /// and will call the counterRatesProcessing callback provided when trying to process a shipment
        /// without any accounts for this shipment type in ShipWorks, otherwise the shipment is unchanged.
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="counterRatesProcessing"></param>
        /// <param name="selectedRate"></param>
        /// <returns>
        /// The updates shipment (or shipments) that is ready to be processed. A null value may
        /// be returned to indicate that processing should be halted completely.
        /// </returns>
        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing, RateResult selectedRate)
        {
            // We want to perform the processing of the base ShipmentType and not that of the Stamps.com shipment type
            IShipmentProcessingSynchronizer synchronizer = GetProcessingSynchronizer();
            ShipmentTypePreProcessor preProcessor = new ShipmentTypePreProcessor();

            return preProcessor.Run(synchronizer, shipment, counterRatesProcessing, selectedRate);
        }

        /// <summary>
        /// Process the shipment. Overridden here, so overhead of Express1 can be removed.
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                if (ShouldRateShop(shipment) || ShouldTestExpress1Rates(shipment))
                {
                    ProcessShipmentWithRates(shipment);
                }
                else
                {
                    CreateWebClient().ProcessShipment(shipment);
                }
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Should we rate shop before processing
        /// </summary>
        private bool ShouldRateShop(ShipmentEntity shipment)
        {
            return shipment.Postal.Usps.RateShop && 
                AccountRepository.Accounts.Count() > 1;
        }

        /// <summary>
        /// Get the Express1 account that should be used for auto routing.
        /// Returns null if auto routing should not be used.
        /// </summary>
        private static bool ShouldTestExpress1Rates(ShipmentEntity shipment)
        {
            return Express1Utilities.IsPostageSavingService((PostalServiceType)shipment.Postal.Service) &&
                GetExpress1AutoRouteAccount((PostalPackagingType)shipment.Postal.PackagingType) != null;
        }

        /// <summary>
        /// Process the shipment using the account with the cheapest rate for the requested service
        /// </summary>
        private void ProcessShipmentWithRates(ShipmentEntity shipment)
        {
            IUspsWebClient client = CreateWebClient();
            List<UspsAccountEntity> accounts = GetRates(shipment).Rates
                    .OrderBy(x => x.Amount)
                    .Select(x => x.OriginalTag as UspsPostalRateSelection)
                    .Where(x => x.IsRateFor(shipment))
                    .Select(x => x.Accounts)
                    .FirstOrDefault();

            if (accounts == null)
            {
                throw new UspsException("Could not get rates for the specified service type");
            }

            foreach (UspsAccountEntity account in accounts)
            {
                try
                {
                    if (account.UspsReseller == (int)UspsResellerType.Express1)
                    {
                        shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;

                        ShipmentType express1ShipmentType = ShipmentTypeManager.GetType(shipment);
                        shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
                        UseAccountForShipment(account, shipment);
                        
                        express1ShipmentType.UpdateDynamicShipmentData(shipment);
                        express1ShipmentType.ProcessShipment(shipment);
                    }
                    else
                    {
                        UseAccountForShipment(account, shipment);
                        client.ProcessShipment(shipment);
                    }

                    break;
                }
                catch (UspsInsufficientFundsException)
                {
                    if (ReferenceEquals(account, accounts.Last()))
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the USPS (Stamps.com Expedited) 
        /// shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a StampsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            if (AccountRepository.Accounts.Any())
            {
                // We have an account, so use the normal broker
                return new UspsBestRateBroker(this, AccountRepository);
            }
            
            // No accounts, so use the counter rates broker to allow the user to
            // sign up for the account. We can use the StampsCounterRateAccountRepository 
            // here because the underlying accounts being used are the same.
            return new UspsCounterRatesBroker(new UspsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance));
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        protected override IEnumerable<IEntityField2> GetRatingFields(ShipmentEntity shipment)
        {
            return base.GetRatingFields(shipment)
                .Concat(new[]
                {
                    shipment.Postal.Usps.Fields[UspsShipmentFields.RateShop.FieldIndex]
                });
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            profile.Postal.Usps.RateShop = true;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Usps != null)
            {
                UspsShipmentEntity uspsShipment = shipment.Postal.Usps;
                UspsProfileEntity uspsProfile = profile.Postal.Usps;

                ShippingProfileUtility.ApplyProfileValue(uspsProfile.RateShop, uspsShipment, UspsShipmentFields.RateShop);
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

            if (shipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(account, string.Empty, shipment, "Origin");
            }
        }
    }
}
