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
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// UserControl for editing options specific to the Stamps.com integration
    /// </summary>
    public partial class StampsOptionsControl : PostalOptionsControlBase
    {
        private readonly bool isExpress1;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor that allows setting of Express1
        /// </summary>
        /// <param name="useExpress1Settings"></param>
        public StampsOptionsControl(bool useExpress1Settings)
            : this()
        {
            isExpress1 = useExpress1Settings;
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            EnumHelper.BindComboBox<ThermalLabelType>(thermalType);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            if (isExpress1)
            {
                thermalPrinter.Checked = settings.Express1StampsThermal;
                thermalType.SelectedValue = (ThermalLabelType)settings.Express1StampsThermalType;
            }
            else
            {
                thermalPrinter.Checked = settings.StampsThermal;
                thermalType.SelectedValue = (ThermalLabelType)settings.StampsThermalType;
            }
        }

        /// <summary>
        /// Update the enabled state of the thermal UI based on what's selected
        /// </summary>
        private void OnUpdateThermalUI(object sender, EventArgs e)
        {
            labelThermalType.Enabled = thermalPrinter.Checked;
            thermalType.Enabled = thermalPrinter.Checked;
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            if (isExpress1)
            {
                settings.Express1StampsThermal = thermalPrinter.Checked;
                settings.Express1StampsThermalType = (int)thermalType.SelectedValue;   
            }
            else
            {
                settings.StampsThermal = thermalPrinter.Checked;
                settings.StampsThermalType = (int)thermalType.SelectedValue;    
            }
        }
    }
}
