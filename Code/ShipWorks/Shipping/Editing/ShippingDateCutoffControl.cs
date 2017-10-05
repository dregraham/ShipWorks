using System;
using System.Windows.Forms;
using ShipWorks.Settings;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Manage the shipping date cutoff time settings
    /// </summary>
    public partial class ShippingDateCutoffControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDateCutoffControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets/Sets the cutoff time settings
        /// </summary>
        public ShipmentDateCutoff Value
        {
            get
            {
                return new ShipmentDateCutoff(
                    cutoffEnabled.Checked,
                    cutoffTime.Value.TimeOfDay);
            }
            set
            {
                cutoffEnabled.Checked = value.Enabled;
                cutoffTime.Enabled = cutoffEnabled.Checked;
                cutoffTime.Value = new DateTime(2000, 1, 1).Add(value.CutoffTime);
            }
        }

        /// <summary>
        /// Handle the cutoff enabled check changed event
        /// </summary>
        private void OnCutoffEnabledCheckedChanged(object sender, EventArgs e) =>
            cutoffTime.Enabled = cutoffEnabled.Checked;
    }
}
