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
        public Tuple<bool, TimeSpan> Value
        {
            get
            {
                return Tuple.Create(
                    cutoffEnabled.Checked,
                    cutoffTime.Value.TimeOfDay);
            }
            set
            {
                cutoffEnabled.Checked = value.Item1;
                cutoffTime.Enabled = cutoffEnabled.Checked;
                cutoffTime.Value = new DateTime(2000, 1, 1).Add(value.Item2);
            }
        }

        /// <summary>
        /// Handle the cutoff enabled check changed event
        /// </summary>
        private void OnCutoffEnabledCheckedChanged(object sender, EventArgs e) =>
            cutoffTime.Enabled = cutoffEnabled.Checked;
    }
}
