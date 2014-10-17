﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Shipment type for Express 1 shipments.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public class Express1EndiciaShipmentType : EndiciaShipmentType
    {
        /// <summary>
        /// Create an instance of the Express1 Endicia Shipment Type
        /// </summary>
        public Express1EndiciaShipmentType()
        {
            AccountRepository = new Express1EndiciaAccountRepository();
        }

        /// <summary>
        /// Postal Shipment Type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.Express1Endicia;
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
                    (ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Stamps) || 
                    ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Stamps)) ?
                    "USPS (Express1 for Endicia)" : "USPS (Express1)";
            }
        }

        /// <summary>
        /// Reseller type
        /// </summary>
        public override EndiciaReseller EndiciaReseller
        {
            get
            {
                return EndiciaReseller.Express1;
            }
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        public override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new Express1EndiciaShipmentProcessingSynchronizer();
        }

        /// <summary>
        /// Create the Service Control
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new Express1EndiciaServiceControl(rateControl);
        }

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            ICarrierAccountRepository<EndiciaAccountEntity> originalAccountRepository = AccountRepository;
            ICertificateInspector originalCertificateInspector = CertificateInspector;

            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                AccountRepository = new Express1EndiciaCounterAccountRepository(TangoCounterRatesCredentialStore.Instance);
                CertificateInspector = new CertificateInspector(TangoCounterRatesCredentialStore.Instance.Express1EndiciaCertificateVerificationData);

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
        /// Process the label server shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                (new EndiciaApiClient(AccountRepository, LogEntryFactory, CertificateInspector)).ProcessShipment(shipment, this);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Void the given endicia shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            try
            {
                Express1EndiciaCustomerServiceClient.RequestRefund(shipment);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Create the setup wizard for configuring an Express 1 account.
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            Express1Registration registration = new Express1Registration(ShipmentTypeCode, new EndiciaExpress1RegistrationGateway(), new EndiciaExpress1RegistrationRepository(), 
                new EndiciaExpress1PasswordEncryptionStrategy(), new Express1RegistrationValidator());

            EndiciaAccountManagerControl accountManagerControl = new EndiciaAccountManagerControl();
            EndiciaOptionsControl optionsControl = new EndiciaOptionsControl(EndiciaReseller.Express1);
            EndiciaBuyPostageDlg postageDlg = new EndiciaBuyPostageDlg();

            return new Express1SetupWizard(postageDlg, accountManagerControl, optionsControl, registration, EndiciaAccountManager.Express1Accounts);
        }

        /// <summary>
        /// Get the Express1 MailClass code for the given service
        /// </summary>
        public override string GetMailClassCode(PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            // Express1 is not supporting the July 2013 Express/Priority Express updates, so this is the same
            // as the virtual method it overrides with the exception of ExpressMail returns "Express" instead
            // of "PriorityExpress"
            switch (serviceType)
            {
                case PostalServiceType.ExpressMail: return "Express";
                case PostalServiceType.FirstClass: return "First";
                case PostalServiceType.LibraryMail: return "LibraryMail";
                case PostalServiceType.MediaMail: return "MediaMail";
                case PostalServiceType.StandardPost: return "StandardPost";
                case PostalServiceType.ParcelSelect: return "ParcelSelect";
                case PostalServiceType.PriorityMail: return "Priority";
                case PostalServiceType.CriticalMail: return "CriticalMail";

                case PostalServiceType.InternationalExpress: return "ExpressMailInternational";
                case PostalServiceType.InternationalPriority: return "PriorityMailInternational";

                case PostalServiceType.InternationalFirst:
                    {
                        return PostalUtility.IsEnvelopeOrFlat(packagingType) ? "FirstClassMailInternational" : "FirstClassPackageInternationalService";
                    }
            }

            if (ShipmentTypeManager.IsEndiciaDhl(serviceType))
            {
                return EnumHelper.GetApiValue(serviceType);
            }

            throw new EndiciaException(string.Format("{0} is not supported when shipping with Endicia.", PostalUtility.GetPostalServiceTypeDescription(serviceType)));
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Express1 for Endicia shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an Express1EndiciaBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            IBestRateShippingBroker broker = new NullShippingBroker();
            if (EndiciaAccountManager.GetAccounts(EndiciaReseller.Express1).Any())
            {
                // Only use an Express1 broker if there is an account. We no longer want to
                // get Express1 counter rates
                broker = new Express1EndiciaBestRateBroker();
            }

            return broker;
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates
        {
            get { return true; }
        }
    }
}
