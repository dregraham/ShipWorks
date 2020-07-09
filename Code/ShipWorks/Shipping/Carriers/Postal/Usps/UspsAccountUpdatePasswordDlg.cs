﻿using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Window for updating the password of a USPS account
    /// </summary>
    public partial class UspsAccountUpdatePasswordDlg : Form
    {
        readonly UspsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsAccountUpdatePasswordDlg(UspsAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
            username.Text = account.Username;
        }

        /// <summary>
        /// OK to try to update the password
        /// </summary>
        private async void OnOK(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(password.Text))
            {
                MessageHelper.ShowError(this, "Please enter a password.");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                new UspsWebClient(IoC.UnsafeGlobalLifetimeScope, (UspsResellerType) account.UspsReseller)
                    .AuthenticateUser(account.Username, password.Text);

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    account.Password = SecureText.Encrypt(password.Text, account.Username);
                    adapter.SaveAndRefetch(account);

                    adapter.Commit();
                }

                UspsAccountManager.CheckForChangesNeeded();

                if (!string.IsNullOrWhiteSpace(account.ShipEngineCarrierId))
                {
                    using(ILifetimeScope scope = IoC.BeginLifetimeScope())
                    {
                        IShipEngineWebClient client = scope.Resolve<IShipEngineWebClient>();
                        Result result = await client.UpdateStampsAccount(account.ShipEngineCarrierId, account.Username, password.Text);

                        if(result.Failure)
                        {
                            MessageHelper.ShowError(this, $"An error occurred updating password. {Environment.NewLine}{result.Message}");
                            return;
                        }
                    }
                }

                DialogResult = DialogResult.OK;

            }
            catch (UspsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
