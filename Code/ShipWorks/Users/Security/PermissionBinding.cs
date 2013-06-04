using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Used to bind a UI element to a permission setting
    /// </summary>
    public class PermissionBinding
    {
        CheckBox checkBox;
        PermissionType permission;
        long? objectID;

        /// <summary>
        /// Constructor
        /// </summary>
        public PermissionBinding(PermissionType permission, long? objectID, CheckBox checkBox)
        {
            this.permission = permission;
            this.objectID = objectID;
            this.checkBox = checkBox;
        }

        /// <summary>
        /// The UI that is controling the permissino
        /// </summary>
        public CheckBox CheckBox
        {
            get { return checkBox; }
        }

        /// <summary>
        /// The type of permission
        /// </summary>
        public PermissionType Permission
        {
            get { return permission; }
        }

        /// <summary>
        /// The specific object that the permission controls access to, if any.
        /// </summary>
        public long? ObjectID
        {
            get { return objectID; }
        }
    }
}
