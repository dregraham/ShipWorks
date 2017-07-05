using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Window for creating a new customer
    /// </summary>
    public partial class AddCustomerDlg : Form
    {
        CustomerEntity customer;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddCustomerDlg()
        {
            InitializeComponent();

            customer = new CustomerEntity();
            customer.InitializeNullsToDefault();

            customer.BillCountryCode = "US";
            customer.ShipCountryCode = "US";
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.CustomersCreateEdit);

            shipBillControl.LoadEntity(customer);
        }

        /// <summary>
        /// The ID of the created customer.  Only valid if the DialogResult is OK
        /// </summary>
        public long CustomerID
        {
            get
            {
                if (DialogResult == DialogResult.OK)
                {
                    return customer.CustomerID;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// User wants to create the customer
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            shipBillControl.SavePendingChanges();

            if (customer.ShipFirstName.Length == 0 || customer.ShipLastName.Length == 0 ||
                customer.BillFirstName.Length == 0 || customer.BillLastName.Length == 0)
            {
                MessageHelper.ShowInformation(this,
                    "You must enter a first and last name for the billing and shipping address.");

                return;
            }

            long? foundID = CustomerProvider.FindExistingCustomer(customer, true, false, SqlAdapter.Default)?.CustomerID;
            if (foundID != null)
            {
                DialogResult result = MessageHelper.ShowQuestion(this,
                    "ShipWorks found an existing customer with the same email address.\n\n" +
                    "Create another customer with this email address?");

                if (result != DialogResult.OK)
                {
                    return;
                }
            }

            foundID = CustomerProvider.FindExistingCustomer(customer, false, true, SqlAdapter.Default)?.CustomerID;
            if (foundID != null)
            {
                DialogResult result = MessageHelper.ShowQuestion(this,
                    "ShipWorks found an existing customer with the same mailing address.\n\n" +
                    "Create another customer with this mailing address?");

                if (result != DialogResult.OK)
                {
                    return;
                }
            }

            Cursor.Current = Cursors.Default;

            SqlAdapter.Default.SaveAndRefetch(customer);

            DialogResult = DialogResult.OK;
        }
    }
}
