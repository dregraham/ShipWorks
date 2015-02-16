using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using ShipWorks.UI;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using System.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// User control for displaying account info for a stamps.com acount
    /// </summary>
    public partial class StampsAccountInfoControl : UserControl
    {
        UspsAccountEntity account;
        private PostageBalance postageBalance;
        private decimal? balance;

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
        public void Initialize(UspsAccountEntity account)
        {
            Cursor.Current = Cursors.WaitCursor;

            this.account = account;
            this.accountName.Text = account.Description;
            postageBalance = new PostageBalance(new UspsPostageWebClient(account), new TangoWebClientWrapper());

            contractType.Text = EnumHelper.GetDescription((UspsAccountContractType)account.ContractType);

            bool isExpress1 = account.UspsReseller == (int)StampsResellerType.Express1;
            
            if (isExpress1)
            {
                // Hide the links specific to Stamps.com if this is an Express1 account
                labelStampsWebsite.Visible = false;
                accountSettingsLink.Visible = false;
                onlineReportsLink.Visible = false;

                // Adjust the size of the control, so the control hosting this control doesn't have 
                // a ton of empty space
                panelInfo.Height = purchase.Bottom + 4;
                this.Height = panelInfo.Bottom + 4;
            }

            int tries = 5;

            while (tries-- > 0)
            {
                try
                {
                    balance = postageBalance.Value;
                    postage.Text = balance.Value.ToString("c");

                    purchase.Left = postage.Right;
                    panelInfo.Enabled = true;

                    break;
                }
                catch (StampsException ex)
                {
                    bool keepTrying = false;
                    string message = ex.Message;

                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        message = string.Format("Your {0} account is not ready yet.", StampsAccountManager.GetResellerName((StampsResellerType)account.UspsReseller));
                        keepTrying = true;
                    }

                    accountName.Text = message;
                    postage.Text = "$0.00";
                    purchase.Left = postage.Right;

                    panelInfo.Enabled = false;
                    
                    if (keepTrying)
                    {
                        // Sleep for a few seconds to allow the registration time to go through
                        Thread.Sleep(3000);
                    }
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
                WebHelper.OpenUrl(new StampsWebClient((StampsResellerType)account.UspsReseller).GetUrl(account, UrlType.AccountSettingsPage), this);
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
                WebHelper.OpenUrl(new StampsWebClient((StampsResellerType)account.UspsReseller).GetUrl(account, UrlType.AccountSettingsPage), this);
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
            try
            {
                using (StampsPurchasePostageDlg dlg = balance.HasValue ? new StampsPurchasePostageDlg(account, balance.Value) : new StampsPurchasePostageDlg(account))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Initialize(account);

                        postagePurchased = true;
                    }
                }
            }
            catch (StampsException stampsException)
            {
                MessageHelper.ShowError(this, stampsException.Message);
            }
        }
    }
}
