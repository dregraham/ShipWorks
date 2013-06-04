﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using Divelements.SandGrid;
using ShipWorks.UI;
using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Control for managing UPS accounts
    /// </summary>
    public partial class UpsAccountManagerControl : UserControl
    {
        long initialShipperID = -1;

        private bool initialized = false;

        ShipmentTypeCode shipmentTypeCode;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsAccountManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize(ShipmentTypeCode shipmentTypeCode)
        {
            if (!initialized)
            {
                this.shipmentTypeCode = shipmentTypeCode;

                editionGuiHelper.RegisterElement(add, EditionFeature.UpsAccountLimit, () => UpsAccountManager.Accounts.Count + 1);

                LoadShippers();
            }

            initialized = true;
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadShippers()
        {
            sandGrid.Rows.Clear();

            foreach (UpsAccountEntity shipper in UpsAccountManager.Accounts)
            {
                GridRow row = new GridRow(new string[] { shipper.Description });
                sandGrid.Rows.Add(row);
                row.Tag = shipper;

                if (shipper.UpsAccountID == initialShipperID)
                {
                    row.Selected = true;
                }
            }

            if (sandGrid.SelectedElements.Count == 0 && sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }

            UpdateButtonState();
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool enabled = sandGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            delete.Enabled = enabled;

            editionGuiHelper.UpdateUI();
        }

        /// <summary>
        /// Selected sheet is changing
        /// </summary>
        private void OnChangeSelectedShipper(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Edit the selected sheet
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            UpsAccountEntity shipper = (UpsAccountEntity) sandGrid.SelectedElements[0].Tag;
            initialShipperID = shipper.UpsAccountID;

            using (UpsAccountEditorDlg dlg = new UpsAccountEditorDlg(shipper))
            {
                dlg.ShowDialog(this);

                UpsAccountManager.CheckForChangesNeeded();
                LoadShippers();
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
        /// Add a new custom sheet
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {
            using (UpsSetupWizard dlg = new UpsSetupWizard(shipmentTypeCode, true))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadShippers();
                }
            }
        }

        /// <summary>
        /// Delete the selected custom sheet
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow row = (GridRow) sandGrid.SelectedElements[0];
            UpsAccountEntity shipper = (UpsAccountEntity) row.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the shipper '{0}'?", shipper.Description);

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                UpsAccountManager.DeleteAccount(shipper);
                LoadShippers();
            }
        }    
    }
}
