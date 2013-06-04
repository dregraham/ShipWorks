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
    public partial class NeedMdac28 : Form
    {
        public NeedMdac28()
        {
            InitializeComponent();
        }

        private void OnGetMdac28(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://go.microsoft.com/fwlink/?LinkId=50233", this);
        }
    }
}