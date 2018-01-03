using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;
using System;
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
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsPostProcessingMessage(IGlobalPostLabelNotification globalPostNotification, IDateTimeProvider dateTimeProvider)
        {
            this.globalPostNotification = globalPostNotification;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Show messages that apply to the given shipment
        /// </summary>
        public void Show(IEnumerable<IShipmentEntity> processedShipments)
        {
            bool showNotificationForShipment = processedShipments.Any(s => s.Processed && ShowNotifiactionForShipment(s.Postal));

            if (showNotificationForShipment && globalPostNotification.AppliesToCurrentUser())
            {
                globalPostNotification.Show();
            }
        }

        /// <summary>
        /// Should we show the notification
        /// </summary>
        private bool ShowNotifiactionForShipment(IPostalShipmentEntity shipment)
        {
            return IsdGapLabel(shipment) || 
                PostalUtility.IsGlobalPost((PostalServiceType) shipment.Service);
        }

        /// <summary>
        /// Determines whether the shipment is a Gap shipment.
        /// </summary>
        private bool IsdGapLabel(IPostalShipmentEntity shipment)
        {
            if (dateTimeProvider.Now < new DateTime(2018, 1, 21) && !InterapptiveOnly.MagicKeysDown)
            {
                return false;
            }

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
