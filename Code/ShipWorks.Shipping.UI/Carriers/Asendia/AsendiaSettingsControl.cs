using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(SettingsControlBase), ShipmentTypeCode.Asendia)]
    public partial class AsendiaSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaSettingsControl() 
        {
            InitializeComponent();
            base.Initialize(ShipmentTypeCode.Asendia);
        }

        /// <summary>
        /// Load the account manager with Asendia accounts
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
