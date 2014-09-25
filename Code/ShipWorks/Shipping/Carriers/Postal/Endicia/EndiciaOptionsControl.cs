using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// UserControl for editing options specific to the endicia integration
    /// </summary>
    public partial class EndiciaOptionsControl : PostalOptionsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaOptionsControl"/> class resulting
        /// in the EndiciaReseller value being None.
        /// </summary>
        public EndiciaOptionsControl()
            : this(EndiciaReseller.None)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaOptionsControl"/> class.
        /// </summary>
        /// <param name="reseller">The reseller.</param>
        public EndiciaOptionsControl(EndiciaReseller reseller)
        {
            InitializeComponent();
            Reseller = reseller;

            EnumHelper.BindComboBox<ThermalDocTabType>(thermalDocTabType);
        }

        /// <summary>
        /// Gets or sets the reseller.
        /// </summary>
        public EndiciaReseller Reseller { get; set; }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <param name="reseller">The reseller.</param>
        public void LoadSettings(EndiciaReseller reseller)
        {
            // This is just a stop gap until everything has been refactored out
            Reseller = reseller;
            LoadSettings();
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            switch (Reseller)
            {
                case EndiciaReseller.Express1:
                    {
                        requestedLabelFormat.LoadSettings(new Express1EndiciaShipmentType());

                        customsCertify.Checked = settings.Express1EndiciaCustomsCertify;
                        customsSigner.Text = settings.Express1EndiciaCustomsSigner;

                        thermalDocTab.Checked = settings.Express1EndiciaThermalDocTab;
                        thermalDocTabType.SelectedValue = (ThermalDocTabType)settings.Express1EndiciaThermalDocTabType;

                        break;
                    }

                case EndiciaReseller.None:
                default:
                    {
                        requestedLabelFormat.LoadSettings(new EndiciaShipmentType());

                        customsCertify.Checked = settings.EndiciaCustomsCertify;
                        customsSigner.Text = settings.EndiciaCustomsSigner;

                        thermalDocTab.Checked = settings.EndiciaThermalDocTab;
                        thermalDocTabType.SelectedValue = (ThermalDocTabType)settings.EndiciaThermalDocTabType;

                        break;
                    }
            }
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            requestedLabelFormat.SaveSettings();

            switch (Reseller)
            {
                case EndiciaReseller.Express1:
                    {
                        settings.Express1EndiciaCustomsCertify = customsCertify.Checked;
                        settings.Express1EndiciaCustomsSigner = customsSigner.Text;

                        settings.Express1EndiciaThermalDocTab = thermalDocTab.Checked;
                        settings.Express1EndiciaThermalDocTabType = (int)thermalDocTabType.SelectedValue;

                        break;
                    }
                case EndiciaReseller.None:
                default:
                    {
                        settings.EndiciaCustomsCertify = customsCertify.Checked;
                        settings.EndiciaCustomsSigner = customsSigner.Text;

                        settings.EndiciaThermalDocTab = thermalDocTab.Checked;
                        settings.EndiciaThermalDocTabType = (int)thermalDocTabType.SelectedValue;

                        break;
                    }
            }
        }

        /// <summary>
        /// Change the option for customs certify
        /// </summary>
        private void OnChangeCustomsCertify(object sender, EventArgs e)
        {
            customsSigner.Enabled = customsCertify.Checked;
        }
    }
}
