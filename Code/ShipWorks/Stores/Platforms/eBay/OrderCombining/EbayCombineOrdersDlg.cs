﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Paging;
using Interapptive.Shared.UI;
using ShipWorks.Users.Security;
using ShipWorks.Users;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    public partial class EbayCombineOrdersDlg : Form
    {
        // The type of combine operation being configured
        CombineType combineType;

        // some items were removed due to security permissions
        bool securityRestricted = false;

        // collection of orders that were selected by the user
        // when they selected to combine.
        List<CombinedOrder> combiningOrders;
        public List<CombinedOrder> SelectedOrders
        {
            get { return combiningOrders; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayCombineOrdersDlg(CombineType combineType, List<CombinedOrder> orders)
        {
            InitializeComponent();

            this.combiningOrders = new List<CombinedOrder>(orders);
            this.combineType = combineType;

            if (combineType == CombineType.Local)
            {
                remotePanel.Visible = false;
                localPanel.Visible = true;
                combineButton.Text = "&Combine";
                Text = "Combine eBay Orders";
            }
            else
            {
                localPanel.Visible = false;
                remotePanel.Visible = true;
                combineButton.Text = "&Invoice";
                Text = "Send eBay Invoice";
            }
        }

        /// <summary>
        /// Load all of the combined payments 
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            contentPanel.SuspendLayout();

            List<long> storesNotAllowed = combiningOrders.Where(c => !UserSession.Security.HasPermission(PermissionType.OrdersModify, c.StoreID)).Select(c => c.StoreID).ToList();
            securityRestricted = storesNotAllowed.Count > 0;

            // remove those which belong to the stores not allowed
            combiningOrders.RemoveAll(c => storesNotAllowed.Contains(c.StoreID));

            // load the UI
            foreach (CombinedOrder combined in combiningOrders)
            {
                // create an editor control, and add it to the ui
                EbayCombineOrderControl paymentControl = new EbayCombineOrderControl(combineType, combined);
                contentPanel.Controls.Add(paymentControl);
            }

            // update the arrangment of payment controls
            UpdatePaymentControlLayout();

            // resize the form if needed
            if (combiningOrders.Count == 1)
            {
                Height = 440;
            }
            else
            {
                Height = 650;
            }

            contentPanel.ResumeLayout();
        }

        /// <summary>
        /// Layout all of the payment controls correctly
        /// </summary>
        private void UpdatePaymentControlLayout()
        {
            int y = 4;

            for (int i = 0; i < contentPanel.Controls.Count; i++)
            {
                EbayCombineOrderControl paymentControl = (EbayCombineOrderControl) contentPanel.Controls[i];

                paymentControl.Location = new Point(0, y);
                paymentControl.Width = contentPanel.DisplayRectangle.Width;
                paymentControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                y = paymentControl.Bottom + 2;
            }
        }

        /// <summary>
        /// Combine was clicked, collate all selected, combined orders
        /// </summary>
        private void OnCombineClick(object sender, EventArgs e)
        {
            foreach (EbayCombineOrderControl orderControl in contentPanel.Controls.OfType<EbayCombineOrderControl>().Where(c => c.IsSelected))
            {
                if (orderControl.CombinedOrder.Components.Count(c => c.Included) <= 1)
                {
                    MessageHelper.ShowError(this, string.Format("At least two orders for buyer '{0}' must be selected for combining.", orderControl.CombinedOrder.EbayBuyerID));
                    return;
                }
            }

            // remove all unselected ones
            foreach (EbayCombineOrderControl orderControl in contentPanel.Controls.OfType<EbayCombineOrderControl>().Where( control => !control.IsSelected))
            {
                combiningOrders.Remove(orderControl.CombinedOrder);
            }

            // OK
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel the combining operation
        /// </summary>
        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Display any errors/warnings that occurred during the loading of the window
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            if (securityRestricted)
            {
                MessageHelper.ShowMessage(this, "One or more orders were excluded because the current ShipWorks user does not have permissions to modify orders.");
            }
        }
    }
}
