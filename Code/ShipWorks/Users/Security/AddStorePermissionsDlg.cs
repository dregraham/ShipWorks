using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Window for editing the permissions that will apply to existing users for a new store
    /// </summary>
    public partial class AddStorePermissionsDlg : Form
    {
        PermissionSet permissions;
        long storeID;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddStorePermissionsDlg(PermissionSet permissions, long storeID)
        {
            InitializeComponent();

            this.permissions = permissions;
            this.storeID = storeID;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            PermissionBindingController bindingController = new PermissionBindingController();

            // Load the bindings.  
            rightsControl.LoadBindings(storeID, bindingController);

            // Load up the defaults
            bindingController.LoadPermissions(permissions);
        }
    }
}
