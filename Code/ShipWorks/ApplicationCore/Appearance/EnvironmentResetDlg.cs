using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Filters;
using Interapptive.Shared.UI;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Window to allow user to reset ShipWorks environment settings back to default
    /// </summary>
    public partial class EnvironmentResetDlg : Form
    {
        EnvironmentController controller;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnvironmentResetDlg(EnvironmentController controller)
        {
            InitializeComponent();

            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            this.controller = controller;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            OnChangeSelectedSettings(null, EventArgs.Empty);
        }

        /// <summary>
        /// Settings selected to be reset have changed
        /// </summary>
        private void OnChangeSelectedSettings(object sender, EventArgs e)
        {
            reset.Enabled = settingRibbonPanels.Checked || settingGridColumns.Checked || settingContextMenus.Checked;
        }

        /// <summary>
        /// Reset the selected settings
        /// </summary>
        private void OnReset(object sender, EventArgs e)
        {
            try
            {
                controller.Reset(new EnvironmentOptions(settingRibbonPanels.Checked, settingContextMenus.Checked, settingGridColumns.Checked));

                DialogResult = DialogResult.OK;
            }
            catch (AppearanceException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
