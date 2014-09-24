using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Shipment type for Express 1 for Stamps.com shipments.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = false)]    
    public class Express1StampsShipmentType : StampsShipmentType
    {
        /// <summary>
        /// Create an instance of the Express1 Stamps Shipment Type
        /// </summary>
        public Express1StampsShipmentType()
        {
            AccountRepository = new Express1StampsAccountRepository();
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
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts
        {
            get
            {
                return StampsAccountManager.Express1Accounts.Any();
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
            get { return true; }
        }

        /// <summary>
        /// Creates the Express1/Stamps service control.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new Express1StampsServiceControl(rateControl);
        }

        /// <summary>
        /// Creates the Express1/Stamps setup wizard.
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            Express1Registration registration = new Express1Registration(ShipmentTypeCode, new StampsExpress1RegistrationGateway(), new StampsExpress1RegistrationRepository(), new StampsExpress1PasswordEncryptionStrategy(), new Express1RegistrationValidator());

            StampsAccountManagerControl accountManagerControl = new StampsAccountManagerControl { IsExpress1 = true };
            StampsOptionsControl optionsControl = new StampsOptionsControl { IsExpress1 = true };
            StampsPurchasePostageDlg postageDialog = new StampsPurchasePostageDlg();

            return new Express1SetupWizard(postageDialog, accountManagerControl, optionsControl, registration, StampsAccountManager.Express1Accounts);
        }
        
        /// <summary>
        /// Creates the Express1/Stamps settings control.
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new StampsSettingsControl(true);
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps w/ Express1 profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new StampsProfileControl(ShipmentTypeCode);
        }
        
        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            ICarrierAccountRepository<StampsAccountEntity> originalAccountRepository = AccountRepository;
            ICertificateInspector originalCertificateInspector = CertificateInspector;

            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                AccountRepository = new Express1StampsCounterRatesAccountRepository(TangoCounterRatesCredentialStore.Instance);
                CertificateInspector = new CertificateInspector(TangoCounterRatesCredentialStore.Instance.Express1StampsCertificateVerificationData);

                // This call to GetRates won't be recursive since the counter rate account repository will return an account
                return GetRates(shipment);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(this));
                return errorRates;
            }
            finally
            {
                AccountRepository = originalAccountRepository;
                CertificateInspector = originalCertificateInspector;
            }
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        public override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new Express1StampsShipmentProcessingSynchronizer();
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
                shipment.Postal.Stamps.HidePostage = true;
                new StampsApiSession().ProcessShipment(shipment);
            }
            catch(StampsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Express1 for Stamps.com shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an Express1StampsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            if (StampsAccountManager.Express1Accounts.Any())
            {
                return new Express1StampsBestRateBroker();
            }
            
            return new Express1StampsCounterRatesBroker();
        }

        /// <summary>
        /// Will just assign the contract type of the account to Unknown and save the account to the repository.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void UpdateContractType(StampsAccountEntity account)
        {
            // If the ContractType is unknown, we must not have tried to check this account yet.
            // Just assign the contract type to NotApplicable; we don't need to worry about Express1 accounts
            if (account.ContractType == (int) StampsAccountContractType.Unknown)
            {
                account.ContractType = (int) StampsAccountContractType.NotApplicable;
                AccountRepository.Save(account);
            }
        }
    }
}
