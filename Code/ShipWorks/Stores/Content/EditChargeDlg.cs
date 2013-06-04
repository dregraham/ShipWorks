using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Window for editing\adding an order charge
    /// </summary>
    public partial class EditChargeDlg : Form
    {
        OrderChargeEntity charge;
        PanelDataMode dataMode;

        static Dictionary<string, string> commonCharges = new Dictionary<string, string>();

        /// <summary>
        /// Static consturctor
        /// </summary>
        static EditChargeDlg()
        {
            commonCharges.Add("SHIPPING", "Shipping");
            commonCharges.Add("INSURANCE", "Insurance");
            commonCharges.Add("TAX", "Tax");
            commonCharges.Add("ADJUST", "Adjust");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditChargeDlg(OrderChargeEntity charge, PanelDataMode dataMode)
        {
            InitializeComponent();

            if (charge == null)
            {
                throw new ArgumentNullException("charge");
            }

            this.charge = charge;
            this.dataMode = dataMode;

            Text = charge.IsNew ? "Add Charge" : "Edit Charge";
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            type.Items.AddRange(commonCharges.Keys.ToArray());

            if (dataMode == PanelDataMode.LiveDatabase)
            {
                UserSession.Security.DemandPermission(PermissionType.OrdersModify, charge.OrderID);
            }

            type.Text = charge.Type;
            description.Text = charge.Description;
            amount.Amount = charge.Amount;
        }

        /// <summary>
        /// Charge type combobox has changed
        /// </summary>
        private void OnChangeType(object sender, EventArgs e)
        {
            string lookup;
            if (commonCharges.TryGetValue(type.Text, out lookup))
            {
                description.Text = lookup;
            }
        }

        /// <summary>
        /// Save what's changed
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (type.Text.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a type for the charge.");
                return;
            }

            if (description.Text.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a description for the charge.");
                return;
            }

            charge.Type = type.Text;
            charge.Description = description.Text;
            charge.Amount = amount.Amount;

            try
            {
                if (dataMode == PanelDataMode.LiveDatabase)
                {
                    OrderUtility.SaveCharge(charge);
                }

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this,
                    "The charge has been edited or deleted by another user, and your changes could not be saved.");

                DialogResult = DialogResult.Abort;
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowError(this,
                    "The order has been deleted, and the charge cannot be saved.");

                DialogResult = DialogResult.Abort;
            }
        }
    }
}
