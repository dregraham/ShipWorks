using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Custom editor for the UserInitiatedTrigger type
    /// </summary>
    public partial class UserInitiatedTriggerEditor : ActionTriggerEditor
    {
        UserInitiatedTrigger trigger;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserInitiatedTriggerEditor(UserInitiatedTrigger trigger)
        {
            InitializeComponent();

            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            this.trigger = trigger;

            showOnRibbon.Checked = trigger.ShowOnRibbon;
            showOnOrdersMenu.Checked = trigger.ShowOnOrdersMenu;
            showOnCustomersMenu.Checked = trigger.ShowOnCustomersMenu;

            showOnRibbon.CheckedChanged += this.OnChangeShowOnOption;
            showOnOrdersMenu.CheckedChanged += this.OnChangeShowOnOption;
            showOnCustomersMenu.CheckedChanged += this.OnChangeShowOnOption;

            EnumHelper.BindComboBox<UserInitiatedSelectionRequirement>(comboRequire);
            comboRequire.SelectedValue = trigger.SelectionRequirement;

            comboRequire.SelectedIndexChanged += this.OnChangeSelectionRequirement;

            UpdateUI();
        }

        /// <summary>
        /// Update the UI layout
        /// </summary>
        private void UpdateUI()
        {
            buttonImage.Image = trigger.LoadImage();

            panelBottom.Top = buttonImage.Bottom;

            this.Height = panelBottom.Bottom;
        }

        /// <summary>
        /// Changing one of the "Show On" options
        /// </summary>
        private void OnChangeShowOnOption(object sender, EventArgs e)
        {
            trigger.ShowOnRibbon = showOnRibbon.Checked;
            trigger.ShowOnOrdersMenu = showOnOrdersMenu.Checked;
            trigger.ShowOnCustomersMenu = showOnCustomersMenu.Checked;
        }

        /// <summary>
        /// Changing the selection requirement
        /// </summary>
        private void OnChangeSelectionRequirement(object sender, EventArgs e)
        {
            trigger.SelectionRequirement = (UserInitiatedSelectionRequirement) comboRequire.SelectedValue;
        }

        /// <summary>
        /// Open a window to choose an image for the task
        /// </summary>
        private void OnChooseImage(object sender, EventArgs e)
        {
            using (UserInitiatedImageChooserDlg dlg = new UserInitiatedImageChooserDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    trigger.PendingImage = dlg.SelectedImage;

                    buttonImage.Image = trigger.LoadImage();
                }
            }
        }
    }
}
