using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    public class OnTracShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public OnTracShipmentAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.OnTrac, nameof(shipment.OnTrac));

            this.shipment = shipment;
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.OnTrac.OnTracAccountID; }
            set { shipment.OnTrac.OnTracAccountID = value.GetValueOrDefault(); }
        }
    }
}
