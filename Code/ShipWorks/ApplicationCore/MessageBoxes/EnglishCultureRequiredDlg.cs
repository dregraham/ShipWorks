﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Window for displaying a message to the user that US-English culture is required
    /// </summary>
    public partial class EnglishCultureRequiredDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EnglishCultureRequiredDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clicked the support link
        /// </summary>
        private void OnClickSupportForum(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/support", this);
        }
    }
}
