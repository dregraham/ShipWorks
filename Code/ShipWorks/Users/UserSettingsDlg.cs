using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Users.Logon;
using ShipWorks.Users.Security;

namespace ShipWorks.Users
{
    /// <summary>
    /// Window for managing the settings of a specific user
    /// </summary>
    public partial class UserSettingsDlg : Form
    {
        UserEntity user;
        PermissionSet permissions;

        static Guid auditGridSettingsKey = new Guid("{A64E840C-43E2-4b89-A873-53793D2D0DA0}");

        public enum Section
        {
            User,
            Audit,
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserSettingsDlg(UserEntity user, PermissionSet permissions)
            : this(user, permissions, Section.User)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserSettingsDlg(UserEntity user, PermissionSet permissions, Section section)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);

            this.user = user;
            this.permissions = permissions;

            switch (section)
            {
                case Section.User: optionControl.SelectedPage = optionPageUser; break;
                case Section.Audit: optionControl.SelectedPage = optionPageAudit; break;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            titleUsername.Text = user.Username;
            titleUserDescription.Text = GetUserDescription(user);

            username.Text = user.Username;
            email.Text = user.Email;

            accountAdmin.Checked = user.IsAdmin;
            accountStandard.Checked = !user.IsAdmin;

            statusActive.Checked = !user.IsDeleted;
            statusDeleted.Checked = user.IsDeleted;

            UpdateAccountChangeCapabilities();

            // Load the security editor
            permissionEditor.LoadPermissions(permissions);

            // Load the audit
            auditControl.Initialize(auditGridSettingsKey, layout =>
                {
                    layout.AllColumns[AuditFields.UserID].Visible = false;
                });

            // Lock the control into displaying stuff for this user
            auditControl.LockUserSearchCriteria(user.UserID);
        }

        /// <summary>
        /// It may not be possible to delete a user, or change their admin status.
        /// </summary>
        private void UpdateAccountChangeCapabilities()
        {
            // You can always delete a standard user, or promote them to an admin
            if (!user.IsAdmin)
            {
                return;
            }

            // See how many active admins there are besides this one
            int activeAdmins = UserManager.GetUsers(false).Count(u => u.IsAdmin && u.UserID != user.UserID);

            // If there is only one admin, we cannot remove this admin or delete him
            if (activeAdmins == 0)
            {
                statusDeleted.Enabled = false;
                accountStandard.Enabled = false;

                panelNoChangeAccount.Visible = true;
            }
        }

        /// <summary>
        /// Get the description of the user to display
        /// </summary>
        private string GetUserDescription(UserEntity user)
        {
            if (user.IsDeleted)
            {
                return "Deleted";
            }

            if (user.IsAdmin)
            {
                return "Administrator";
            }

            return "Standard User";
        }

        /// <summary>
        /// Changing standard \ admin
        /// </summary>
        private void OnChangeAccountType(object sender, EventArgs e)
        {
            panelAdminAllRights.Visible = accountAdmin.Checked;
            permissionEditor.Visible = accountStandard.Checked;
            panelNotUntilNextLogon.Visible = accountStandard.Checked;
        }

        /// <summary>
        /// Open the window to allow change of password
        /// </summary>
        private void OnChangePassword(object sender, EventArgs e)
        {
            using (UserChangePasswordDlg dlg = new UserChangePasswordDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    user.Password = UserUtility.HashPassword(dlg.Password);
                }
            }
        }

        /// <summary>
        /// User is saving the state of the app
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            string name = username.Text.Trim();

            if (name.Length == 0)
            {
                MessageHelper.ShowMessage(this, "You must enter a username.");
                return;
            }

            if (!EmailUtility.IsValidEmailAddress(email.Text))
            {
                MessageHelper.ShowMessage(this, "Please enter a valid email address.");
                return;
            }

            if (UserUtility.IsUsernameTaken(name, user))
            {
                MessageHelper.ShowMessage(this, "The username is already being used by another user.");
                return;
            }

            user.Username = name;
            user.Email = email.Text;
            user.IsAdmin = accountAdmin.Checked;
            user.IsDeleted = statusDeleted.Checked;

            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    adapter.SaveAndRefetch(user);
                    permissions.Save(adapter);

                    adapter.Commit();
                }

                DialogResult = DialogResult.OK;
            }
            catch (DuplicateNameException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                return;
            }
        }

        /// <summary>
        /// The window has closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            // If the operation was cancelled, rollback
            if (DialogResult != DialogResult.OK)
            {
                user.RollbackChanges();
                permissions.CancelChanges();
            }
        }
    }
}