using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.CoreExtensions.Grid
{
    /// <summary>
    /// Display type for showing the service used for a shipment
    /// </summary>
    public class ShipmentServiceUsedDisplayType : GridTextDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentServiceUsedDisplayType()
        {
            PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Get the value to use for the given entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the display text to use for the given value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (value is ShipmentEntity shipment)
            {
                return ShippingManager.GetActualServiceUsed(shipment);
            }

            if (value is ProcessedShipmentEntity processedShipment)
            {
                return ShippingManager.GetActualServiceUsed(processedShipment.ShipmentType, processedShipment.Service);
            }

            return string.Empty;
        }
    }
}
