using System;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;

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
            var settings = ShippingSettings.Fetch();
            express1Control.SaveSettings(settings);

            if (settings.EndiciaAutomaticExpress1 && express1Settings.Express1Account <= 0)
            {
                MessageHelper.ShowMessage(this, "Please select or create an Express1 account.");
                e.Cancel = true;
                return;
            }

            ShippingSettings.Save(settings);

            DialogResult = settings.EndiciaAutomaticExpress1 ? DialogResult.OK : DialogResult.Cancel;
        }
    }
}
