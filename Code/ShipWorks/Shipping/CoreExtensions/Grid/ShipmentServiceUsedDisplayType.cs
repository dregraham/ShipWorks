using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using SD.LLBLGen.Pro.ORMSupportClasses;

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
            ShipmentEntity shipment = value as ShipmentEntity;
            if (shipment == null)
            {
                return "";
            }

            return ShippingManager.GetActualServiceUsed(shipment);
        }
    }
}
