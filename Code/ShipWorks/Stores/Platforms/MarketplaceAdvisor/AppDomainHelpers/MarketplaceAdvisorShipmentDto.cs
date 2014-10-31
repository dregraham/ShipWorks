using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers
{
    /// <summary>
    /// Shipment DTO
    /// </summary>
    /// <remarks>
    /// The entity classes don't serialize correctly, we need to provide a dto
    /// </remarks>
    [Serializable]
    public class MarketplaceAdvisorShipmentDto
    {
        /// <summary>
        /// Constructor for serialization
        /// </summary>
        public MarketplaceAdvisorShipmentDto()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorShipmentDto(ShipmentEntity shipment)
        {
            Processed = shipment.Processed;
            Voided = shipment.Voided;
            ShipDate = shipment.ShipDate;
            TrackingNumber = shipment.TrackingNumber;
            ShipmentCost = shipment.ShipmentCost;
            ShippingMethodCode = MarketplaceAdvisorUtility.GetShippingMethodID(shipment);
        }

        public bool Processed { get; set; }
        public bool Voided { get; set; }
        public DateTime ShipDate { get; set; }
        public string TrackingNumber { get; set; }
        public decimal ShipmentCost { get; set; }
        public int ShippingMethodCode { get; set; }
    }
}