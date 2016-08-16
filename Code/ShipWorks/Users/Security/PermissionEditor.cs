using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.UI.Utility;
using ShipWorks.UI.Controls;
using ShipWorks.Properties;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// UserControl for editing user rights (permissions)
    /// </summary>
    public partial class PermissionEditor : UserControl
    {
        PermissionBindingController bindingController;

        string customerEmailText;
        string customerCreateText;
        string customerDeleteText;
        string customerNotesText;

        /// <summary>
        /// Constructor
        /// </summary>
        public PermissionEditor()
        {
            InitializeComponent();

            customerEmailText = customerEmailLabel.Text;
            customerCreateText = customerCreateLabel.Text;
            customerDeleteText = customerDeleteLabel.Text;
            customerNotesText = customerNotesLabel.Text; ;

            ThemedBorderProvider.Apply(this);
        }

        /// <summary>
        /// Load the specified PermissionSet into the control for editing
        /// </summary>
        public void LoadPermissions(PermissionSet permissions)
        {
            if (permissions == null)
            {
                throw new ArgumentNullException("permissions");
            }

            // Lazy initialization
            if (bindingController == null)
            {
                LoadBindings();
            }

            bindingController.LoadPermissions(permissions);
        }

        /// <summary>
        /// Load which permissions the checkboxes represent.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadBindings()
        {
            bindingController = new PermissionBindingController();

            bindingController.AddBinding(manageActions, PermissionType.ManageActions);
            bindingController.AddBinding(manageFilters, PermissionType.ManageFilters);
            bindingController.AddBinding(manageTemplates, PermissionType.ManageTemplates);

            List<StoreEntity> stores = StoreManager.GetAllStores();

            if (stores.Count == 1)
            {
                StorePermissionsControl storeControl = new StorePermissionsControl();
                panelStores.Controls.Add(storeControl);
                storeControl.LoadBindings(stores[0].StoreID, bindingController);
                panelStores.Height = storeControl.Bottom;                
            }
            else
            {
                Label labelStores = new Label();
                labelStores.Text = "Stores";
                labelStores.Font = new Font(Font, FontStyle.Bold);
                labelStores.Location = new Point(3, 0);
                labelStores.AutoSize = true;

                panelStores.Controls.Add(labelStores);

                // Create a group for each store
                foreach (StoreEntity store in stores)
                {
                    StorePermissionsControl storeControl = new StorePermissionsControl();
                    storeControl.LoadBindings(store.StoreID, bindingController);
                    storeControl.Location = new Point(2, 4);

                    CollapsibleGroupControl group = new CollapsibleGroupControl();
                    group.SectionName = store.StoreName;
                    group.Collapsed = false;
                    group.SettingsKey = string.Format("Permissions_Store{0}", store.StoreID);
                    
                    group.ContentPanel.Controls.Add(storeControl);

                    group.Height = storeControl.Height + (group.Height - group.ContentPanel.Height) + 5;
                    group.Width = 275;
                    group.SizeChanged += new EventHandler(OnGroupSizeChanged);

                    panelStores.Controls.Add(group);
                }

                LayoutStoreGroups();
            }

            // Listen for changes to permissions to update the display
            bindingController.PermissionChanged += new EventHandler(OnBoundPermissionsChanged);
        }

        /// <summary>
        /// Some of the bound permissions have changed
        /// </summary>
        void OnBoundPermissionsChanged(object sender, EventArgs e)
        {
            UpdateCustomerPermissionDisplay();
        }

        /// <summary>
        /// Customer permissions are derived from order permissions.  This updates their display based 
        /// on the current set of order permissions.
        /// </summary>
        private void UpdateCustomerPermissionDisplay()
        {
            SecurityContext context = new SecurityContext(bindingController.PermissionSet, false);

            bool canEmail = context.HasPermission(PermissionType.CustomersSendEmail);
            bool canCreate = context.HasPermission(PermissionType.CustomersCreateEdit);
            bool canDelete = context.HasPermission(PermissionType.CustomersDelete);
            bool canNotes = context.HasPermission(PermissionType.CustomersEditNotes);

            customerEmailLabel.Text = string.Format(customerEmailText, canEmail ? "Can" : "Cannot");
            customerEmailPicture.Image = canEmail ? Resources.check16 : Resources.forbidden;
            infoTipEmailCustomers.Left = customerEmailLabel.Right;

            customerCreateLabel.Text = string.Format(customerCreateText, canCreate ? "Can" : "Cannot");
            customerCreatePicture.Image = canCreate ? Resources.check16 : Resources.forbidden;
            infotipCreateCustomers.Left = customerCreateLabel.Right;

            customerDeleteLabel.Text = string.Format(customerDeleteText, canDelete ? "Can" : "Cannot");
            customerDeletePicture.Image = canDelete ? Resources.check16 : Resources.forbidden;
            infoTipDeleteCustomers.Left = customerDeleteLabel.Right;

            customerNotesLabel.Text = string.Format(customerNotesText, canNotes ? "Can" : "Cannot");
            customerNotesPicture.Image = canNotes ? Resources.check16 : Resources.forbidden;
            infoTipCustomerNotes.Left = customerNotesLabel.Right;
        }

        /// <summary>
        /// When the size of one of the groups changes, we have to lay them all out
        /// </summary>
        void OnGroupSizeChanged(object sender, EventArgs e)
        {
            LayoutStoreGroups();
        }

        /// <summary>
        /// When the size of one of the groups changes, we have to lay them all out
        /// </summary>
        private void LayoutStoreGroups()
        {
            int location = panelStores.Controls[0].Bottom + 5;

            foreach (CollapsibleGroupControl group in panelStores.Controls.OfType<CollapsibleGroupControl>())
            {
                group.Location = new Point(10, location);
                location = group.Bottom + 10;
            }

            panelStores.Height = location;
        }
    }
}
