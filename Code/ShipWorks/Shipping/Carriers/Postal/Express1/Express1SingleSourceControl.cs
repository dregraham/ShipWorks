﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

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
        /// Load the settings
        /// </summary>
        public void LoadSettings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            checkBoxExpress1SingleSource.Checked = settings.Express1SingleSource;
        }

        /// <summary>
        /// Save the UI settings to the entity
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.Express1SingleSource = checkBoxExpress1SingleSource.Checked;
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
