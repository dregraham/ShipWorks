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
    /// Window to allow user to save ShipWorks environment settings
    /// </summary>
    public partial class EnvironmentSaveDlg : Form
    {
        EnvironmentController controller;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnvironmentSaveDlg(EnvironmentController controller)
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

        }

        /// <summary>
        /// Settings selected to be saved have changed
        /// </summary>
        private void OnChangeSelectedSettings(object sender, EventArgs e)
        {
            save.Enabled = settingRibbonPanels.Checked || settingContextMenus.Checked;
        }

        /// <summary>
        /// Save the settings file
        /// </summary>
        private void OnSave(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    controller.SaveFile(saveFileDialog.FileName, new EnvironmentOptions(settingRibbonPanels.Checked, settingContextMenus.Checked, false));

                    DialogResult = DialogResult.OK;
                }
                catch (AppearanceException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }
        }

    }
}
