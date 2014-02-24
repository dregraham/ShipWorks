﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
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
        /// Gets the configured Express1 Accounts
        /// </summary>
        public override List<EndiciaAccountEntity> Accounts
        {
            get
            {
                return EndiciaAccountManager.Express1Accounts;
            }
        }

        /// <summary>
        /// Create the Service Control
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public override ServiceControlBase CreateServiceControl(RateControl rateControl)
        {
            return new Express1EndiciaServiceControl(rateControl);
        }

        /// <summary>
        /// Process the label server shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                (new EndiciaApiClient()).ProcessShipment(shipment, this);
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
            return (EndiciaAccountManager.GetAccounts(EndiciaReseller.Express1).Any()) ?
                new Express1EndiciaBestRateBroker() : 
                new Express1EndiciaCounterRatesBroker();
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
