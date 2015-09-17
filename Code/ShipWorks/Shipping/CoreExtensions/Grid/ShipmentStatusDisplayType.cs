using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.ShipSense;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.CoreExtensions.Grid
{
    /// <summary>
    /// Display type for showing the status of a shipment in the shipment grid
    /// </summary>
    public class ShipmentStatusDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Create the editor to use
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new ShipmentStatusDisplayTypeEditor(this);
        }

        /// <summary>
        /// Indicates whether the icon should be drawn for the control
        /// </summary>
        public bool ShowIcon { get; set; }

        /// <summary>
        /// Get the value to use for the given entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity) => entity;

        /// <summary>
        /// Get the image to use for the given display value
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            if (!ShowIcon)
            {
                return null;
            }

            ShipmentEntity shipment = value as ShipmentEntity;
            if (shipment == null)
            {
                return null;
            }

            if (shipment.Voided)
            {
                return Resources.cancel16;
            }

            if (shipment.Processed)
            {
                return Resources.check16;
            }

            if (!shipment.Processed && shipment.ShipSenseStatus == (int)ShipSenseStatus.Applied)
            {
                return EnumHelper.GetImage(ShipSenseStatus.Applied);
            }

            if (ShippingDlg.ErrorManager?.ShipmentHasError(shipment.ShipmentID) ?? false)
            {
                return Resources.error16;
            }

            return null;
        }

        /// <summary>
        /// Get the display text to use
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            ShipmentEntity shipment = value as ShipmentEntity;
            if (shipment == null)
            {
                return null;
            }

            if (shipment.Voided)
            {
                return "Voided";
            }

            if (shipment.Processed)
            {
                return "Processed";
            }

            if (ShippingDlg.ErrorManager?.ShipmentHasError(shipment.ShipmentID) ?? false)
            {
                return "Error";
            }

            return string.Empty;
        }
    }
}
