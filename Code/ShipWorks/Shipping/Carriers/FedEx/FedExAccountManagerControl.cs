﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// UserControl for editing the list of FedEx shippers
    /// </summary>
    public partial class FedExAccountManagerControl : UserControl
    {
        long initialShipperID = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExAccountManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize()
        {
            LoadShippers();
            UpdateButtonState();
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadShippers()
        {
            sandGrid.Rows.Clear();

            foreach (FedExAccountEntity shipper in FedExAccountManager.Accounts)
            {
                GridRow row = new GridRow(new string[] { shipper.Description });
                sandGrid.Rows.Add(row);
                row.Tag = shipper;

                if (shipper.Is2xMigrationPending)
                {
                    row.Cells[0].Text += " (ShipWorks 2)";
                }

                if (shipper.FedExAccountID == initialShipperID)
                {
                    row.Selected = true;
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
            bool enabled = sandGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            delete.Enabled = enabled;
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
            FedExAccountEntity shipper = (FedExAccountEntity) sandGrid.SelectedElements[0].Tag;
            initialShipperID = shipper.FedExAccountID;

            if (shipper.Is2xMigrationPending)
            {
                DialogResult result = MessageHelper.ShowQuestion(this, "This account was migrated from ShipWorks 2.\n\nWould you like to configure the account for use in ShipWorks 3?");

                if (result == DialogResult.OK)
                {
                    using (FedExSetupWizard setupWizard = new FedExSetupWizard(shipper))
                    {
                        setupWizard.ShowDialog(this);
                    }
                }
            }
            else
            {
                using (FedExAccountEditorDlg dlg = new FedExAccountEditorDlg(shipper))
                {
                    dlg.ShowDialog(this);
                }
            }

            FedExAccountManager.CheckForChangesNeeded();
            LoadShippers();
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
            using (FedExSetupWizard dlg = new FedExSetupWizard())
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
            FedExAccountEntity shipper = (FedExAccountEntity) row.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the shipper '{0}'?", shipper.Description);

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                FedExAccountManager.DeleteAccount(shipper);
                LoadShippers();
            }
        }
    }
}
