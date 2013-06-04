using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Used to control and manage UI permission bindings
    /// </summary>
    public class PermissionBindingController
    {
        List<PermissionBinding> bindings = new List<PermissionBinding>();

        PermissionSet permissions;

        /// <summary>
        /// One of the bound permissions changed
        /// </summary>
        public event EventHandler PermissionChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public PermissionBindingController()
        {

        }

        /// <summary>
        /// The permission set currently controled by the binding controller
        /// </summary>
        public PermissionSet PermissionSet
        {
            get { return permissions; }
        }

        /// <summary>
        /// Load the permission set into the UI.
        /// </summary>
        public void LoadPermissions(PermissionSet permissions)
        {
            this.permissions = permissions;

            foreach (PermissionBinding binding in bindings)
            {
                binding.CheckBox.Checked = permissions.HasPermission(binding.Permission, binding.ObjectID);
            }

            if (PermissionChanged != null)
            {
                PermissionChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Bind the specified checkbox to the given permission.  This automates load\save
        /// </summary>
        public void AddBinding(CheckBox checkBox, PermissionType permission)
        {
            AddBinding(checkBox, permission, null);
        }

        /// <summary>
        /// Bind the specified checkbox to the given permission and object.  This automates load\save
        /// </summary>
        public void AddBinding(CheckBox checkBox, PermissionType permission, long? objectID)
        {
            PermissionBinding binding = new PermissionBinding(permission, objectID, checkBox);

            bindings.Add(binding);

            checkBox.Tag = binding;
            checkBox.CheckedChanged += new EventHandler(OnPermissionChanged);
        }

        /// <summary>
        /// A permission value has changed.  We need to update the underlying PermissionSet
        /// </summary>
        private void OnPermissionChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox) sender;
            PermissionBinding binding = (PermissionBinding) checkBox.Tag;

            if (checkBox.Checked)
            {
                permissions.AddPermission(binding.Permission, binding.ObjectID);
            }
            else
            {
                permissions.RemovePermission(binding.Permission, binding.ObjectID);
            }

            if (PermissionChanged != null)
            {
                PermissionChanged(this, EventArgs.Empty);
            }
        }
    }
}
