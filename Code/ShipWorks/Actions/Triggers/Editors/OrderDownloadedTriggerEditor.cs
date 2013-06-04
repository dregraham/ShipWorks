using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Editor for the settings of the OrderDownloaded trigger
    /// </summary>
    public partial class OrderDownloadedTriggerEditor : ActionTriggerEditor
    {
        OrderDownloadedTrigger trigger;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDownloadedTriggerEditor(OrderDownloadedTrigger trigger)
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
            downloadRestriction.Checked = trigger.Restriction != OrderDownloadedRestriction.None;
            if (downloadRestriction.Checked)
            {
                downloadRestrictionCombo.SelectedIndex = (trigger.Restriction == OrderDownloadedRestriction.OnlyInitial) ? 0 : 1;
            }
            else
            {
                downloadRestrictionCombo.SelectedIndex = 0;
                downloadRestrictionCombo.Enabled = false;
            }

            // Start listening for changes
            downloadRestriction.CheckedChanged += new EventHandler(OnValueChanged);
            downloadRestrictionCombo.SelectedIndexChanged += new EventHandler(OnValueChanged);
        }

        /// <summary>
        /// The value of one of the settings controls has changed
        /// </summary>
        void OnValueChanged(object sender, EventArgs e)
        {
            if (downloadRestriction.Checked)
            {
                trigger.Restriction = (downloadRestrictionCombo.SelectedIndex == 0) ? OrderDownloadedRestriction.OnlyInitial : OrderDownloadedRestriction.NotInitial;
            }
            else
            {
                trigger.Restriction = OrderDownloadedRestriction.None;
            }

            downloadRestrictionCombo.Enabled = downloadRestriction.Checked;
        }
    }
}
