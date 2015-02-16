using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// UserControl for editing options specific to the Stamps.com integration
    /// </summary>
    public partial class UspsOptionsControl : PostalOptionsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The stamps reseller type.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            requestedLabelFormat.LoadDefaultProfile(shipmentType);
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            requestedLabelFormat.SaveDefaultProfile();
        }
    }
}
