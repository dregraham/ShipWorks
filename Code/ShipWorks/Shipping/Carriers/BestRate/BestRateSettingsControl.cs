using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Policies;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Settings control for the BestRate shipping type
    /// </summary>
    public partial class BestRateSettingsControl : SettingsControlBase
    {
        private static readonly List<ShipmentTypeCode> ExcludedShipmentTypes = new List<ShipmentTypeCode>
        {
            ShipmentTypeCode.None,
            ShipmentTypeCode.BestRate,
            ShipmentTypeCode.Other,
            ShipmentTypeCode.PostalWebTools,
            ShipmentTypeCode.Endicia,
            ShipmentTypeCode.Express1Endicia,
            ShipmentTypeCode.Express1Usps,
            ShipmentTypeCode.UpsOnLineTools,
            ShipmentTypeCode.UpsWorldShip,
            ShipmentTypeCode.Amazon,
            ShipmentTypeCode.iParcel
        };

        private bool isDirty;

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateSettingsControl()
        {
            InitializeComponent();

            panelProviders.ChangeEnabledShipmentTypes += OnPanelProvidersChangeEnabledShipmentTypes;
        }

        /// <summary>
        /// The enabled shipment types have changed
        /// </summary>
        void OnPanelProvidersChangeEnabledShipmentTypes(object sender, System.EventArgs e)
        {
            isDirty = true;
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
            // If there are no changes, don't do anything
            if (!isDirty)
            {
                return;
            }

            // Save the explicitly unselected shipment types
            // Include previously excluded types to handle a situation where the type was excluded from best rates then hidden globally
            // Include non carrier shipment types (other, none, etc.), since we never want to do anything with those
            // Finally, remove explicitly included shipment types to remove previously excluded types that are now shown globally
            settings.BestRateExcludedTypes = panelProviders.UnselectedShipmentTypes.Select(x => (int) x.ShipmentTypeCode)
                .Union(settings.BestRateExcludedTypes)
                .Union(ExcludedShipmentTypes.Cast<int>())
                .Except(panelProviders.SelectedShipmentTypes.Select(x => (int) x.ShipmentTypeCode))
                .ToArray();

            isDirty = false;
        }

        /// <summary>
        /// Reload the list of available shipment types when the control is activated
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            List<ShipmentTypeCode> carriersHiddenByShipmentPolicy = new List<ShipmentTypeCode>();
            ShippingPolicies.Current.Apply(ShipmentTypeCode.BestRate, carriersHiddenByShipmentPolicy);

            panelProviders.LoadProviders(ShipmentTypeManager.ShipmentTypes
                .Where(c => !carriersHiddenByShipmentPolicy.Contains(c.ShipmentTypeCode) && IsCarrierShippingType(c)),
                typeCode => !settings.BestRateExcludedTypes.Contains((int)typeCode));
        }

        /// <summary>
        /// Gets whether the specified shipment type is an actual carrier
        /// </summary>
        /// <param name="shipmentType">The shipment type to test</param>
        private static bool IsCarrierShippingType(ShipmentType shipmentType)
        {
            return !ExcludedShipmentTypes.Contains(shipmentType.ShipmentTypeCode);
        }
    }
}
