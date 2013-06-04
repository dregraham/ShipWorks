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
    public partial class EnvironmentLoadDlg : Form
    {
        EnvironmentController controller;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnvironmentLoadDlg(EnvironmentController controller)
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
            panelSettings.Enabled = false;

            UpdateLoadButton();
        }

        /// <summary>
        /// Settings selected to be saved have changed
        /// </summary>
        private void OnChangeSelectedSettings(object sender, EventArgs e)
        {
            UpdateLoadButton();
        }

        /// <summary>
        /// Update the load button status
        /// </summary>
        private void UpdateLoadButton()
        {
            load.Enabled = panelSettings.Enabled && (settingRibbonPanels.Checked || settingContextMenus.Checked);
        }

        /// <summary>
        /// Browse for the settings file
        /// </summary>
        private void OnBrowse(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    EnvironmentOptions available = controller.InspectFile(openFileDialog.FileName);

                    settingsFile.Text = openFileDialog.FileName;

                    settingRibbonPanels.Checked = available.Windows;
                    settingContextMenus.Checked = available.Menus;

                    settingRibbonPanels.Enabled = available.Windows;
                    settingContextMenus.Enabled = available.Menus;

                    panelSettings.Enabled = true;
                    UpdateLoadButton();
                }
                catch (AppearanceException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Save the settings file
        /// </summary>
        private void OnSave(object sender, EventArgs e)
        {
            try
            {
                controller.LoadFile(openFileDialog.FileName, new EnvironmentOptions(settingRibbonPanels.Checked, settingContextMenus.Checked, false));

                DialogResult = DialogResult.OK;
            }
            catch (AppearanceException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
