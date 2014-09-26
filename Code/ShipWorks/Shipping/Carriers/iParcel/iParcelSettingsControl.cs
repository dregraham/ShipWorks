using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// i-parcel settings control for changing i-parcel shipping settings.
    /// </summary>
    public partial class iParcelSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelSettingsControl" /> class.
        /// </summary>
        public iParcelSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the shippings settings for i-parcel.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            optionsControl.LoadSettings();
            accountManager.Initialize();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.IParcelInsuranceProvider;
            pennyOne.Checked = settings.IParcelInsurancePennyOne;
        }

        /// <summary>
        /// Save the shipping settings for i-parcel.
        /// </summary>
        /// <param name="settings"></param>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            base.SaveSettings(settings);
            optionsControl.SaveSettings();

            settings.IParcelInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.IParcelInsurancePennyOne = pennyOne.Checked;
        }


        /// <summary>
        /// Called when [insurance provider changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnInsuranceProviderChanged(object sender, EventArgs e)
        {
            pennyOne.Enabled = (insuranceProviderChooser.InsuranceProvider == InsuranceProvider.ShipWorks);
        }


        /// <summary>
        /// Called when the penny one link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnLinkPennyOne(object sender, EventArgs e)
        {
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg("i-parcel", false))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
