using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Adapter for Ups specific shipment information
    /// </summary>
    public class UpsShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public UpsShipmentAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Ups, nameof(shipment.Ups));

            this.shipment = shipment;
        }

        /// <summary>
        /// Id of the Ups account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.Ups.UpsAccountID; }
            set { shipment.Ups.UpsAccountID = value.GetValueOrDefault(); }
        }
    }
}
