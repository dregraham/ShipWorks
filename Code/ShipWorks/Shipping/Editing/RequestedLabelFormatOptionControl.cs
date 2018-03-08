﻿using System.Drawing;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
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
        static readonly ILog log = LogManager.GetLogger(typeof(RequestedLabelFormatOptionControl));

        private ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public RequestedLabelFormatOptionControl()
        {
            InitializeComponent();

            SetSettingsMovedMessageLocation();

            SetDisplayMode(DisplayMode.LanguageSelection);
        }

        /// <summary>
        /// Remove the specified formats from the list of options
        /// </summary>
        public void ExcludeFormats(params ThermalLanguage[] formats)
        {
            requestedLabelFormat.ExcludeFormats(formats);
        }

        /// <summary>
        /// Load settings into the control
        /// </summary>
        public void LoadDefaultProfile(ShipmentType workingShipmentType)
        {
            shipmentType = workingShipmentType;
            ShippingProfileEntity shippingProfile = null;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileManager shippingProfileManager = lifetimeScope.Resolve<IShippingProfileManager>();

                shippingProfile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);
            }

            if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
            {
                SetDisplayMode(DisplayMode.ProfileNotification);
                primaryProfileLink.Text = shippingProfile.Name;
            }
            else
            {
                requestedLabelFormat.LoadFromEntity(shippingProfile);
            }
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

            log.InfoFormat("Saving requested label format for {0}", EnumHelper.GetDescription(shipmentType.ShipmentTypeCode));

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileManager shippingProfileManager = lifetimeScope.Resolve<IShippingProfileManager>();

                ShippingProfileEntity profile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);
                requestedLabelFormat.SaveToEntity(profile);
                ShippingProfileManager.SaveProfile(profile);
            }
        }

        /// <summary>
        /// Handle when the user clicks on the profile link
        /// </summary>
        private void OnProfileLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileManager shippingProfileManager = lifetimeScope.Resolve<IShippingProfileManager>();
                ShippingProfileEntity profile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);

                IShippingProfileService shippingProfileService = lifetimeScope.Resolve<IShippingProfileService>();
                IShippingProfile shippingProfile = shippingProfileService.Get(profile.ShippingProfileID);

                ShippingProfileEditorDlg profileEditor = lifetimeScope.Resolve<ShippingProfileEditorDlg>(
                    new TypedParameter(typeof(IShippingProfile), shippingProfile)
                );
                profileEditor.ShowDialog(this);
            }
        }

        /// <summary>
        /// Update the settings moved message so that it is located over the label format message
        /// </summary>
        /// <remarks>This isn't being done with the designer so that both messages can be edited without having
        /// to move things around in the designer</remarks>
        private void SetSettingsMovedMessageLocation()
        {
            Point delta = requestedLabelFormat.Location - (Size) settingsMovedMessage.Location;
            delta.Offset(0, 3);

            settingsMovedMessage.Location = settingsMovedMessage.Location + (Size) delta;
            primaryProfileLink.Location = primaryProfileLink.Location + (Size) delta;
        }

        /// <summary>
        /// Set which controls should be displayed
        /// </summary>
        private void SetDisplayMode(DisplayMode displayMode)
        {
            bool showLanguageSelection = displayMode == DisplayMode.LanguageSelection;

            requestedLabelFormat.Visible = showLanguageSelection;

            primaryProfileLink.Visible = !showLanguageSelection;
            settingsMovedMessage.Visible = !showLanguageSelection;
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
