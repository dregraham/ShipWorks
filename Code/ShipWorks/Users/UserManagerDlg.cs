using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.Users.Security;

namespace ShipWorks.Users
{
    /// <summary>
    /// Window for managing ShipWorks users
    /// </summary>
    public partial class UserManagerDlg : Form
    {
        List<UserEntity> users = null;

        Dictionary<long, PermissionSet> permissionCache = new Dictionary<long, PermissionSet>();

        /// <summary>
        /// Constructor
        /// </summary>
        public UserManagerDlg()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadUserCollection();
            LoadUserGrid();

            if (sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }

            UpdateButtonState();
        }

        /// <summary>
        /// Get the user currently selected in the grid
        /// </summary>
        public UserEntity SelectedUser
        {
            get
            {
                if (sandGrid.SelectedElements.Count != 1)
                {
                    return null;
                }

                return sandGrid.SelectedElements[0].Tag as UserEntity;
            }
            set
            {
                if (value != null)
                {
                    foreach (GridRow row in sandGrid.Rows)
                    {
                        if (((UserEntity) row.Tag).UserID == value.UserID)
                        {
                            row.Selected = true;
                            return;
                        }
                    }
                }

                sandGrid.SelectedElements.Clear();
            }
        }

        /// <summary>
        /// Open the User Editor for editing the properties of a user
        /// </summary>
        private void OnEditUser(object sender, EventArgs e)
        {
            EditUser(SelectedUser);
        }

        /// <summary>
        /// Load the user collection from the database
        /// </summary>
        private void LoadUserCollection()
        {
            UserManager.CheckForChangesNeeded();
            users = UserManager.GetUsers(true);
        }

        /// <summary>
        /// Load
        /// </summary>
        private void LoadUserGrid()
        {
            sandGrid.Rows.Clear();

            foreach (UserEntity user in users)
            {
                if (user.IsDeleted && !showDeletedUsers.Checked)
                {
                    continue;
                }

                GridRow gridRow = sandGrid.NewRow();

                // Tag and add it
                gridRow.Tag = user;
                sandGrid.Rows.Add(gridRow);

                gridRow.Cells[0].SetValue(user.Username);
                gridRow.Cells[0].Image = user.IsDeleted ? Resources.user_deleted_16 : Resources.user_16;

                gridRow.SetCellValue(gridColumnEmail, user.Email);
                gridRow.SetCellValue(gridColumnDescription, GetUserDescription(user));

                if (user.IsDeleted)
                {
                    gridRow.Font = new Font(gridRow.Font, FontStyle.Strikeout);
                    gridRow.Cells[2].Font = Font;

                    foreach (GridCell cell in gridRow.Cells)
                    {
                        cell.ForeColor = Color.DimGray;
                    }
                }
            }

        }

        /// <summary>
        /// Get the value to display in the description column
        /// </summary>
        private string GetUserDescription(UserEntity user)
        {
            if (user.IsDeleted)
            {
                if (user.IsAdmin)
                {
                    return "Deleted (Admin)";
                }
                else
                {
                    return "Deleted";
                }
            }

            if (user.IsAdmin)
            {
                return "Administrator";
            }

            return "";
        }

        /// <summary>
        /// When the grid selection changes, update the UI
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Row activated (double-click)
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            EditUser(e.Row.Tag as UserEntity);
        }

        /// <summary>
        /// Open the editor for the given user
        /// </summary>
        private void EditUser(UserEntity user)
        {
            EditUser(user, UserSettingsDlg.Section.User);
        }

        /// <summary>
        /// Open the editor for the given user
        /// </summary>
        private void EditUser(UserEntity user, UserSettingsDlg.Section section)
        {
            using (UserSettingsDlg dlg = new UserSettingsDlg(user, GetUserPermissions(user.UserID), section))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUserGrid();

                    // May select nothing if user has been deleted and we are not showing deleted
                    SelectedUser = user;
                }
            }
        }

        /// <summary>
        /// Get the permission set for the specified user
        /// </summary>
        private PermissionSet GetUserPermissions(long userID)
        {
            PermissionSet permissions;
            if (!permissionCache.TryGetValue(userID, out permissions))
            {
                permissions = new PermissionSet();
                permissions.Load(userID);

                permissionCache[userID] = permissions;
            }

            return permissions;
        }

        /// <summary>
        /// Update the state of the UI buttons
        /// </summary>
        private void UpdateButtonState()
        {
            edit.Enabled = SelectedUser != null;
            viewAuditHistory.Enabled = SelectedUser != null;
            copyRightsFrom.Enabled = SelectedUser != null && !SelectedUser.IsAdmin && users.Count > 1;

            bool canDelete = false;

            if (SelectedUser != null && !SelectedUser.IsDeleted)
            {
                if (!SelectedUser.IsAdmin)
                {
                    canDelete = true;
                }
                else
                {
                    canDelete = UserManager.GetUsers(false).Count(u => u.IsAdmin) > 1;
                }
            }

            delete.Enabled = canDelete;
        }

        /// <summary>
        /// Changing if we should show deleted users or not
        /// </summary>
        private void OnChangeShowDeleted(object sender, EventArgs e)
        {
            UserEntity selected = SelectedUser;
            LoadUserGrid();
            SelectedUser = selected;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        private void OnNewUser(object sender, EventArgs e)
        {
            using (AddUserWizard dlg = new AddUserWizard())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUserCollection();
                    LoadUserGrid();

                    SelectedUser = dlg.CreatedUser;
                }
            }
        }

        /// <summary>
        /// Opening the menu to copy rights from another user
        /// </summary>
        private void OnOpeningMenuCopyRightsFrom(object sender, CancelEventArgs e)
        {
            PermissionUtility.PopulateCopyRightsFromMenu(
                menuCopyRightsFrom,
                GetUsersSansSelected(),
                new EventHandler(OnCopyRightsFromUser));
        }

        /// <summary>
        /// Get a list of users that excludes the selected user
        /// </summary>
        private List<UserEntity> GetUsersSansSelected()
        {
            List<UserEntity> usersSansSelected = new List<UserEntity>(users);

            // Remove the selected user
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].UserID == SelectedUser.UserID)
                {
                    usersSansSelected.RemoveAt(i);
                    break;
                }
            }

            return usersSansSelected;
        }

        /// <summary>
        /// Copy rights from a given user
        /// </summary>
        void OnCopyRightsFromUser(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            long userID = (long) item.Tag;

            PermissionSet source = GetUserPermissions(userID);
            PermissionSet target = GetUserPermissions(SelectedUser.UserID);

            target.CopyFrom(source);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                target.Save(adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the selected user
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            UserEntity user = SelectedUser;

            DialogResult result = MessageHelper.ShowQuestion(this,
                string.Format("Delete user '{0}'?", user.Username));

            if (result == DialogResult.OK)
            {
                user.IsDeleted = true;

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(user);
                }

                LoadUserGrid();
            }
        }

        /// <summary>
        /// View the audit trail for the current user5
        /// </summary>
        private void OnViewAudit(object sender, EventArgs e)
        {
            EditUser(SelectedUser, UserSettingsDlg.Section.Audit);
        }
    }
}