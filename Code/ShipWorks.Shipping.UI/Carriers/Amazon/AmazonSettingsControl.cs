using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    public partial class AmazonSettingsControl : SettingsControlBase
    {
        public AmazonSettingsControl()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {                        
            //TODO Set insurance provider based on database field for Amazon Insurance
            //ShippingSettingsEntity settings = ShippingSettings.Fetch();
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