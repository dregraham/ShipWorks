using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Process voids for the shipment history view
    /// </summary>
    [Component]
    public class ShipmentHistoryVoidProcessor : IShipmentHistoryVoidProcessor
    {
        private readonly IShippingManager shippingManager;
        private readonly IShippingErrorManager shippingErrorManager;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryVoidProcessor(
            IShippingManager shippingManager,
            IShippingErrorManager shippingErrorManager,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.shippingErrorManager = shippingErrorManager;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Void a processed shipment
        /// </summary>
        public GenericResult<ProcessedShipmentEntity> Void(ProcessedShipmentEntity shipment) =>
            shippingManager.VoidShipment(shipment.ShipmentID, shippingErrorManager)
                .Map(_ => shipment);
    }
}
