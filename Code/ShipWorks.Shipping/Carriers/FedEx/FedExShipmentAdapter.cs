using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Adapter for FedEx specific shipment information
    /// </summary>
    public class FedExShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public FedExShipmentAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.FedEx, nameof(shipment.FedEx));

            this.shipment = shipment;
        }

        /// <summary>
        /// Id of the FedEx account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.FedEx.FedExAccountID; }
            set { shipment.FedEx.FedExAccountID = value.GetValueOrDefault(); }
        }
    }
}
