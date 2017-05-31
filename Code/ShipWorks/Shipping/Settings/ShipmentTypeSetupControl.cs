using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Utility;
using ShipWorks.Editions;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// UserControl for letting the user go through setup of a given shipment type
    /// </summary>
    public partial class ShipmentTypeSetupControl : UserControl
    {
        ShipmentType shipmentType;

        /// <summary>
        /// Raised when setup has completed
        /// </summary>
        public event EventHandler SetupComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeSetupControl(ShipmentType shipmentType, OpenedFromSource openedFrom)
        {
            InitializeComponent();

            OpenedFrom = openedFrom;
            this.shipmentType = shipmentType;

            labelSetup.Text = string.Format(labelSetup.Text, shipmentType.ShipmentTypeName);
            setup.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);

            labelUpgrade.Text = string.Format(labelUpgrade.Text, shipmentType.ShipmentTypeName);
            panelUpgrade.Location = panelSetup.Location;

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = scope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.ShipmentType, shipmentType.ShipmentTypeCode);

                if (restrictionLevel != EditionRestrictionLevel.None)
                {
                    panelSetup.Visible = false;
                }
                else
                {
                    panelUpgrade.Visible = false;
                }
            }
        }

        /// <summary>
        /// From where was this dialog opened
        /// </summary>
        OpenedFromSource OpenedFrom { get; }

        /// <summary>
        /// Run setup for the configured shipment type
        /// </summary>
        private void OnSetup(object sender, EventArgs e)
        {
            // Ensure when we check if its setup that its up-to-date as it can be
            ShippingSettings.CheckForChangesNeeded();

            if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
            {
                MessageHelper.ShowInformation(this, "Setup has already been completed on another computer.");
                RaiseSetupComplete();
            }
            else
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IShipmentTypeSetupWizard wizard = lifetimeScope.Resolve<IShipmentTypeSetupWizardFactory>()
                        .Create(shipmentType.ShipmentTypeCode, OpenedFrom);

                    if (SetupShipmentType(this, shipmentType.ShipmentTypeCode, wizard))
                    {
                        RaiseSetupComplete();
                    }
                }
            }
        }

        /// <summary>
        /// Setup the given shipment type, returns true if it's setup.
        /// </summary>
        public static bool SetupShipmentType(IWin32Window messageOwner, ShipmentTypeCode shipmentTypeCode, IShipmentTypeSetupWizard setupDlg) =>
            setupDlg.ShowDialog(messageOwner) == DialogResult.OK;

        /// <summary>
        /// Upgrade the current edition of ShipWorks
        /// </summary>
        private void OnUpgrade(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = scope.Resolve<ILicenseService>();

                if (licenseService.HandleRestriction(EditionFeature.ShipmentType, shipmentType.ShipmentTypeCode, this))
                {
                    panelSetup.Visible = true;
                    panelUpgrade.Visible = false;
                }
            }
        }

        /// <summary>
        /// Raise the setup complete event
        /// </summary>
        private void RaiseSetupComplete()
        {
            SetupComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}
