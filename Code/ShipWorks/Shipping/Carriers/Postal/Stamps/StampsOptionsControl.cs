using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// UserControl for editing options specific to the Stamps.com integration
    /// </summary>
    public partial class StampsOptionsControl : PostalOptionsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsOptionsControl()
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
