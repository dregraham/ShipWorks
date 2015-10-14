using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    public class iParcelShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public iParcelShipmentAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.IParcel, nameof(shipment.IParcel));

            this.shipment = shipment;
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.IParcel.IParcelAccountID; }
            set { shipment.IParcel.IParcelAccountID = value.GetValueOrDefault(); }
        }
    }
}
