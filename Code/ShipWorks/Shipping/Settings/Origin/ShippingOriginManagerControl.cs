using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.Core.Messaging;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.Settings.Origin
{
    /// <summary>
    /// UserControl for adding\editing postal shippers
    /// </summary>
    public partial class ShippingOriginManagerControl : UserControl
    {
        long initialShipperID = -1;

        /// <summary>
        /// Raised after a shipper has been added
        /// </summary>
        public event EventHandler ShipperAdded;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingOriginManagerControl()
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

            foreach (ShippingOriginEntity shipper in ShippingOriginManager.Origins)
            {
                GridRow row = new GridRow(new string[] { shipper.Description });
                sandGrid.Rows.Add(row);
                row.Tag = shipper;

                if (shipper.ShippingOriginID == initialShipperID)
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
            ShippingOriginEntity shipper = (ShippingOriginEntity) sandGrid.SelectedElements[0].Tag;
            initialShipperID = shipper.ShippingOriginID;

            using (ShippingOriginEditorDlg dlg = new ShippingOriginEditorDlg(shipper))
            {
                dlg.ShowDialog(this);

                ShippingOriginManager.CheckForChangesNeeded();
                LoadShippers();

                // Send message when an origin address has been changed
                Messenger.Current.Send(new OriginAddressChangedMessage(null));
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
            ShippingOriginEntity shipper = new ShippingOriginEntity();
            shipper.CountryCode = "US";
            shipper.StateProvCode = "AL";
            shipper.InitializeNullsToDefault();

            using (ShippingOriginEditorDlg dlg = new ShippingOriginEditorDlg(shipper))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    initialShipperID = shipper.ShippingOriginID;

                    LoadShippers();

                    if (ShipperAdded != null)
                    {
                        ShipperAdded(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Delete the selected custom sheet
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow row = (GridRow) sandGrid.SelectedElements[0];
            ShippingOriginEntity shipper = (ShippingOriginEntity) row.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the shipper '{0}'?", shipper.Description);

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.DeleteEntity(shipper);
                }

                ShippingOriginManager.CheckForChangesNeeded();
                LoadShippers();

                // Send message when account has been deleted
                Messenger.Current.Send(new OriginAddressChangedMessage(null));
            }
        }
    }
}
