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
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
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
            ShouldRetrieveExpress1Rates = false;

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
        public override StampsResellerType ResellerType
        {
            get { return StampsResellerType.StampsExpedited; }
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new StampsSettingsControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the Form used to do the setup for the Stamps.com API
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            IRegistrationPromotion promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
            return new UspsSetupWizard(promotion, true);
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            IStampsWebClient client = CreateWebClient();

            List<RateResult> stampsRates;
            RateGroup rateGroup;

            if (shipment.Postal.Stamps.RateShop)
            {
                stampsRates = GetRatesForAllAccounts(shipment, client);
                rateGroup = new RateGroup(stampsRates);
            }
            else
            {
                stampsRates = client.GetRates(shipment);
                rateGroup = new RateGroup(stampsRates);
                AddUspsRatePromotionFootnote(shipment, rateGroup);
            }

            return rateGroup;
        }

        /// <summary>
        /// Get rates for all available accounts
        /// </summary>
        private List<RateResult> GetRatesForAllAccounts(ShipmentEntity shipment, IStampsWebClient client)
        {
            List<StampsAccountEntity> uspsAccounts = AccountRepository.Accounts.ToList();

            try
            {
                List<Task<List<RateResult>>> tasks = uspsAccounts.Select(accountToCopy => CreateShipmentCopy(accountToCopy, shipment))
                    .Select(shipmentWithAccount => Task.Factory.StartNew(() => client.GetRates(shipmentWithAccount)))
                    .ToList();

                foreach (Task<List<RateResult>> task in tasks)
                {
                    task.Wait();
                }

                return new UspsRateConsolidator().Consolidate(tasks.Select(x => x.Result));
            }
            catch (AggregateException ex)
            {
                // Try to rethrow the first api exception we got
                StampsApiException apiException = ex.InnerExceptions.OfType<StampsApiException>().FirstOrDefault();
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
                if (shipment.Postal.Stamps.RateShop && AccountRepository.Accounts.Count() > 1)
                {
                    ProcessShipmentWithRateShopping(shipment);
                }
                else
                {
                    CreateWebClient().ProcessShipment(shipment);
                }
            }
            catch (StampsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Process the shipment using the account with the cheapest rate for the requested service
        /// </summary>
        private void ProcessShipmentWithRateShopping(ShipmentEntity shipment)
        {
            IStampsWebClient client = CreateWebClient();
            List<StampsAccountEntity> accounts = GetRatesFromApi(shipment).Rates
                    .OrderBy(x => x.Amount)
                    .Select(x => x.OriginalTag as UspsPostalRateSelection)
                    .Where(x => x.IsRateFor(shipment))
                    .Select(x => x.Accounts)
                    .FirstOrDefault();

            if (accounts == null)
            {
                throw new StampsException("Could not get rates for the specified service type");
            }

            foreach (StampsAccountEntity account in accounts)
            {
                try
                {
                    UseAccountForShipment(account, shipment);

                    client.ProcessShipment(shipment);
                    break;
                }
                catch (StampsInsufficientFundsException)
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
            else
            {
                // No accounts, so use the counter rates broker to allow the user to
                // sign up for the account. We can use the StampsCounterRateAccountRepository 
                // here because the underlying accounts being used are the same.
                return new UspsCounterRatesBroker(new StampsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance));
            }
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        protected override IEnumerable<IEntityField2> GetRatingFields(ShipmentEntity shipment)
        {
            return base.GetRatingFields(shipment)
                .Concat(new[]
                {
                    shipment.Postal.Stamps.Fields[StampsShipmentFields.RateShop.FieldIndex]
                });
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new UspsProfileControl(ShipmentTypeCode.Usps);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            profile.Postal.Stamps.RateShop = true;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Stamps != null)
            {
                StampsShipmentEntity stampsShipment = shipment.Postal.Stamps;
                StampsProfileEntity stampsProfile = profile.Postal.Stamps;

                ShippingProfileUtility.ApplyProfileValue(stampsProfile.RateShop, stampsShipment, StampsShipmentFields.RateShop);
            }
        }

        /// <summary>
        /// Create a copy of the shipment, using the specified account
        /// </summary>
        private static ShipmentEntity CreateShipmentCopy(StampsAccountEntity account, ShipmentEntity shipment)
        {
            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);

            UseAccountForShipment(account, clonedShipment);

            return clonedShipment;
        }

        /// <summary>
        /// Update the shipment to use the specified account
        /// </summary>
        private static void UseAccountForShipment(StampsAccountEntity account, ShipmentEntity shipment)
        {
            shipment.Postal.Stamps.StampsAccountID = account.StampsAccountID;

            if (shipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(account, string.Empty, shipment, "Origin");
            }
        }
    }
}
