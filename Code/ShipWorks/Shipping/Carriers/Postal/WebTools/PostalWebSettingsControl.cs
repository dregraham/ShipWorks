﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// User control for USPS WebTools settings
    /// </summary>
    public partial class PostalWebSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Carrier supports services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Carrier supports packages
        /// </summary>
        protected override bool SupportsPackages => true;

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            Initialize(ShipmentTypeCode.PostalWebTools);

            base.LoadSettings();

            originManagerControl.Initialize();

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            PostalUtility.InitializeServicePicker(servicePicker, shipmentType);
            PostalUtility.InitializePackagePicker(packagePicker, shipmentType);
        }

        /// <summary>
        /// Refresh the content of the control
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            originManagerControl.Initialize();
        }

        /// <summary>
        /// Gets the excludeded services.
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedEnumValues.Cast<int>();
        }

        /// <summary>
        /// Gets the excludeded packages.
        /// </summary>
        public override IEnumerable<int> GetExcludedPackageTypes()
        {
            return packagePicker.ExcludedEnumValues.Cast<int>();
        }
    }
}