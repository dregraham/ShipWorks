using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Interapptive.Shared.Win32;
using System.Collections;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Custom ComboBox for making working with loading and displaying of the policy values easier
    /// </summary>
    public class ComputerDownloadAllowedComboBox : ComboBox
    {
        bool loading = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ComputerDownloadAllowedComboBox()
        {

        }

        /// <summary>
        /// Load the allowed choices, can be used to refresh when the default policy changes
        /// </summary>
        public void LoadChoices(bool defaultToYes)
        {
            loading = true;
            int selectedIndex = this.SelectedIndex;

            this.DisplayMember = "Display";
            this.ValueMember = "Value";
            this.DataSource = new ArrayList
                {
                    new { Display = string.Format("Default ({0})", defaultToYes ? "Yes" : "No"), Value = ComputerDownloadAllowed.Default },
                    new { Display = "Yes", Value = ComputerDownloadAllowed.Yes },
                    new { Display = "No", Value = ComputerDownloadAllowed.No }
                };

            this.SelectedIndex = selectedIndex;
            loading = false;
        }

        /// <summary>
        /// Override to prevent event if we are loading
        /// </summary>
        protected override void OnSelectedValueChanged(EventArgs e)
        {
            if (loading)
            {
                return;
            }

            base.OnSelectedValueChanged(e);
        }

        /// <summary>
        /// Override to prevent event if we are loading
        /// </summary>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (loading)
            {
                return;
            }

            base.OnSelectedIndexChanged(e);
        }
    }
}
