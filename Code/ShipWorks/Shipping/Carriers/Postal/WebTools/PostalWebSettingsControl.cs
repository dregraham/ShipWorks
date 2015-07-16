using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// User control for USPS WebTools settings
    /// </summary>
    public partial class PostalWebSettingsControl : SettingsControlBase
    {
        private CarrierServicePickerControl<PostalServiceType> servicePicker;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebSettingsControl()
        {
            InitializeComponent();

            InitializeServicePicker();
        }

        /// <summary>
        /// Initializes the service picker with Postal service types for the USPS carrier.
        /// </summary>
        private void InitializeServicePicker()
        {
            // Add carrier service picker control to the exclusions panel
            servicePicker = new CarrierServicePickerControl<PostalServiceType>();
            servicePicker.Dock = DockStyle.Fill;
            servicePicker.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            panelExclusionConfiguration.Controls.Add(servicePicker);
            panelExclusionConfiguration.Height = servicePicker.Height + 10;
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            originManagerControl.Initialize();

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            List<PostalServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (PostalServiceType) exclusion).ToList();

            List<PostalServiceType> postalServices = PostalUtility.GetDomesticServices(ShipmentTypeCode).Union(PostalUtility.GetInternationalServices(ShipmentTypeCode)).ToList();
            servicePicker.Initialize(postalServices, excludedServices);
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
        public override List<ExcludedServiceTypeEntity> GetExcludedServices()
        {
            List<int> servicesToExclude = servicePicker.ExcludedServiceTypes.Select(type => (int) type).ToList();

            List<ExcludedServiceTypeEntity> excludedServiceTypes = servicesToExclude
                .Select(serviceToExclude => new ExcludedServiceTypeEntity { ShipmentType = (int) ShipmentTypeCode, ServiceType = serviceToExclude })
                .ToList();

            return excludedServiceTypes;
        }
    }
}