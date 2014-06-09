using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Window to activate a user's Express1 discount account
    /// </summary>
    public partial class Express1ActivateDiscountDlg : Form
    {
        private readonly IExpress1SettingsFacade express1Settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1ActivateDiscountDlg(IExpress1SettingsFacade express1Settings)
        {
            this.express1Settings = express1Settings;
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            express1Control.LoadSettings(express1Settings);
        }

        /// <summary>
        /// Closing the window
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Make sure the settings are valid before trying to save them
            if (express1Settings.UseExpress1 && express1Settings.Express1Account <= 0)
            {
                MessageHelper.ShowMessage(this, "Please select or create an Express1 account.");
                e.Cancel = true;
                return;
            }

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            express1Settings.SaveSettings(settings);

            ShippingSettings.Save(settings);
            RateCache.Instance.Clear();

            DialogResult = express1Settings.UseExpress1 ? DialogResult.OK : DialogResult.Cancel;
        }
    }
}
