using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// AmazonSettingsControl
    /// </summary>
    public partial class AmazonSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSettingsControl()
        {
            InitializeComponent();
            base.Initialize(ShipmentTypeCode.Amazon);
        }

        /// <summary>
        /// Carrier supports services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Load the amazon settings
        /// </summary>
        public override void LoadSettings()
        {
            AmazonShipmentType shipmentType = (AmazonShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode);

            InitializeServicePicker(shipmentType);
        }

        /// <summary>
        /// Initialize the service picker
        /// </summary>
        private void InitializeServicePicker(AmazonShipmentType shipmentType)
        {
            List<AmazonServiceType> excludedServices =
                shipmentType.GetExcludedServiceTypes().Select(exclusion => (AmazonServiceType) exclusion).ToList();

            List<AmazonServiceType> upsServices = Enum.GetValues(typeof(AmazonServiceType)).Cast<AmazonServiceType>()
                .ToList();

            servicePicker.Initialize(upsServices, excludedServices);
        }

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedEnumValues.Select(type => (int) type);
        }
    }
}