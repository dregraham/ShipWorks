using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SWA;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(SettingsControlBase), ShipmentTypeCode.AmazonSWA)]
    public partial class AmazonSWASettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWASettingsControl()
        {
            InitializeComponent();
            base.Initialize(ShipmentTypeCode.AmazonSWA);

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            InitializeServicePicker(shipmentType);
        }

        /// <summary>
        /// AmazonSWA does support services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Initialize the service picker control
        /// </summary>
        private void InitializeServicePicker(ShipmentType shipmentType)
        {
            IEnumerable<AmazonSWAServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Cast<AmazonSWAServiceType>();

            IEnumerable<AmazonSWAServiceType> allServices = EnumHelper.GetEnumList<AmazonSWAServiceType>()
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
        /// Load the account manager with AmazonSWA accounts
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
