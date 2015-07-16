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
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// i-parcel settings control for changing i-parcel shipping settings.
    /// </summary>
    public partial class iParcelSettingsControl : SettingsControlBase
    {

        private CarrierServicePickerControl<iParcelServiceType> servicePicker;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelSettingsControl" /> class.
        /// </summary>
        public iParcelSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the ShipmentTypeCode from derived class
        /// </summary>
        public override void Initialize(ShipmentTypeCode shipmentTypeCode)
        {
            base.Initialize(shipmentTypeCode); 
            InitializeServicePicker();
        }

        /// <summary>
        /// Initializes the service picker with Postal service types for the USPS carrier.
        /// </summary>
        private void InitializeServicePicker()
        {
            // Add carrier service picker control to the exclusions panel
            servicePicker = new CarrierServicePickerControl<iParcelServiceType> { Dock = DockStyle.Fill, Anchor = AnchorStyles.Top | AnchorStyles.Left };

            // Load up the service picker based on the excluded service types
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            List<iParcelServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (iParcelServiceType)exclusion).ToList();

            List<iParcelServiceType> allServices = EnumHelper.GetEnumList<iParcelServiceType>().Select(e => e.Value).ToList();
            servicePicker.Initialize(allServices, excludedServices);

            panelExclusionConfiguration.Controls.Add(servicePicker);
            panelExclusionConfiguration.Height = servicePicker.Height + 10;
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

            insuranceProtectionPanel.Top = panelExclusionConfiguration.Bottom;
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
        /// Gets the excluded services based on the items in the service picker.
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedServiceTypes.Cast<int>();
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
