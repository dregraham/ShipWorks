using System;
using System.Windows.Forms;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Options control for WorldShip
    /// </summary>
    public partial class WorldShipOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            launchWorldShip.Checked = settings.WorldShipLaunch;
        }

        /// <summary>
        /// Save the settings to the settings entity
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.WorldShipLaunch = launchWorldShip.Checked;
        }

        /// <summary>
        /// Handles the click for integrating with WorldShip
        /// </summary>
        private void OnIntegrateWorldShip(object sender, EventArgs e)
        {
            bool completed;
            try
            {
                completed = WorldShipIntegrator.IntegrateWithWorldShip(this);
            }
            catch (WorldShipIntegratorException ex)
            {
                string errorMessage = string.Format("ShipWorks was unable to create WorldShip mappings.{0}{0}{1}", Environment.NewLine, ex.Message);
                MessageHelper.ShowError(this, errorMessage);

                return;
            }

            if (completed)
            {
                MessageHelper.ShowMessage(this, "ShipWorks successfully created the WorldShip mappings.");
            }
            else
            {
                MessageHelper.ShowMessage(this, "ShipWorks did not create any WorldShip mappings.");
            }
        }
    }
}
