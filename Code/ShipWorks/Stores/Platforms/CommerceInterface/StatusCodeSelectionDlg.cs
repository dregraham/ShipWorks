using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.CommerceInterface
{
    /// <summary>
    /// Window for selecting the online status to send on Shipment Update commands
    /// </summary>
    public partial class StatusCodeSelectionDlg : Form
    {
        /// <summary>
        /// Get the selected status code
        /// </summary>
        public int SelectedStatusCode
        {
            get
            {
                if (status.SelectedValue != null)
                {
                    return Convert.ToInt32(status.SelectedValue);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StatusCodeSelectionDlg(GenericStoreStatusCodeProvider statusCodes)
        {
            InitializeComponent();

            // populate the status combo
            status.DisplayMember = "Display";
            status.ValueMember = "Code";
            status.DataSource = statusCodes.CodeValues.Select(c => new { Display = statusCodes[c], Code = c }).ToList();

            if (status.Items.Count > 0)
            {
                // default to "ship**"
                foreach (string codeName in statusCodes.CodeNames)
                {
                    if (codeName.StartsWith("ship", StringComparison.InvariantCultureIgnoreCase))
                    {
                        status.SelectedValue = statusCodes.GetCodeValue(codeName);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// OK was clicked
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (status.SelectedIndex < 0)
            {
                MessageHelper.ShowError(this, "A status must be selected.");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
