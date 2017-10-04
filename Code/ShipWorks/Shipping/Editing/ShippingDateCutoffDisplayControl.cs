using System.Windows.Forms;
using ShipWorks.Core.UI.ValueConverters;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Display the shipment cutoff date for a shipment type
    /// </summary>
    public class ShippingDateCutoffDisplayControl : Label
    {
        private ShipmentTypeCode shipmentType;
        private readonly ShipmentDateCutoffConverter converter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDateCutoffDisplayControl() : base()
        {
            converter = new ShipmentDateCutoffConverter();
            base.Text = "Cutoff time is 3:00 PM";
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
                UpdateText();
            }
        }

        /// <summary>
        /// Text
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                // Don't set because the text property should only be set internally
            }
        }

        /// <summary>
        /// Update the text 
        /// </summary>
        private void UpdateText() =>
            base.Text = converter.Convert(shipmentType, typeof(string), null, null) as string;
    }
}
