﻿using Interapptive.Shared.UI;
using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Settings to use Express1 as a single-source for all postal shipping services
    /// </summary>
    public partial class Express1SingleSourceControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1SingleSourceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Display information about single-source Express1
        /// </summary>
        private void OnSingleSourceLearnMore(object sender, EventArgs e)
        {
            MessageHelper.ShowInformation(this,
                "Express1 allows for the processing of all mail classes for accounts that meet certain minimum requirements.\n\n" +
                "Please contact Express1 at 1-800-399-3971 for more information.");
        }
    }
}
