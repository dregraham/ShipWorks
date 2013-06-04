﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using Rebex.Net;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Window for editing an existing FTP account
    /// </summary>
    public partial class FtpAccountEditorDlg : Form
    {
        FtpAccountEntity ftpAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public FtpAccountEditorDlg(FtpAccountEntity ftpAccount)
        {
            InitializeComponent();

            if (ftpAccount == null)
            {
                throw new ArgumentNullException("ftpAccount");
            }

            this.ftpAccount = ftpAccount;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            host.Text = ftpAccount.Host;
            username.Text = ftpAccount.Username;
            password.Text = SecureText.Decrypt(ftpAccount.Password, ftpAccount.Username);

            port.Text = ftpAccount.Port.ToString();

            FtpSecurityType security = (FtpSecurityType) ftpAccount.SecurityType;
            secureRequired.Checked = (security != FtpSecurityType.Unsecure);
            securityMethod.SelectedIndex = (security != FtpSecurityType.Implicit) ? 0 : 1;

            transferMethod.SelectedIndex = ftpAccount.Passive ? 1 : 0;
        }

        /// <summary>
        /// Changing the secure connection requirement
        /// </summary>
        private void OnChangeSecureConnection(object sender, EventArgs e)
        {
            labelMethod.Enabled = secureRequired.Checked;
            securityMethod.Enabled = secureRequired.Checked;
        }

        /// <summary>
        /// Save the settings in the UI to the given account entity
        /// </summary>
        private bool SaveToAccount(FtpAccountEntity account)
        {
            if (string.IsNullOrEmpty(host.Text))
            {
                MessageHelper.ShowInformation(this, "Please specify the host FTP server.");
                return false;
            }

            if (string.IsNullOrEmpty(username.Text))
            {
                MessageHelper.ShowInformation(this, "Please specify your FTP username.");
                return false;
            }

            int thePort;
            if (!int.TryParse(port.Text, out thePort) || thePort <= 0)
            {
                MessageHelper.ShowError(this, "Please enter a valid port number.");
                return false;
            }

            account.Host = host.Text.Trim();
            account.Username = username.Text.Trim();
            account.Password = SecureText.Encrypt(password.Text, account.Username);

            account.Port = thePort;

            if (secureRequired.Checked)
            {
                account.SecurityType = (int) (securityMethod.SelectedIndex == 0 ? FtpSecurityType.Explicit : FtpSecurityType.Implicit);
            }
            else
            {
                account.SecurityType = (int) FtpSecurityType.Unsecure;
            }

            account.Passive = transferMethod.SelectedIndex == 1;

            return true;
        }

        /// <summary>
        /// Test the connection properties
        /// </summary>
        private void OnTestConnection(object sender, EventArgs e)
        {
            FtpAccountEntity clone = EntityUtility.CloneEntity(ftpAccount);
            SaveToAccount(clone);

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                FtpUtility.TestDataTransfer(clone);

                MessageHelper.ShowInformation(this, "The connection succeeded.");
            }
            catch (FileTransferException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Save the settings in the window to the account
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (SaveToAccount(ftpAccount))
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(ftpAccount);
                }

                DialogResult = DialogResult.OK;
            }
        }
    }
}
