using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Shipment type for Express 1 for USPS shipments.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = false)]    
    public class Express1UspsShipmentType : UspsShipmentType
    {
        /// <summary>
        /// Create an instance of the Express1 USPS Shipment Type
        /// </summary>
        public Express1UspsShipmentType()
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
                return ShipmentTypeCode.Express1Usps;
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
                return "USPS (Express1)";
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
        /// <returns>An instance of IUspsWebClient.</returns>
        public override IUspsWebClient CreateWebClient()
        {
            return new Express1UspsWebClient(AccountRepository, new LogEntryFactory(), CertificateInspector);
        }

        /// <summary>
        /// Creates the Express1/USPS service control.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new Express1UspsServiceControl(rateControl);
        }

        /// <summary>
        /// Creates the Express1/USPS setup wizard.
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            Express1Registration registration = new Express1Registration(ShipmentTypeCode, new UspsExpress1RegistrationGateway(), new UspsExpress1RegistrationRepository(), new UspsExpress1PasswordEncryptionStrategy(), new Express1RegistrationValidator());

            UspsAccountManagerControl accountManagerControl = new UspsAccountManagerControl { UspsResellerType = UspsResellerType.Express1 };
            UspsOptionsControl optionsControl = new UspsOptionsControl { ShipmentTypeCode = ShipmentTypeCode.Express1Usps };
            UspsPurchasePostageDlg postageDialog = new UspsPurchasePostageDlg();

            return new Express1SetupWizard(postageDialog, accountManagerControl, optionsControl, registration, UspsAccountManager.Express1Accounts);
        }
        
        /// <summary>
        /// Creates the Express1/USPS settings control.
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new UspsSettingsControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the UserControl used to handle USPS w/ Express1 profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new Express1UspsProfileControl();
        }

        /// <summary>
        /// Update the dyamic data of the shipment
        /// </summary>
        /// <param name="shipment"></param>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetRatesInternal(ShipmentEntity shipment)
        {
            // Overridden here otherwise relying on the UspsShipmentType to get rates
            // would result in infinite recursion when using auto-routing since the UspsShipmentType 
            // is just calling GetRatesInternal on an Express1UspsShipmentType which then creates a new
            // Express1UspsShipmentType and gets rates, and on and on...

            return GetCachedRates<UspsException>(shipment, GetRatesFromApi);
        }

        /// <summary>
        /// Gets the rates from the Exprss1 API.
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            List<RateResult> rateResults = CreateWebClient().GetRates(shipment);
            RateGroup rateGroup = new RateGroup(rateResults);

            if (UspsAccountManager.UspsAccounts.All(a => a.ContractType != (int) UspsAccountContractType.Reseller))
            {
                rateGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, true));
            }

            return rateGroup;
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
        /// Processes a shipment.
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                // Express1 for USPS requires that postage be hidden per their negotiated
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
        /// Gets an instance to the best rate shipping broker for the Express1 for USPS shipment type based on the shipment configuration.
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

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            // Don't update Express1 entries because they could overwrite Usps records
        }

        /// <summary>
        /// Gets all of the confirmation types that are available to a particular implementation of PostalShipmentType.
        /// </summary>
        /// <returns>A collection of all the confirmation types that are available to a Express1 (USPS) shipment.</returns>
        public override IEnumerable<PostalConfirmationType> GetAllConfirmationTypes()
        {
            // The adult signature types are not available
            return new List<PostalConfirmationType>
            {
                PostalConfirmationType.None,
                PostalConfirmationType.Delivery,
                PostalConfirmationType.Signature
            };
        }

        /// <summary>
        /// Determines if delivery\signature confirmation is available for the given service
        /// </summary>
        public override List<PostalConfirmationType> GetAvailableConfirmationTypes(string countryCode, PostalServiceType service, PostalPackagingType? packaging)
        {
            List<PostalConfirmationType> confirmationTypes = base.GetAvailableConfirmationTypes(countryCode, service, packaging);

            return confirmationTypes.Where(ct => ct != PostalConfirmationType.AdultSignatureRestricted && ct != PostalConfirmationType.AdultSignatureRequired).ToList();
        }

    }
}
