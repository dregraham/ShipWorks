using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// UserControl for editing options specific to the USPS integration
    /// </summary>
    public partial class UspsOptionsControl : PostalOptionsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(UspsOptionsControl));

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The USPS reseller type.
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
            log.Info("Preparing to save requested label format to RequestedLabelFormatOptionControl.");
            requestedLabelFormat.SaveDefaultProfile();
        }
    }
}
