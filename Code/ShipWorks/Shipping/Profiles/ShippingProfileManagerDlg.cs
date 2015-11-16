using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.Data.Connection;
using Divelements.SandGrid.Specialized;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.MessageBoxes;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Window for managing a user's shipping profiles
    /// </summary>
    public partial class ShippingProfileManagerDlg : Form
    {
        long initialProfileID = -1;
        ShipmentTypeCode? restriction;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDlg(ShipmentTypeCode? restriction)
        {
            InitializeComponent();

            this.restriction = restriction;

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadShipmentTypeMenuList();
            UpdateButtonState();
        }

        /// <summary>
        /// Load the menu list for choosing what shipment type
        /// </summary>
        private void LoadShipmentTypeMenuList()
        {
            foreach (ShipmentType shipmentType in GetEnabledShipmentTypes())
            {
                menuList.Items.Add(new EnumEntry<ShipmentTypeCode>(shipmentType.ShipmentTypeCode));
            }

            if (menuList.Items.Count > 0)
            {
                menuList.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Gets the shipment type currently selected in the menu list
        /// </summary>
        private ShipmentTypeCode SelectedShipmentType
        {
            get
            {
                if (menuList.Items.Count > 0)
                {
                    return ((EnumEntry<ShipmentTypeCode>) menuList.SelectedItem).Value;
                }

                return ShipmentTypeCode.None;
            }
        }

        /// <summary>
        /// Changing the selected shipment type
        /// </summary>
        private void OnChangeShipmentType(object sender, EventArgs e)
        {
            LoadProfiles();
        }

        /// <summary>
        /// Load the profiles for the shipmenttype selected in the menulist
        /// </summary>
        private void LoadProfiles()
        {
            LoadProfiles(SelectedShipmentType);
        }

        /// <summary>
        /// Load all the profiles fro the given shipment type into the grid
        /// </summary>
        private void LoadProfiles(ShipmentTypeCode shipmentTypeCode)
        {
            sandGrid.Rows.Clear();

            // This orders the default to the top, followed by the rest
            foreach (ShippingProfileEntity profile in ShippingProfileManager.Profiles.OrderBy(p => p.ShipmentTypePrimary ? "____" : p.Name))
            {
                if (profile.ShipmentType == (int) shipmentTypeCode)
                {
                    GridRow row = new GridRow(new GridCell[] 
                    { 
                        new GridCell(profile.Name),
                        new GridCell((profile.ShipmentType == (int) ShipmentTypeCode.None)
                            ? "Any" 
                            : ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType).ShipmentTypeName)
                    });

                    sandGrid.Rows.Add(row);
                    row.Tag = profile;

                    if (profile.ShippingProfileID == initialProfileID)
                    {
                        row.Selected = true;
                    }
                }
            }

            if (sandGrid.SelectedElements.Count == 0 && sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool editable = sandGrid.SelectedElements.Count > 0;
            bool deletable = editable && !((ShippingProfileEntity) sandGrid.SelectedElements[0].Tag).ShipmentTypePrimary;

            add.Enabled = menuList.Items.Count > 0;
            edit.Enabled = editable;
            delete.Enabled = deletable;
        }

        /// <summary>
        /// Selected profile
        /// </summary>
        private void OnChangeSelectedProfile(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Get all the shipment types that are enabled for viewing profiles for
        /// </summary>
        private IEnumerable<ShipmentType> GetEnabledShipmentTypes()
        {
            foreach (ShipmentType shipmentType in ShipmentTypeManager.ShipmentTypes)
            {
                if (shipmentType.ShipmentTypeCode != ShipmentTypeCode.None)
                {
                    if ((restriction == null && ShippingManager.IsShipmentTypeEnabled(shipmentType.ShipmentTypeCode) && ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode)) ||
                        (restriction == shipmentType.ShipmentTypeCode))
                    {
                        yield return shipmentType;
                    }
                }
            }
        }

        /// <summary>
        /// Edit the selected profile
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            ShippingProfileEntity profile = (ShippingProfileEntity) sandGrid.SelectedElements[0].Tag;
            initialProfileID = profile.ShippingProfileID;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ShippingProfileEditorDlg profileEditor = lifetimeScope.Resolve<ShippingProfileEditorDlg>(
                    new TypedParameter(typeof(ShippingProfileEntity), profile)
                );
                profileEditor.ShowDialog(this);

                LoadProfiles((ShipmentTypeCode)profile.ShipmentType);
            }
        }

        /// <summary>
        /// Double-clicked a row (do Edit)
        /// </summary>
        private void OnActivate(object sender, GridRowEventArgs e)
        {
            OnEdit(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Add a new profile
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {
            ShippingProfileEntity profile = new ShippingProfileEntity();
            profile.Name = "";
            profile.ShipmentType = (int) SelectedShipmentType;
            profile.ShipmentTypePrimary = false;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ShippingProfileEditorDlg profileEditor = lifetimeScope.Resolve<ShippingProfileEditorDlg>(
                    new TypedParameter(typeof(ShippingProfileEntity), profile)
                );

                if (profileEditor.ShowDialog(this) == DialogResult.OK)
                {
                    initialProfileID = profile.ShippingProfileID;

                    LoadProfiles((ShipmentTypeCode)profile.ShipmentType);
                }
            }
        }

        /// <summary>
        /// Delete the selected profile
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow row = (GridRow) sandGrid.SelectedElements[0];
            ShippingProfileEntity profile = (ShippingProfileEntity) row.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the profile '{0}'?", profile.Name);

            // Profiles can be referenced by shipping rules
            using (DeleteObjectReferenceDlg dlg = new DeleteObjectReferenceDlg(question, new List<long> { profile.ShippingProfileID } ))
            {
                if (dlg.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(profile);
            }

            ShippingProfileManager.CheckForChangesNeeded();
            LoadProfiles();
        }
    }
}
