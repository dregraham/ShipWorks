using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Autofac;

namespace ShipWorks.Users.Logon
{
    /// <summary>
    /// Window for logging into ShipWorks as a ShipWorks user
    /// </summary>
    partial class LogonDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogonDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // See if we use the dropdown
            if (ConfigurationData.Fetch().LogOnMethod == (int) LogonMethod.SelectUsername)
            {
                username.Visible = false;

                foreach (UserEntity user in UserManager.GetUsers(false))
                {
                    userlist.Items.Add(user.Username);
                }

                userlist.SelectedItem = UserSession.LastSuccessfulUsername;

                if (userlist.SelectedIndex < 0 && userlist.Items.Count > 0)
                {
                    userlist.SelectedIndex = 0;
                }

                password.Select();
            }
            else
            {
                userlist.Visible = false;
            }
        }

        /// <summary>
        /// User wants to try to log on.
        /// </summary>
        private void OnLogon(object sender, EventArgs e)
        {
            string name;

            if (username.Visible)
            {
                name = username.Text;
            }
            else
            {
                name = (string) userlist.SelectedItem;
            }

            IUserService userService = IoC.UnsafeGlobalLifetimeScope.Resolve<IUserService>();

            EnumResult<UserServiceLogonResultType> logonResult = userService.Logon(new LogonCredentials { Username = name, Password = password.Text, Remember = automaticLogon.Checked });

            if (logonResult.Value == UserServiceLogonResultType.Success)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageHelper.ShowMessage(this, logonResult.Message);
            }
        }

        /// <summary>
        /// Clicking the forgot username link
        /// </summary>
        private void OnForgotUsername(object sender, EventArgs e)
        {
            using (ForgotUsernameDlg dlg = new ForgotUsernameDlg())
            {
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// Clicking the forgot password link
        /// </summary>
        private void OnForgotPassword(object sender, EventArgs e)
        {
            using (ForgotPasswordDlg dlg = new ForgotPasswordDlg())
            {
                dlg.ShowDialog();
            }
        }
    }
}