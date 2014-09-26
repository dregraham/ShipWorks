﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Control that displays the requested label format
    /// </summary>
    public partial class RequestedLabelFormatOptionControl : UserControl, IShippingProfileControl
    {
        private ShipmentType shipmentType;
        private ThermalLanguage[] excludedFormats;

        /// <summary>
        /// Constructor
        /// </summary>
        public RequestedLabelFormatOptionControl()
        {
            InitializeComponent();

            SetSettingsMovedMessageLocation();

            BindComboBox();
            SetDisplayMode(DisplayMode.LanguageSelection);
        }

        public bool State { get; set; }

        /// <summary>
        /// Remove the specified formats from the list of options
        /// </summary>
        public void ExcludeFormats(params ThermalLanguage[] formats)
        {
            excludedFormats = formats;

            object currentValue = labelFormat.SelectedValue;
            BindComboBox();

            if (currentValue != null && labelFormat.Items.Contains(currentValue))
            {
                labelFormat.SelectedValue = currentValue;   
            }
        }

        /// <summary>
        /// Load settings into the control
        /// </summary>
        public void LoadDefaultProfile(ShipmentType workingShipmentType)
        {
            shipmentType = workingShipmentType;

            if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
            {
                SetDisplayMode(DisplayMode.ProfileNotification);   
            }
            else
            {
                LoadFromEntity(shipmentType.GetPrimaryProfile());
            }
        }

        /// <summary>
        /// Load settings into the control
        /// </summary>
        public void LoadFromEntity(EntityBase2 entity)
        {
            ShippingProfileEntity profile = entity as ShippingProfileEntity;
            Debug.Assert(profile != null, "Entity must be a profile");

            BindComboBox();
            labelFormat.SelectedValue = GetLabelFormatFromDefaultProfile(profile);
            
            SetDisplayMode(DisplayMode.LanguageSelection);
        }

        /// <summary>
        /// Save the settings to the primary profile
        /// </summary>
        public void SaveDefaultProfile()
        {
            if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
            {
                return;
            }

            ShippingProfileEntity profile = shipmentType.GetPrimaryProfile();
            SaveToEntity(profile);
            ShippingProfileManager.SaveProfile(profile);
        }

        /// <summary>
        /// Save settings into the specified profile
        /// </summary>
        public void SaveToEntity(EntityBase2 entity)
        {
            ShippingProfileEntity profile = entity as ShippingProfileEntity;
            Debug.Assert(profile != null, "Entity must be a profile");

            profile.RequestedLabelFormat = (int)labelFormat.SelectedValue;
        }

        /// <summary>
        /// Handle when the user clicks on the profile link
        /// </summary>
        private void OnProfileLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (ShippingProfileEditorDlg profileEditor = new ShippingProfileEditorDlg(shipmentType.GetPrimaryProfile()))
            {
                profileEditor.ShowDialog(this);   
            }
        }

        /// <summary>
        /// Bind the combo box to the available label formats
        /// </summary>
        private void BindComboBox()
        {
            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat, x => excludedFormats == null || !excludedFormats.Contains(x));
        }

        /// <summary>
        /// Update the settings moved message so that it is located over the label format message
        /// </summary>
        /// <remarks>This isn't being done with the designer so that both messages can be edited without having
        /// to move things around in the designer</remarks>
        private void SetSettingsMovedMessageLocation()
        {
            Point delta = labelFormatMessage.Location - (Size)settingsMovedMessage.Location;

            settingsMovedMessage.Location = labelFormatMessage.Location;
            primaryProfileLink.Location = primaryProfileLink.Location + (Size)delta;
        }

        /// <summary>
        /// Set which controls should be displayed
        /// </summary>
        private void SetDisplayMode(DisplayMode displayMode)
        {
            bool showLanguageSelection = displayMode == DisplayMode.LanguageSelection;

            labelFormat.Visible = showLanguageSelection;
            labelFormatMessage.Visible = showLanguageSelection;
            infotipLabelType.Visible = showLanguageSelection;

            primaryProfileLink.Visible = !showLanguageSelection;
            settingsMovedMessage.Visible = !showLanguageSelection;
        }

        /// <summary>
        /// Get the label format from the default profile of the specified shipment type
        /// </summary>
        private ThermalLanguage GetLabelFormatFromDefaultProfile(ShippingProfileEntity profile)
        {
            if (profile.RequestedLabelFormat.HasValue)
            {
                return (ThermalLanguage)profile.RequestedLabelFormat.Value;
            }

            return ThermalLanguage.None;
        }

        /// <summary>
        /// Defines possible display types
        /// </summary>
        private enum DisplayMode
        {
            /// <summary>
            /// Show the UI that will allow selection of the label format
            /// </summary>
            LanguageSelection,

            /// <summary>
            /// Show the UI that will notify users that the setting has moved
            /// </summary>
            ProfileNotification
        }
    }
}
