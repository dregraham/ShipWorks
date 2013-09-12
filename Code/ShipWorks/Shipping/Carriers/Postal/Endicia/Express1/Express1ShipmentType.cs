using System.Collections.Generic;
using Interapptive.Shared.Utility;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Shipment type for Express 1 shipments.
    /// </summary>
    public class Express1ShipmentType : EndiciaShipmentType
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
        public override ServiceControlBase CreateServiceControl()
        {
            return new Express1ServiceControl();
        }

        /// <summary>
        /// Process the label server shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                EndiciaApiClient.ProcessShipment(shipment, this);
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
                Express1CustomerServiceClient.RequestRefund(shipment);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Create the setup wizard for configuring an Express 1 account.
        /// </summary>
        public override Form CreateSetupWizard()
        {
            return new Express1SetupWizard();
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
    }
}
