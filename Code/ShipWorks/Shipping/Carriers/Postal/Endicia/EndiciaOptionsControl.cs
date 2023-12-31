﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// UserControl for editing options specific to the endicia integration
    /// </summary>
    public partial class EndiciaOptionsControl : PostalOptionsControlBase
    {
        private bool showShippingCutoffDate = true;
        private int originalCustomsTop;

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

            originalCustomsTop = customsPanel.Top;
        }

        /// <summary>
        /// Gets or sets the reseller.
        /// </summary>
        public EndiciaReseller Reseller { get; set; }

        /// <summary>
        /// Show the shipping cutoff date
        /// </summary>
        public bool ShowShippingCutoffDate
        {
            get
            {
                return showShippingCutoffDate;
            }
            set
            {
                showShippingCutoffDate = value;
                UpdateUI();
            }
        }

        /// <summary>
        /// Update the UI
        /// </summary>
        private void UpdateUI()
        {
            shippingCutoff.Visible = ShowShippingCutoffDate;
            customsPanel.Top = ShowShippingCutoffDate ?
                originalCustomsTop :
                shippingCutoff.Top;

            Height = customsPanel.Bottom;
        }

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
            IShippingSettingsEntity settings = ShippingSettings.FetchReadOnly();

            switch (Reseller)
            {
                case EndiciaReseller.Express1:
                    {
                        requestedLabelFormat.LoadDefaultProfile(new Express1EndiciaShipmentType());

                        customsCertify.Checked = settings.Express1EndiciaCustomsCertify;
                        customsSigner.Text = settings.Express1EndiciaCustomsSigner;

                        if (ShowShippingCutoffDate)
                        {
                        	shippingCutoff.Value = settings.GetShipmentDateCutoff(ShipmentTypeCode.Express1Endicia);
						}
                        break;
                    }

                case EndiciaReseller.None:
                default:
                    {
                        requestedLabelFormat.LoadDefaultProfile(new EndiciaShipmentType());

                        customsCertify.Checked = settings.EndiciaCustomsCertify;
                        customsSigner.Text = settings.EndiciaCustomsSigner;

                        if (ShowShippingCutoffDate)
                        {
                            shippingCutoff.Value = settings.GetShipmentDateCutoff(ShipmentTypeCode.Endicia);
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            requestedLabelFormat.SaveDefaultProfile();

            switch (Reseller)
            {
                case EndiciaReseller.Express1:
                    {
                        settings.Express1EndiciaCustomsCertify = customsCertify.Checked;
                        settings.Express1EndiciaCustomsSigner = customsSigner.Text;

                        if (ShowShippingCutoffDate)
                        {
                        	settings.SetShipmentDateCutoff(ShipmentTypeCode.Express1Endicia, shippingCutoff.Value);
                        }

                        break;
                    }
                case EndiciaReseller.None:
                default:
                    {
                        settings.EndiciaCustomsCertify = customsCertify.Checked;
                        settings.EndiciaCustomsSigner = customsSigner.Text;

                        if (ShowShippingCutoffDate)
                        {
                            settings.SetShipmentDateCutoff(ShipmentTypeCode.Endicia, shippingCutoff.Value);
                        }

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
