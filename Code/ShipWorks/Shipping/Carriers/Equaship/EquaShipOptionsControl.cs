using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Common.IO.Hardware.Printers;

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
            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat);
        }

        /// <summary>
        /// Load settings from the database
        /// </summary>
        public void LoadSettings()
        {
            ShippingProfileEntity profile = new EquaShipShipmentType().GetPrimaryProfile();

            if (profile.RequestedLabelFormat.HasValue)
            {
                labelFormat.SelectedValue = (ThermalLanguage)profile.RequestedLabelFormat.Value;   
            }
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            ShippingProfileEntity profile = new EquaShipShipmentType().GetPrimaryProfile();
            profile.RequestedLabelFormat = (int)labelFormat.SelectedValue;

            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                adapter.SaveAndRefetch(profile);
            }
        }
    }
}
