using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Window for purchasing more stamps.com postage
    /// </summary>
    public partial class UspsPurchasePostageDlg : Form, IExpress1PurchasePostageDlg
    {
        static readonly ILog log = LogManager.GetLogger(typeof(UspsPurchasePostageDlg));

        private UspsAccountEntity account;
        private PostageBalance postageBalance;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsPurchasePostageDlg"/> class.
        /// </summary>
        public UspsPurchasePostageDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsPurchasePostageDlg"/> class.
        /// </summary>
        public UspsPurchasePostageDlg(UspsAccountEntity account)
        {
            InitializeComponent();

            postageBalance = new PostageBalance(new UspsPostageWebClient(account), new TangoWebClientWrapper());
            
            current.Text = postageBalance.Value.ToString("c");
            this.account = account;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsPurchasePostageDlg"/> class.
        /// </summary>
        public UspsPurchasePostageDlg(UspsAccountEntity account, decimal balance)
        {
            InitializeComponent();

            postageBalance = new PostageBalance(new UspsPostageWebClient(account), new TangoWebClientWrapper());

            current.Text = balance.ToString("c");
            this.account = account;
        }

        /// <summary>
        /// Initializes the account info.
        /// </summary>
        /// <exception cref="UspsException">ShipWorks could not retrieve the account information from the carrier API.</exception>
        private decimal GetBalance(UspsAccountEntity uspsAccount)
        {
            // Define these here since they could be used in either inside or outside the try statement
            string carrierName = StampsAccountManager.GetResellerName((StampsResellerType)uspsAccount.UspsReseller); 
            string exceptionMessage = string.Format("ShipWorks could not retrieve your account information from {0} at this time. Please try again later.", carrierName);

            if (postageBalance == null)
            {
                postageBalance = new PostageBalance(new UspsPostageWebClient(uspsAccount), new TangoWebClientWrapper());
            }

            try
            {
                return postageBalance.Value;
            }
            catch (UspsException apiException)
            {
                log.Error(string.Format("ShipWorks could not retrieve account information from {0}. {1}", carrierName, apiException.Message), apiException);
                throw new UspsException(exceptionMessage, apiException);
            }
        }

        /// <summary>
        /// An implementation of the IExpress1PostageDialog interface. This will show the dialog using the
        /// information for the given Stamps account entity provided.
        /// </summary>
        /// <exception cref="UspsException">ShipWorks could not find information for this account.</exception>
        public DialogResult ShowDialog(IWin32Window owner, long accountID)
        {
            account = StampsAccountManager.GetAccount(accountID);
            if (account == null)
            {
                // The account could have been deleted by another user/process
                throw new UspsException("ShipWorks could not find information for this account.");
            }

            // We have a valid stamps account, so we can use it to initialize the account info
            // and show the dialog
            current.Text = GetBalance(account).ToString("c");
            return ShowDialog(owner);
        }

        /// <summary>
        /// Purchase the postage entered in the control
        /// </summary>
        private void OnPurchase(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string carrierName = StampsAccountManager.GetResellerName((StampsResellerType)account.UspsReseller);
            StampsShipmentType stampsShipmentType = PostalUtility.GetStampsShipmentTypeForStampsResellerType((StampsResellerType) account.UspsReseller);

            try
            {
                // Take this opportunity to update the contract type of the account
                stampsShipmentType.UpdateContractType(account);
                postageBalance.Purchase(postage.Amount);

                string message = string.Format("The purchase request has been submitted to {0}.\n\n" +
                                               "It may take a few minutes before the amount is reflected in your available balance.", carrierName);

                MessageHelper.ShowInformation(this, message);
                DialogResult = DialogResult.OK;
            }
            catch (UspsException ex)
            {
                string logMessage = string.Format("{0} purchase postage", carrierName);
                log.ErrorFormat(logMessage, ex);

                // Purchase "stale" (someone else has purchased in the meantime
                if (ex.Code == 0x00450116)
                {
                    MessageHelper.ShowWarning(this, "Another user has purchased postage for the account since the available postage displayed was retrieved.");

                    ReloadAccountInfo();
                }
                else
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Reload the account info due to stale postage
        /// </summary>
        private void ReloadAccountInfo()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                current.Text = postageBalance.Value.ToString("c");
            }
            catch (UspsException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not retrieve the postage information for your account.\n\nError: " + ex.Message);

                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
