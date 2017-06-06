using System;
using log4net;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Ensure all relevant shipment data is loaded
    /// </summary>
    [Order(typeof(ILabelRetrievalShipmentManipulator), 1)]
    public class EnsureLoadedManipulator : ILabelRetrievalShipmentManipulator
    {
        private readonly ILog log;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnsureLoadedManipulator(IShipmentTypeManager shipmentTypeManager,
            IShippingManager shippingManager,
            Func<Type, ILog> createLogger)
        {
            this.shippingManager = shippingManager;
            this.shipmentTypeManager = shipmentTypeManager;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Load the shipment data
        /// </summary>
        public ShipmentEntity Manipulate(ShipmentEntity shipment)
        {
            log.InfoFormat("Shipment {0} - Ensuring loaded", shipment.ShipmentID);
            shippingManager.EnsureShipmentLoaded(shipment);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);

            return shipment;
        }
    }
}
