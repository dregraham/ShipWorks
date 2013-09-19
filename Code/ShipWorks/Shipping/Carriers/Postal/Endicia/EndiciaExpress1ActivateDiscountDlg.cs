using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Window to activate a user's Express1 discount account
    /// </summary>
    public partial class EndiciaExpress1ActivateDiscountDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaExpress1ActivateDiscountDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            express1Control.LoadSettings(new Express1EndiciaSettingsFacade());
        }

        /// <summary>
        /// Closing the window
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = ShippingSettings.Fetch();
            express1Control.SaveSettings(settings);

            if (settings.EndiciaAutomaticExpress1 && settings.EndiciaAutomaticExpress1Account <= 0)
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
