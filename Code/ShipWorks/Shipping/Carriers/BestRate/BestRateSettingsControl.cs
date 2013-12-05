using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Settings control for the BestRate shipping type
    /// </summary>
    public partial class BestRateSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads initial settings for this control
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            RefreshContent();
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            var shipments = ShipmentTypeManager.EnabledShipmentTypes
                                               .Select(x => (int)x.ShipmentTypeCode)
                                               .ToList();
            var disabledShipments = ShipmentTypeManager.ShipmentTypes
                                                       .Select(x => (int) x.ShipmentTypeCode)
                                                       .Except(shipments)
                                                       .Intersect(settings.BestRateExcludedTypes);

            var excludedShipmentCodes = shipments.Except(panelProviders.SelectedShipmentTypes.Select(x => (int)x.ShipmentTypeCode))
                                                 .Union(disabledShipments)
                                                 .ToArray();

            settings.BestRateExcludedTypes = excludedShipmentCodes;
        }

        /// <summary>
        /// Reload the list of available shipment types when the control is activated
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            panelProviders.LoadProviders(ShipmentTypeManager.EnabledShipmentTypes.Where(IsCarrierShippingType), 
                typeCode => !settings.BestRateExcludedTypes.Contains((int)typeCode));
        }

        /// <summary>
        /// Gets whether the specified shipment type is an actual carrier
        /// </summary>
        /// <param name="shipmentType">The shipment type to test</param>
        private static bool IsCarrierShippingType(ShipmentType shipmentType)
        {
            return shipmentType.ShipmentTypeCode != ShipmentTypeCode.None && 
                        shipmentType.ShipmentTypeCode != ShipmentTypeCode.BestRate && 
                        shipmentType.ShipmentTypeCode != ShipmentTypeCode.Other;
        }
    }
}
