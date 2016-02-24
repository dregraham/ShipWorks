using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.UI;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Utility;
using ShipWorks.Editions;

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
        public ShipmentTypeSetupControl(ShipmentType shipmentType)
        {
            InitializeComponent();

            this.shipmentType = shipmentType;

            labelSetup.Text = string.Format(labelSetup.Text, shipmentType.ShipmentTypeName);
            setup.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);

            labelUpgrade.Text = string.Format(labelUpgrade.Text, shipmentType.ShipmentTypeName);
            panelUpgrade.Location = panelSetup.Location;

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = scope.Resolve<ILicenseService>();

                EditionRestrictionLevel editionRestrictionLevel = licenseService.CheckRestriction(EditionFeature.ShipmentType, shipmentType.ShipmentTypeCode);
                if (editionRestrictionLevel != EditionRestrictionLevel.None)
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
                    if (SetupShipmentType(this, shipmentType.ShipmentTypeCode, shipmentType.CreateSetupWizard(lifetimeScope)))
                    {
                        RaiseSetupComplete();
                    }   
                }
            }
        }

        /// <summary>
        /// Setup the given shipment type, returns true if it's setup.
        /// </summary>
        public static bool SetupShipmentType(IWin32Window messageOwner, ShipmentTypeCode shipmentTypeCode, Form setupDlg)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipmentTypeCode);

            try
            {
                using (SqlAppResourceLock setupLock = new SqlAppResourceLock("Setup - " + shipmentType.ShipmentTypeName))
                {
                    using (Form dlg = setupDlg)
                    {
                        if (dlg.ShowDialog(messageOwner) == DialogResult.OK)
                        {
                            ShippingSettings.MarkAsConfigured(shipmentType.ShipmentTypeCode);

                            return true;
                        }
                    }
                }
            }
            catch (SqlAppResourceLockException)
            {
                MessageHelper.ShowInformation(messageOwner, "The shipping provider is currently being setup on another computer.");
            }

            return false;
        }

        /// <summary>
        /// Upgrade the current edition of ShipWorks
        /// </summary>
        private void OnUpgrade(object sender, EventArgs e)
        {
            if (EditionManager.HandleRestrictionIssue(this, EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.ShipmentType, shipmentType.ShipmentTypeCode)))
            {
                panelSetup.Visible = true;
                panelUpgrade.Visible = false;
            }
        }

        /// <summary>
        /// Raise the setup complete event
        /// </summary>
        private void RaiseSetupComplete()
        {
            if (SetupComplete != null)
            {
                SetupComplete(this, EventArgs.Empty);
            }
        }
    }
}
