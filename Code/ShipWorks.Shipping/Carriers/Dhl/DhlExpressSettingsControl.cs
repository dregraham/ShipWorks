using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(SettingsControlBase), ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressSettingsControl : SettingsControlBase
    {
        public DhlExpressSettingsControl() 
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            carrierAccountManagerControl.Initialize(ShipmentTypeCode.DhlExpress);
        }
    }
}
