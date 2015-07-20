using System.Collections.Generic;
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
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            Initialize(ShipmentTypeCode.PostalWebTools);

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
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedEnumValues.Select(type => (int) type).ToList();
        }
    }
}