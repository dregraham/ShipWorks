using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Reset return status if shipment type does not support returns
    /// </summary>
    [Order(Order.Unordered, typeof(ILabelRetrievalShipmentManipulator))]
    public class SupportsReturnsManipulator : ILabelRetrievalShipmentManipulator
    {
        readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public SupportsReturnsManipulator(IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Manipulate the shipment
        /// </summary>
        public ShipmentEntity Manipulate(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            if (!shipmentType.SupportsReturns)
            {
                shipment.ReturnShipment = false;
            }

            return shipment;
        }
    }
}
