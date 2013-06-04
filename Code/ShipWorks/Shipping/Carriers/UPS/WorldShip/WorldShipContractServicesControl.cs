using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Control for enabling/disabling the availability of UPS contract-based 
    /// service types that are available through WorldShip.
    /// </summary>
    public partial class WorldShipContractServicesControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipContractServicesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the enabled UPS WorldShip services from the database
        /// </summary>
        public void LoadSettings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            List<int> enabledServices = new List<int>(settings.WorldShipServices);
          
            // checking Mail Innovations actually includes all 3 Mail Innovations services. Key the UI off of the Domestic one
            mailInnovations.Checked = enabledServices.Contains((int)UpsContractService.MailInnovations);
        }

        /// <summary>
        /// Saves the UI values to the provided entity
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            List<int> enabled = new List<int>();

            // checking Mail Innovations actually enabled the 3 MI services
            if (mailInnovations.Checked)
            {
                enabled.Add((int)UpsContractService.MailInnovations);
            }

            settings.WorldShipServices = enabled.ToArray();
        }
    }
}
