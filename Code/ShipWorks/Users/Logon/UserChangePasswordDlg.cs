using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Users.Logon
{
    /// <summary>
    /// Window for allowing the user to change their password
    /// </summary>
    public partial class UserChangePasswordDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserChangePasswordDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The password the user entered.  Only valid if the Form result is DialogResult.OK
        /// </summary>
        public string Password
        {
            get
            {
                return password.Text;
            }
        }

        /// <summary>
        /// Once they type something, we'll let them say ok
        /// </summary>
        private void OnTextChanged(object sender, EventArgs e)
        {
            ok.Enabled = true;
        }

        /// <summary>
        /// Saving the change
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (password.Text != passwordAgain.Text)
            {
                MessageHelper.ShowMessage(this, "The passwords you typed do not match.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

    }
}