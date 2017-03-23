using System;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// User control for displaying account info for a USPS acount
    /// </summary>
    public partial class UspsAccountInfoControl : UserControl
    {
        private UspsAccountEntity account;
        private decimal? balance;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsAccountInfoControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public void Initialize(UspsAccountEntity uspsAccount)
        {
            Cursor.Current = Cursors.WaitCursor;

            account = uspsAccount;
            accountName.Text = uspsAccount.Description;

            contractType.Text = EnumHelper.GetDescription((UspsAccountContractType) uspsAccount.ContractType);

            HideUspsControlsIfExpress1();

            UpdatePostageBalance();
        }

        /// <summary>
        /// Indicates if postage has been purchased for the account
        /// </summary>
        public bool PostagePurchased { get; private set; }

        /// <summary>
        /// Open the account settings page
        /// </summary>
        private void OnAccountSettingsLinkClicked(object sender, EventArgs e) => OpenUrl(UrlType.AccountSettingsPage);

        /// <summary>
        /// Open the online reports page
        /// </summary>
        private void OnOnlineReportsLinkClicked(object sender, EventArgs e) => OpenUrl(UrlType.OnlineReportsPage);

        /// <summary>
        /// Opens the OnlineReportingHistory page
        /// </summary>
        private void OnShipmentHistoryLinkClicked(object sender, EventArgs e) => OpenUrl(UrlType.OnlineReportingHistory);

        /// <summary>
        /// Retrieves URL from UPS and opens it in a browser
        /// </summary>
        private void OpenUrl(UrlType urlType)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                WebHelper.OpenUrl(new UspsWebClient(IoC.UnsafeGlobalLifetimeScope, (UspsResellerType) account.UspsReseller).GetUrl(account, urlType), this);
            }
            catch (UspsException ex)
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
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    UspsPurchasePostageDlg dialog = lifetimeScope.Resolve<UspsPurchasePostageDlg>();
                    dialog.LoadAccount(account);

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        Initialize(account);

                        PostagePurchased = true;
                    }
                }
            }
            catch (UspsException uspsException)
            {
                MessageHelper.ShowError(this, uspsException.Message);
            }
        }

        /// <summary>
        /// Update the postage balance of the account
        /// </summary>
        private void UpdatePostageBalance()
        {
            int tries = 5;

            while (tries-- > 0)
            {
                try
                {
                    balance = new PostageBalance(new UspsPostageWebClient(account)).Value;
                    postage.Text = balance.Value.FormatFriendlyCurrency();

                    purchase.Left = postage.Right;
                    panelInfo.Enabled = true;

                    break;
                }
                catch (UspsException ex)
                {
                    bool keepTrying = false;
                    string message = ex.Message;

                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        message = $"Your {UspsAccountManager.GetResellerName((UspsResellerType) account.UspsReseller)} account is not ready yet.";
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
        /// Hide the Usps controls if the account is Express 1
        /// </summary>
        private void HideUspsControlsIfExpress1()
        {
            bool isExpress1 = account.UspsReseller == (int) UspsResellerType.Express1;

            if (!isExpress1)
            {
                return;
            }

            // Hide the links specific to USPS if this is an Express1 account
            labelStampsWebsite.Visible = false;
            accountSettingsLink.Visible = false;
            onlineReportsLink.Visible = false;

            // Adjust the size of the control, so the control hosting this control doesn't have
            // a ton of empty space
            panelInfo.Height = purchase.Bottom + 4;
            Height = panelInfo.Bottom + 4;
        }
    }
}
