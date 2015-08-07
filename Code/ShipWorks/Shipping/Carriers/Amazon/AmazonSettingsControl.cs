using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Model.EntityClasses;

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

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            
            //TODO Set insurance provider based on database field for Amazon Insurance
            //insuranceProviderChooser.InsuranceProvider = (InsuranceProvider)settings.;

        }

        /// <summary>
        /// Save the shipping settings for Amazon
        /// </summary>
        /// <param name="settings"></param>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            base.SaveSettings(settings);

            //TODO save amazon insurance provider
            //settings.AmazonInsuranceProvider = (int)insuranceProviderChooser.InsuranceProvider;
        }
    }
}