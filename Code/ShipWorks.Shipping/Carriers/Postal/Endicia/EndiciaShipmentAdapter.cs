using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Adapter for Endicia specific shipment information
    /// </summary>
    public class EndiciaShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public EndiciaShipmentAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal, nameof(shipment.Postal));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal.Endicia, nameof(shipment.Postal.Endicia));

            this.shipment = shipment;
        }

        /// <summary>
        /// Id of the Endicia account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.Postal.Endicia.EndiciaAccountID; }
            set { shipment.Postal.Endicia.EndiciaAccountID = value.GetValueOrDefault(); }
        }
    }
}
