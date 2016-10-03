namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    ///  A pre-processor that makes sure shipment type is configured prior to processing
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.ShipmentTypePreProcessor" />
    public class UpsShipmentTypePreProcessor : ShipmentTypePreProcessor
    {
        private readonly IShippingManager shippingManager;
        private readonly ShipmentTypeCode shipmentTypeCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsShipmentTypePreProcessor"/> class.
        /// </summary>
        /// <param name="shippingManager">The shipping manager.</param>
        /// <param name="shipmentTypeCode">The shipment type code.</param>
        public UpsShipmentTypePreProcessor(IShippingManager shippingManager, ShipmentTypeCode shipmentTypeCode)
        {
            this.shippingManager = shippingManager;
            this.shipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Determines whether shipment is ready to ship.
        /// </summary>
        /// <remarks>
        /// The base class is responsible for calling ShippingSettings.CheckForChangesNeeded() in order
        /// to leverage updated settings.
        /// </remarks>
        protected override bool IsReadyToShip(IShipmentProcessingSynchronizer synchronizer)
        {
            return shippingManager.IsShipmentTypeConfigured(shipmentTypeCode) && base.IsReadyToShip(synchronizer);
        }
    }
}
