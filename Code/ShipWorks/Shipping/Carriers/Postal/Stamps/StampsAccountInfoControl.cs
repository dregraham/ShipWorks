﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using ShipWorks.UI;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using System.Threading;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// User control for displaying account info for a stamps.com acount
    /// </summary>
    public partial class StampsAccountInfoControl : UserControl
    {
        StampsAccountEntity account;
        AccountInfo accountInfo;

        bool postagePurchased = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAccountInfoControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public void Initialize(StampsAccountEntity account)
        {
            Cursor.Current = Cursors.WaitCursor;

            this.account = account;
            this.accountName.Text = account.Username;

            int tries = 5;

            while (tries-- > 0)
            {
                bool keepTrying = false;

                try
                {
                    accountInfo = StampsApiSession.GetAccountInfo(account);
                    postage.Text = accountInfo.PostageBalance.AvailablePostage.ToString("c");
                    purchase.Left = postage.Right;

                    panelInfo.Enabled = true;
                }
                catch (StampsException ex)
                {
                    string message = ex.Message;

                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        message = "Your Stamps.com account is not ready yet.";
                        keepTrying = true;
                    }

                    accountName.Text = message;
                    postage.Text = "$0.00";
                    purchase.Left = postage.Right;

                    panelInfo.Enabled = false;
                }

                if (!keepTrying)
                {
                    Thread.Sleep(5000);

                    break;
                }
            }
        }

        /// <summary>
        /// Indicates if postage has been purchased for the account
        /// </summary>
        public bool PostagePurchased
        {
            get { return postagePurchased; }
        }

        /// <summary>
        /// Open the account settings page
        /// </summary>
        private void OnLinkAccountSettings(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                WebHelper.OpenUrl(StampsApiSession.GetUrl(account, UrlType.AccountSettingsPage), this);
            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Open the online reports page
        /// </summary>
        private void OnLinkOnlineReports(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                WebHelper.OpenUrl(StampsApiSession.GetUrl(account, UrlType.AccountSettingsPage), this);
            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Purchase more postage
        /// </summary>
        private void OnPurchasePostage(object sender, EventArgs e)
        {
            using (StampsPurchasePostageDlg dlg = new StampsPurchasePostageDlg(account, accountInfo))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Initialize(account);

                    postagePurchased = true;
                }
            }
        }
    }
}
