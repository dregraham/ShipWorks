using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.UI.Wizard;
using ShipWorks.Users.Security;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wizard for creating a new user in ShipWorks
    /// </summary>
    public partial class AddUserWizard : WizardForm
    {
        UserEntity user;
        PermissionSet permissions;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddUserWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            permissions = UserUtility.CreateDefaultPermissionSet();
            permissionEditor.LoadPermissions(permissions);
        }

        /// <summary>
        /// The user that was creatd if the return result is DialogResult.OK
        /// </summary>
        public UserEntity CreatedUser
        {
            get
            {
                return user;
            }
        }

        /// <summary>
        /// Stepping next off the account page
        /// </summary>
        private void OnStepNextAccountPage(object sender, WizardStepEventArgs e)
        {
            string name = username.Text.Trim();

            if (name.Length == 0)
            {
                MessageHelper.ShowMessage(this, "You must enter a username.");
                e.NextPage = CurrentPage;
                return;
            }

            if (!EmailUtility.IsValidEmailAddress(email.Text))
            {
                MessageHelper.ShowMessage(this, "Please enter a valid email address.");
                e.NextPage = CurrentPage;
                return;
            }

            if (password.Text != passwordAgain.Text)
            {
                MessageHelper.ShowMessage(this, "The passwords you typed do not match.");
                e.NextPage = CurrentPage;
                return;
            }

            if (UserUtility.IsUsernameTaken(name))
            {
                MessageHelper.ShowMessage(this, "The username is already being used by another user.");
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping into the user rights page
        /// </summary>
        private void OnSteppingIntoRightsPage(object sender, WizardSteppingIntoEventArgs e)
        {
            panelAdminAllRights.Visible = accountAdmin.Checked;
            permissionEditor.Visible = accountStandard.Checked;
            copyRightsFrom.Visible = accountStandard.Checked;
        }

        /// <summary>
        /// Stepping next from the user rights page
        /// </summary>
        private void OnStepNextRightsPage(object sender, WizardStepEventArgs e)
        {
            try
            {
                string name = username.Text.Trim();

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    user = UserUtility.CreateUser(name, email.Text.Trim(), password.Text, accountAdmin.Checked, adapter);
                    permissions.CopyTo(user.UserID, adapter);

                    adapter.Commit();
                }
            }
            catch (DuplicateNameException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Opening the menu for copying rights
        /// </summary>
        private void OnOpeningCopyFromMenu(object sender, CancelEventArgs e)
        {
            PermissionUtility.PopulateCopyRightsFromMenu(menuCopyRightsFrom, UserManager.GetUsers(true), new EventHandler(OnCopyRightsFromUser));
        }

        /// <summary>
        /// Copy rights from a given user
        /// </summary>
        void OnCopyRightsFromUser(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            long userID = (long) item.Tag;

            PermissionSet copyPermissions = new PermissionSet();
            copyPermissions.Load(userID);

            permissions.CopyFrom(copyPermissions);
            permissionEditor.LoadPermissions(permissions);
        }
    }
}