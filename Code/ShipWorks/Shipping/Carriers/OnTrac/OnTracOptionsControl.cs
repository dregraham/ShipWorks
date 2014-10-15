using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// UserControl for editing options specific to the OnTrac integration
    /// </summary>
    public partial class OnTracOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracOptionsControl()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            requestedLabelFormat.LoadDefaultProfile(new OnTracShipmentType());
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