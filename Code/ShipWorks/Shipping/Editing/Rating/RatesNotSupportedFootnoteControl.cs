using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Shows Rate not supported and allows a user 
    /// </summary>
    public partial class RatesNotSupportedFootnoteControl : RateFootnoteControl
    {
        private readonly Action<ShipmentTypeCode> shipmentTypeSelected;

        public RatesNotSupportedFootnoteControl(Action<ShipmentTypeCode> shipmentTypeSelected)
        {
            this.shipmentTypeSelected = shipmentTypeSelected;
            InitializeComponent();

            List<ShipmentType> shipmentTypes = ShipmentTypeManager.ShipmentTypes;
            selectCarrier.InitializeFromEnumList(ShipmentTypeManager.EnabledShipmentTypes
                .Where(shipmentType=>shipmentType.SupportsGetRates)
                .Select(shipmentType=> shipmentType.ShipmentTypeCode));
        }

        /// <summary>
        /// Called when [select carrier changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSelectCarrierChanged(object sender, EventArgs e)
        {
            shipmentTypeSelected.Invoke((ShipmentTypeCode)selectCarrier.SelectedValue);
        }
    }
}
