using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
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
            base.Initialize(ShipmentTypeCode.DhlExpress);
        }

        /// <summary>
        /// Load the account manager with dhl express accounts
        /// </summary>
        public override void LoadSettings()
        {
            carrierAccountManagerControl.Initialize(ShipmentTypeCode);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            shippingCutoff.Value = settings.GetShipmentDateCutoff(ShipmentTypeCode);

            requestedLabelFormatOptionControl.LoadDefaultProfile(ShipmentTypeManager.GetType(ShipmentTypeCode));
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        protected override void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.SetShipmentDateCutoff(ShipmentTypeCode, shippingCutoff.Value);

            requestedLabelFormatOptionControl.SaveDefaultProfile();
        }
    }
}
