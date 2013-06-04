using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// UserControl for permissions that are specific to a single store
    /// </summary>
    public partial class StorePermissionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StorePermissionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the bindings for the given store into the controller
        /// </summary>
        public void LoadBindings(long storeID, PermissionBindingController bindingController)
        {
            bindingController.AddBinding(manageOrders, PermissionType.OrdersModify, storeID);
            bindingController.AddBinding(viewPaymentDetails, PermissionType.OrdersViewPaymentData, storeID);
            bindingController.AddBinding(editOrderNotes, PermissionType.OrdersEditNotes, storeID);
            bindingController.AddBinding(editOrderStatus, PermissionType.OrdersEditStatus, storeID);

            bindingController.AddBinding(prepareShipments, PermissionType.ShipmentsCreateEditProcess, storeID);
            bindingController.AddBinding(voidShipments, PermissionType.ShipmentsVoidDelete, storeID);

            bindingController.AddBinding(sendEmail, PermissionType.OrdersSendEmail, storeID);
        }
    }
}
