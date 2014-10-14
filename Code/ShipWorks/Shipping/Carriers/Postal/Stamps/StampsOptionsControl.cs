using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;

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
        /// Whether the control is used for Express1.
        /// </summary>
        public bool IsExpress1 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            requestedLabelFormat.LoadDefaultProfile(IsExpress1 ? new Express1StampsShipmentType() : new StampsShipmentType());
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
