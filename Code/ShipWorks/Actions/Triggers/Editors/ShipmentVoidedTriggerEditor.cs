using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Editor for the Shipment Processed trigger
    /// </summary>
    public partial class ShipmentVoidedTriggerEditor : ActionTriggerEditor
    {
        ShipmentVoidedTrigger trigger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentVoidedTriggerEditor(ShipmentVoidedTrigger trigger)
        {
            InitializeComponent();

            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            this.trigger = trigger;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            restrictType.Checked = trigger.RestrictType;
            shipmentType.Enabled = trigger.RestrictType;

            restrictReturns.Checked = trigger.RestrictStandardReturn;
            standardReturnType.SelectedIndex = trigger.ReturnShipmentsOnly ? 1 : 0;

            EnumHelper.BindComboBox<ShipmentTypeCode>(shipmentType);
            shipmentType.SelectedValue = trigger.ShipmentType;

            restrictType.CheckedChanged += new EventHandler(OnValueChanged);
            shipmentType.SelectedIndexChanged += new EventHandler(OnValueChanged);

            restrictReturns.CheckedChanged += new EventHandler(OnValueChanged);
            standardReturnType.SelectedIndexChanged += new EventHandler(OnValueChanged);
        }

        /// <summary>
        /// The value of one of the settings has changed
        /// </summary>
        void OnValueChanged(object sender, EventArgs e)
        {
            trigger.RestrictType = restrictType.Checked;
            trigger.ShipmentType = (ShipmentTypeCode) shipmentType.SelectedValue;

            trigger.RestrictStandardReturn = restrictReturns.Checked;
            trigger.ReturnShipmentsOnly = (standardReturnType.SelectedIndex == 1);

            shipmentType.Enabled = trigger.RestrictType;
            standardReturnType.Enabled = trigger.RestrictStandardReturn;
        }
    }
}
