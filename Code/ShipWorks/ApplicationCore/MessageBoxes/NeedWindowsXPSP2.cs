using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    public partial class NeedWindowsXPSP2 : Form
    {
        public NeedWindowsXPSP2()
        {
            InitializeComponent();
        }

        private void OnGetServicePack2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://www.microsoft.com/windowsxp/sp2/", this);
        }
    }
}