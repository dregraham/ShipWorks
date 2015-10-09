using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// User control for editing global ups options
    /// </summary>
    public partial class UpsOltOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            requestedLabelFormat.LoadDefaultProfile(ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools));
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings()
        {
            requestedLabelFormat.SaveDefaultProfile();
        }
    }
}
