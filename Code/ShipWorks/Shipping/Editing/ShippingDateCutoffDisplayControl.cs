using System.Windows;
using ShipWorks.Core.UI.ValueConverters;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Display the shipment cutoff date for a shipment type
    /// </summary>
    public class ShippingDateCutoffDisplayControl : InfoTip
    {
        private ShipmentTypeCode shipmentType;
        private readonly ShipmentDateCutoffConverter converter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDateCutoffDisplayControl() : base()
        {
            converter = new ShipmentDateCutoffConverter();
            base.Title = "Shipment cutoff time";
            base.Caption = "Cutoff time is 3:00 PM";
        }

        /// <summary>
        /// Shipment type to display
        /// </summary>
        public ShipmentTypeCode ShipmentType
        {
            get
            {
                return shipmentType;
            }
            set
            {
                shipmentType = value;
                UpdateUI();
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        public override string Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                // Don't set because the title property should only be set internally
            }
        }

        /// <summary>
        /// Caption
        /// </summary>
        public override string Caption
        {
            get
            {
                return base.Caption;
            }
            set
            {
                // Don't set because the text property should only be set internally
            }
        }

        /// <summary>
        /// Update the UI
        /// </summary>
        private void UpdateUI()
        {
            base.Caption = converter.Convert(shipmentType, typeof(string), null, null) as string;
            base.Visible = ((Visibility) converter.Convert(shipmentType, typeof(Visibility), null, null)) == Visibility.Visible;
        }
    }
}
