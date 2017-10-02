using System;
using System.Windows.Forms;

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
        public Tuple<bool, DateTime> Value
        {
            get
            {
                return Tuple.Create(
                    cutoffEnabled.Checked,
                    cutoffTime.Value);
            }
            set
            {
                cutoffEnabled.Checked = value.Item1;
                cutoffTime.Value = value.Item2;
            }
        }

        /// <summary>
        /// Handle the cutoff enabled check changed event
        /// </summary>
        private void OnCutoffEnabledCheckedChanged(object sender, EventArgs e) =>
            cutoffTime.Enabled = cutoffEnabled.Checked;
    }
}
