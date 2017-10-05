using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(SettingsControlBase), ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressSettingsControl() 
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the account manager with dhl express accounts
        /// </summary>
        public override void LoadSettings()
        {
            carrierAccountManagerControl.Initialize(ShipmentTypeCode.DhlExpress);
        }
    }
}
