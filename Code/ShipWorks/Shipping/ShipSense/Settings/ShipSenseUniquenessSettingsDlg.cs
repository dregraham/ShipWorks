using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    public partial class ShipSenseUniquenessSettingsDlg : Form
    {
        public ShipSenseUniquenessSettingsDlg()
        {
            InitializeComponent();

            // Just some dummy data for now
            configurationControl.LoadAttributeControls(new List<string> { "Size", "Color" });
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSave(object sender, EventArgs e)
        {
            // TODO: Present the user with a confirmation message box describing what will happen and only save the settings if Yes is chosen
        }

        /// <summary>
        /// Called when the Cancel button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCancel(object sender, EventArgs e)
        {
            Close();
        }
    }
}
