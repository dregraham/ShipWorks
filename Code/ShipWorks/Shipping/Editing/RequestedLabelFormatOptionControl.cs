using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Control that displays the requested label format
    /// </summary>
    public partial class RequestedLabelFormatOptionControl : UserControl
    {
        private ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public RequestedLabelFormatOptionControl()
        {
            InitializeComponent();

            SetSettingsMovedMessageLocation();
        }

        /// <summary>
        /// Remove the specified formats from the list of options
        /// </summary>
        public void RemoveFormats(params ThermalLanguage[] formats)
        {
            if (formats == null)
            {
                return;
            }

            foreach (ThermalLanguage format in formats)
            {
                labelFormat.Items.Remove(formats);
            }
        }

        /// <summary>
        /// Load settings into the control
        /// </summary>
        public void LoadSettings(ShipmentType workingShipmentType)
        {
            shipmentType = workingShipmentType;

            if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
            {
                SetDisplayMode(DisplayMode.ProfileNotification);   
            }
            else
            {
                EnumHelper.BindComboBox<ThermalLanguage>(labelFormat);
                labelFormat.SelectedValue = GetLabelFormatFromDefaultProfile();

                SetDisplayMode(DisplayMode.LanguageSelection);
            }
        }

        /// <summary>
        /// Save the settings to the primary profile
        /// </summary>
        public void SaveSettings()
        {
            if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
            {
                return;
            }

            ShippingProfileEntity profile = shipmentType.GetPrimaryProfile();
            profile.RequestedLabelFormat = (int)labelFormat.SelectedValue;
            ShippingProfileManager.SaveProfile(profile);
        }

        /// <summary>
        /// Handle when the user clicks on the profile link
        /// </summary>
        private void OnProfileLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShippingProfileEditorDlg profileEditor = new ShippingProfileEditorDlg(shipmentType.GetPrimaryProfile());
            profileEditor.ShowDialog(this);
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
        private ThermalLanguage GetLabelFormatFromDefaultProfile()
        {
            ShippingProfileEntity profile = shipmentType.GetPrimaryProfile();

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
