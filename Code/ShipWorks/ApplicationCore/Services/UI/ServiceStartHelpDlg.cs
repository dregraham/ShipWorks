﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Services.UI
{
    public partial class ServiceStartHelpDlg : Form
    {
        public ServiceStartHelpDlg()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            Close();
        }
    }
}
