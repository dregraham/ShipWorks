﻿using System.Collections.Generic;
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
        private static readonly List<ShipmentTypeCode> ExcludedShipmentTypes = new List<ShipmentTypeCode>
        {
            ShipmentTypeCode.None,
            ShipmentTypeCode.BestRate,
            ShipmentTypeCode.Other,
            ShipmentTypeCode.PostalWebTools,
            ShipmentTypeCode.Express1Endicia,
            ShipmentTypeCode.Express1Usps,
            ShipmentTypeCode.UpsWorldShip,
            ShipmentTypeCode.AmazonSFP,
            ShipmentTypeCode.iParcel,
            ShipmentTypeCode.AmazonSWA
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
        protected override void SaveSettings(ShippingSettingsEntity settings)
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
            settings.BestRateExcludedTypes = panelProviders.UnselectedShipmentTypes.Select(x => x.ShipmentTypeCode)
                .Union(settings.BestRateExcludedTypes)
                .Union(ExcludedShipmentTypes)
                .Except(panelProviders.SelectedShipmentTypes.Select(x => x.ShipmentTypeCode))
                .ToArray();

            isDirty = false;
        }

        /// <summary>
        /// Reload the list of available shipment types when the control is activated
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            IEnumerable<ShipmentType> availableBestRateShipmentTypes = ShipmentTypeManager.ShipmentTypes
                .Where(c => !ExcludedShipmentTypes.Contains(c.ShipmentTypeCode));

            panelProviders.LoadProviders(availableBestRateShipmentTypes, IsBestRateExcludedType);
        }

        /// <summary>
        /// Determines whether ShipmentTypeCode has been excluded in shipping settings.
        /// </summary>
        private static bool IsBestRateExcludedType(ShipmentTypeCode typeCode)
        {
            return !ShippingSettings.Fetch().BestRateExcludedTypes.Contains(typeCode);
        }
    }
}
