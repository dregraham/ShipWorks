using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Settings control for WorldShip shipments
    /// </summary>
    public partial class WorldShipSettingsControl : SettingsControlBase
    {
        private EnumCheckBoxControl<UpsServiceType> servicePicker;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the ShipmentTypeCode from derived class
        /// </summary>
        public override void Initialize(ShipmentTypeCode shipmentTypeCode)
        {
            base.Initialize(shipmentTypeCode);
            InitializeServicePicker();
        }

        /// <summary>
        /// Initializes the service picker with Ups service types for the USPS carrier.
        /// </summary>
        private void InitializeServicePicker()
        {
            // Add carrier service picker control to the exclusions panel
            servicePicker = new EnumCheckBoxControl<UpsServiceType>();
            servicePicker.Dock = DockStyle.Fill;
            servicePicker.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            panelExclusionConfiguration.Controls.Add(servicePicker);
            panelExclusionConfiguration.Height = servicePicker.Height + 10;
        }

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();
            accountControl.Initialize(ShipmentTypeCode.UpsWorldShip);

            UpsShipmentType shipmentType = (UpsShipmentType)ShipmentTypeManager.GetType(ShipmentTypeCode.UpsWorldShip);

            upsMailInnovationsOptions.LoadSettings(shipmentType);

            // Check if Mi is enabled
            bool isMIAvailable = shipmentType.IsMailInnovationsEnabled();

            // Check if SurePost is enabled
            bool isSurePostAvailable = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.UpsSurePost).Level == EditionRestrictionLevel.None;

            List<UpsServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (UpsServiceType)exclusion).ToList();

            List<UpsServiceType> upsServices = Enum.GetValues(typeof(UpsServiceType)).Cast<UpsServiceType>()
                .Where(t => ShowService(t, isMIAvailable, isSurePostAvailable)).ToList();

            servicePicker.Initialize(upsServices, excludedServices);
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            upsMailInnovationsOptions.SaveSettings(settings);

            optionsControl.SaveSettings(settings);
        }

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            List<int> servicesToExclude = servicePicker.ExcludedServiceTypes.Select(type => (int)type).ToList();

            return servicesToExclude;
        }

        /// <summary>
        /// Returns true if we should show the service. Else false.
        /// </summary>
        private static bool ShowService(UpsServiceType upsServiceType, bool isMiAvailable, bool isSurePostAvailable)
        {
            if (UpsUtility.IsUpsSurePostService(upsServiceType))
            {
                return isSurePostAvailable;
            }

            if (UpsUtility.IsUpsMiService(upsServiceType))
            {
                return isMiAvailable;
            }

            return true;
        }
    }
}
