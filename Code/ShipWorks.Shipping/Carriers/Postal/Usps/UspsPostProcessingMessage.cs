using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Show message for Usps Shipments after processing
    /// </summary>
    [KeyedComponent(typeof(ICarrierPostProcessingMessage), ShipmentTypeCode.Usps)]
    public class UspsPostProcessingMessage : ICarrierPostProcessingMessage
    {
        private readonly IGlobalPostLabelNotification globalPostNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsPostProcessingMessage(IGlobalPostLabelNotification globalPostNotification)
        {
            this.globalPostNotification = globalPostNotification;
        }

        /// <summary>
        /// Show messages that apply to the given shipment
        /// </summary>
        public void Show(IEnumerable<IShipmentEntity> processedShipments)
        {
            bool showNotificationForShipment = processedShipments.Any(s => ShowNotifiactionForShipment(s.Postal));

            if (showNotificationForShipment && globalPostNotification.AppliesToCurrentUser())
            {
                globalPostNotification.Show();
            }
        }

        /// <summary>
        /// Should we show the notification
        /// </summary>
        private static bool ShowNotifiactionForShipment(IPostalShipmentEntity shipment)
        {
            return IsdGapLabel(shipment) || 
                PostalUtility.IsGlobalPost((PostalServiceType) shipment.Service);
        }

        /// <summary>
        /// Determines whether the shipment is a Gap shipment.
        /// </summary>
        private static bool IsdGapLabel(IPostalShipmentEntity shipment)
        {
            if (shipment.Service == (int) PostalServiceType.InternationalFirst &&
                shipment.CustomsContentType != (int) PostalCustomsContentType.Documents &&
                (shipment.PackagingType == (int) PostalPackagingType.Envelope ||
                    shipment.PackagingType == (int) PostalPackagingType.LargeEnvelope))
            { 
                return true;
            }

            return false;
        }
    }
}
