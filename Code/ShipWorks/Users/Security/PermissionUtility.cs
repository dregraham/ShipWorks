using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// UI utilities for permissions
    /// </summary>
    public static class PermissionUtility
    {
        /// <summary>
        /// Populate the specified menu with the users whose permissions can be copied.  The menu is first cleared.
        /// </summary>
        public static void PopulateCopyRightsFromMenu(ContextMenuStrip menuCopyRightsFrom, ICollection<UserEntity> users, EventHandler clickEventHandler)
        {
            menuCopyRightsFrom.Items.Clear();

            List<UserEntity> deleted = new List<UserEntity>();

            // Add all the active users
            foreach (UserEntity user in users)
            {
                if (user.IsDeleted)
                {
                    deleted.Add(user);
                }
                else
                {
                    AddCopyFromUser(menuCopyRightsFrom.Items, user, clickEventHandler);
                }
            }

            // See if any deleted users are out there
            if (deleted.Count > 0)
            {
                menuCopyRightsFrom.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem deletedParent = new ToolStripMenuItem("Deleted Users");
                menuCopyRightsFrom.Items.Add(deletedParent);

                foreach (UserEntity user in deleted)
                {
                    AddCopyFromUser(deletedParent.DropDownItems, user, clickEventHandler);
                }
            }

            // If nothing was added, still add a note that such
            if (menuCopyRightsFrom.Items.Count == 0)
            {
                ToolStripItem item = menuCopyRightsFrom.Items.Add("(No Users)");
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Add the specified user to the menu for copying rights form
        /// </summary>
        private static void AddCopyFromUser(ToolStripItemCollection itemCollection, UserEntity user, EventHandler clickEventHandler)
        {
            ToolStripItem item = itemCollection.Add(user.Username);
            item.Tag = user.UserID;

            if (user.IsDeleted)
            {
                item.Image = Resources.user_deleted_16;
            }
            else
            {
                item.Image = Resources.user_16;
            }

            if (user.IsAdmin)
            {
                item.Text += " (Administrator)";
                item.Enabled = false;
            }

            item.Click += clickEventHandler;
        }
    }
}
