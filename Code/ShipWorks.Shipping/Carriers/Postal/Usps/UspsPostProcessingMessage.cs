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
            bool hasGlobalPost = processedShipments.Any(IsProcessedGlobalPost);

            if (hasGlobalPost)
            {
                if (globalPostNotification.AppliesToCurrentUser())
                {
                    globalPostNotification.Show();
                }
            }
        }

        /// <summary>
        /// Determines whether the shipment is a Processed GlobalPost shipment.
        /// </summary>
        private static bool IsProcessedGlobalPost(IShipmentEntity shipment)
        {
            if (shipment.Processed &&
                shipment.ShipmentType == (int)ShipmentTypeCode.Usps &&
                shipment.Postal != null)
            {
                // We have a processed USPS shipment. Now check for the GlobalPost service type
                return PostalUtility.IsGlobalPost((PostalServiceType)shipment.Postal.Service);
            }

            return false;
        }
    }
}
