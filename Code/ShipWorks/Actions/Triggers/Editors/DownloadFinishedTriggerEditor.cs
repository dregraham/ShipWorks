using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Editor for the settings of the DownloadFinished trigger
    /// </summary>
    public partial class DownloadFinishedTriggerEditor : ActionTriggerEditor
    {
        DownloadFinishedTrigger trigger;

        /// <summary>
        /// Constructor
        /// </summary>
        public DownloadFinishedTriggerEditor(DownloadFinishedTrigger trigger)
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
            restrictResultCombo.DisplayMember = "Key";
            restrictResultCombo.ValueMember = "Value";
            restrictResultCombo.DataSource = EnumHelper.GetEnumList<DownloadResult>().Where(i => i.Value != DownloadResult.Unfinished).ToList();

            // Load DownloadResult restriction
            restrictResult.Checked = trigger.RequiredResult != null;
            if (trigger.RequiredResult != null)
            {
                restrictResultCombo.SelectedValue = trigger.RequiredResult.Value;
            }
            else
            {
                restrictResultCombo.Enabled = false;
            }

            // Load new orders restriction
            onlyNewOrders.Checked = trigger.OnlyIfNewOrders;

            // Start listening for changes
            restrictResult.CheckedChanged += new EventHandler(OnValueChanged);
            restrictResultCombo.SelectedIndexChanged += new EventHandler(OnValueChanged);
            onlyNewOrders.CheckedChanged += new EventHandler(OnValueChanged);
        }

        /// <summary>
        /// The value of one of the options has changed
        /// </summary>
        void OnValueChanged(object sender, EventArgs e)
        {
            if (restrictResult.Checked)
            {
                trigger.RequiredResult = (DownloadResult) restrictResultCombo.SelectedValue;
            }
            else
            {
                trigger.RequiredResult = null;
            }

            restrictResultCombo.Enabled = restrictResult.Checked;

            trigger.OnlyIfNewOrders = onlyNewOrders.Checked;
        }
    }
}
