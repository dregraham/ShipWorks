using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public partial class AmazonSettingsControl : SettingsControlBase
    {
        public AmazonSettingsControl(IAmazonAccountManager accountManager)
        {
            InitializeComponent();
            accountManagerControl.AccountManager = accountManager;
        }


        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            accountManagerControl.Initialize();

        }
    }
}