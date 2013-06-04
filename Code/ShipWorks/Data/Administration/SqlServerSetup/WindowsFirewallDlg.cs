using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Window for allowing the user to configure windows firewall
    /// </summary>
    public partial class WindowsFirewallDlg : Form
    {
        public WindowsFirewallDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open windows firewall for the current sql server instance
        /// </summary>
        private void OnUpdateWindowsFirewall(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SqlWindowsFirewallUtility.OpenWindowsFirewall();

                MessageHelper.ShowInformation(this, "Windows Firewall has been updated for ShipWorks.");
                UseWaitCursor = false;

                Close();
            }
            catch (WindowsFirewallException ex)
            {
                UseWaitCursor = false;
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
