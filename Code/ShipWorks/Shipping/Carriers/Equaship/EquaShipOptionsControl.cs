using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Control for configuring EquaShip shipping options
    /// </summary>
    public partial class EquaShipOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load settings from the database
        /// </summary>
        public void LoadSettings()
        {
            requestedLabelFormat.LoadSettings(new EquaShipShipmentType());
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings()
        {
            requestedLabelFormat.SaveSettings();
        }
    }
}
