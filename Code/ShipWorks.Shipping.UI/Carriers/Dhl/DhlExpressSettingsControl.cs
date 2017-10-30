using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using System.Collections.Generic;
using System.Linq;

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
        /// DhlExpress does support services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Load the account manager with dhl express accounts
        /// </summary>
        public override void LoadSettings()
        {
            carrierAccountManagerControl.Initialize(ShipmentTypeCode);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            shippingCutoff.Value = settings.GetShipmentDateCutoff(ShipmentTypeCode);

            requestedLabelFormatOptionControl.LoadDefaultProfile(ShipmentTypeManager.GetType(ShipmentTypeCode));

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            InitializeServicePicker(shipmentType);
        }

        /// <summary>
        /// Initialize the service picker control
        /// </summary>
        private void InitializeServicePicker(ShipmentType shipmentType)
        {
            IEnumerable<DhlExpressServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Cast<DhlExpressServiceType>();

            IEnumerable<DhlExpressServiceType> allServices = EnumHelper.GetEnumList<DhlExpressServiceType>()
                .OrderBy(s => s.Description)
                .Select(s => s.Value);

            excludedServiceControl.Initialize(allServices, excludedServices);
        }

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return excludedServiceControl.ExcludedEnumValues.Cast<int>();
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
