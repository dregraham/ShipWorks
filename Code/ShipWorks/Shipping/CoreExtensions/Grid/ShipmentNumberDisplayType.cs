using System.Drawing;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.CoreExtensions.Grid
{
    /// <summary>
    /// Grid column display type for showing the "1 of 1" stuff for shipments
    /// </summary>
    public class ShipmentNumberDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentNumberDisplayType()
        {
            PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Get the value to use from the given entity for the rest of the formatting functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return ShippingManager.GetSiblingData((ShipmentEntity) entity);
        }

        /// <summary>
        /// Get the text for display
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            ShipmentSiblingData shipmentData = (ShipmentSiblingData) value;

            return string.Format("{0} of {1}", shipmentData.ShipmentNumber, shipmentData.TotalShipments);
        }

        /// <summary>
        /// Get the text foreground color to use
        /// </summary>
        protected override Color? GetDisplayForeColor(object value)
        {
            ShipmentSiblingData shipmentData = (ShipmentSiblingData) value;

            if (shipmentData.TotalShipments == 1)
            {
                return SystemColors.GrayText;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Override the default width to provide an appropriate value
        /// </summary>
        public override int DefaultWidth
        {
            get
            {
                return 40;
            }
        }
    }
}
