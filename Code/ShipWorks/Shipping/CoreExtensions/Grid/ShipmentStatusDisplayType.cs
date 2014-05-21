using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Shipping.CoreExtensions.Grid
{
    /// <summary>
    /// Display type for showing the status of a shipment in the shipment grid
    /// </summary>
    public class ShipmentStatusDisplayType : GridColumnDisplayType
    {
        bool showIcon = true;

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
        public bool ShowIcon
        {
            get { return showIcon; }
            set { showIcon = value; }
        }

        /// <summary>
        /// Get the value to use for the given entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the image to use for the given display value
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            if (!showIcon)
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
                return Resources.sw_cubes_16;
            }

            if (ShippingDlg.ProcessingErrors.ContainsKey(shipment.ShipmentID))
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

            if (ShippingDlg.ProcessingErrors.ContainsKey(shipment.ShipmentID))
            {
                return "Error";
            }

            return string.Empty;
        }
    }
}
