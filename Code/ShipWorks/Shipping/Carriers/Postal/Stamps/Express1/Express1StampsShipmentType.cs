using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Shipment type for Express 1 for Stamps.com shipments.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = false)]    
    public class Express1StampsShipmentType : UspsShipmentType
    {
        /// <summary>
        /// Create an instance of the Express1 Stamps Shipment Type
        /// </summary>
        public Express1StampsShipmentType()
        {
            AccountRepository = new Express1UspsAccountRepository();
        }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.Express1Stamps;
            }
        }

        /// <summary>
        /// Gets the type of the reseller.
        /// </summary>
        public override UspsResellerType ResellerType
        {
            get { return UspsResellerType.Express1; }
        }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts
        {
            get
            {
                return UspsAccountManager.Express1Accounts.Any();
            }
        }

        /// <summary>
        /// The user-displayable name of the shipment type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string ShipmentTypeName
        {
            get
            {
                return
                    (ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Endicia) ||
                    ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Endicia)) ?
                    "USPS (Express1 for Stamps)" : "USPS (Express1)";
            }
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates
        {
            get { return false; }
        }

        /// <summary>
        /// Creates the web client to use to contact the underlying carrier API.
        /// </summary>
        /// <returns>An instance of IStampsWebClient.</returns>
        public override IUspsWebClient CreateWebClient()
        {
            return new Express1UspsWebClient(AccountRepository, new LogEntryFactory(), CertificateInspector);
        }

        /// <summary>
        /// Creates the Express1/Stamps service control.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new Express1UspsServiceControl(rateControl);
        }

        /// <summary>
        /// Creates the Express1/Stamps setup wizard.
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            Express1Registration registration = new Express1Registration(ShipmentTypeCode, new UspsExpress1RegistrationGateway(), new UspsExpress1RegistrationRepository(), new UspsExpress1PasswordEncryptionStrategy(), new Express1RegistrationValidator());

            UspsAccountManagerControl accountManagerControl = new UspsAccountManagerControl { StampsResellerType = UspsResellerType.Express1 };
            UspsOptionsControl optionsControl = new UspsOptionsControl { ShipmentTypeCode = ShipmentTypeCode.Express1Stamps };
            UspsPurchasePostageDlg postageDialog = new UspsPurchasePostageDlg();

            return new Express1SetupWizard(postageDialog, accountManagerControl, optionsControl, registration, UspsAccountManager.Express1Accounts);
        }
        
        /// <summary>
        /// Creates the Express1/Stamps settings control.
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new UspsSettingsControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps w/ Express1 profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new Express1UspsProfileControl();
        }
        
        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            return GetCachedRates<UspsException>(shipment, entity => { throw new UspsException("An account is required to view Express1 rates."); });
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        public override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new Express1UspsShipmentProcessingSynchronizer();
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
        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, System.Func<CounterRatesProcessingArgs, System.Windows.Forms.DialogResult> counterRatesProcessing, RateResult selectedRate)
        {
            // We want to perform the processing of the base ShipmentType and not that of the Stamps.com shipment type
            IShipmentProcessingSynchronizer synchronizer = GetProcessingSynchronizer();
            ShipmentTypePreProcessor preProcessor = new ShipmentTypePreProcessor();

            return preProcessor.Run(synchronizer, shipment, counterRatesProcessing, selectedRate);
        }

        /// <summary>
        /// Processes a shipment.
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                // Express1 for Stamps.com requires that postage be hidden per their negotiated
                // service agreement
                shipment.Postal.Usps.HidePostage = true;
                new Express1UspsWebClient().ProcessShipment(shipment);
            }
            catch(UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Express1 for Stamps.com shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an Express1UspsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Will just assign the contract type of the account to Unknown and save the account to the repository.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void UpdateContractType(UspsAccountEntity account)
        {
            // If the ContractType is unknown, we must not have tried to check this account yet.
            // Just assign the contract type to NotApplicable; we don't need to worry about Express1 accounts
            if (account != null && account.ContractType == (int)UspsAccountContractType.Unknown)
            {
                account.ContractType = (int)UspsAccountContractType.NotApplicable;
                AccountRepository.Save(account);
            }
        }        
    }
}
