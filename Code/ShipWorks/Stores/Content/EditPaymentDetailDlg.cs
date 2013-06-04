using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Window for adding\editing a Payment Detail of an order
    /// </summary>
    public partial class EditPaymentDetailDlg : Form
    {
        OrderPaymentDetailEntity detail;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditPaymentDetailDlg(OrderPaymentDetailEntity detail)
        {
            InitializeComponent();

            this.detail = detail;

            Text = detail.IsNew ? "Add Payment Detail" : "Edit Payment Detail";
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.OrdersModify, detail.OrderID);

            label.Text = detail.Label;
            value.Text = PaymentDetailSecurity.ReadValue(detail);

            // Can't allow a user that can't see sensitive data to edit sensitive data
            if (!detail.IsNew && 
                PaymentDetailSecurity.IsCreditCardNumber(detail) &&
                !UserSession.Security.HasPermission(PermissionType.OrdersViewPaymentData, detail.OrderPaymentDetailID))
            {
                label.Enabled = false;
                value.Enabled = false;
            }
        }

        /// <summary>
        /// Save any changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (label.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Enter a label for the payment detail.");
                return;
            }

            // Update the values
            detail.Label = label.Text;
            detail.Value = value.Text;

            // Protect the value if necessary
            PaymentDetailSecurity.Protect(detail);

            try
            {
                OrderUtility.SavePaymentDetail(detail);

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this,
                    "The payment detail has been edited or deleted by another user, and your changes could not be saved.");

                DialogResult = DialogResult.Abort;
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowError(this,
                    "The order has been deleted, and the payment detail cannot be saved.");

                DialogResult = DialogResult.Abort;
            }
        }
    }
}
