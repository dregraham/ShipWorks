﻿using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Window for purchasing more USPS postage
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class UspsPurchasePostageDlg : Form, IExpress1PurchasePostageDlg, IForm
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
        /// Load the account into the dialog
        /// </summary>
        public void LoadAccount(UspsAccountEntity account)
        {
            this.account = account;
        }

        /// <summary>
        /// Handle Shown event
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (account != null)
            {
                postageBalance = new PostageBalance(new UspsPostageWebClient(account));

                postageBalance.GetValueAsync().ContinueWith(x =>
                {
                    BeginInvoke((Action) (() => current.Text = StringUtility.FormatFriendlyCurrency(x.Result)));
                });

                // We have a valid USPS account, so we can use it to initialize the account info
                // and show the dialog
                current.Text = StringUtility.FormatFriendlyCurrency(GetBalance(account));
            }
        }

        /// <summary>
        /// Initializes the account info.
        /// </summary>
        /// <exception cref="UspsException">ShipWorks could not retrieve the account information from the carrier API.</exception>
        private decimal GetBalance(UspsAccountEntity uspsAccount)
        {
            // Define these here since they could be used in either inside or outside the try statement
            string carrierName = UspsAccountManager.GetResellerName((UspsResellerType) uspsAccount.UspsReseller);
            string exceptionMessage = string.Format("ShipWorks could not retrieve your account information from {0} at this time. Please try again later.", carrierName);

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
        /// information for the given USPS account entity provided.
        /// </summary>
        /// <exception cref="UspsException">ShipWorks could not find information for this account.</exception>
        public DialogResult ShowDialog(IWin32Window owner, long accountID)
        {
            account = UspsAccountManager.GetAccount(accountID);
            if (account == null)
            {
                // The account could have been deleted by another user/process
                throw new UspsException("ShipWorks could not find information for this account.");
            }

            return ShowDialog(owner);
        }

        /// <summary>
        /// Purchase the postage entered in the control
        /// </summary>
        private void OnPurchase(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string carrierName = UspsAccountManager.GetResellerName((UspsResellerType) account.UspsReseller);
            UspsShipmentType uspsShipmentType = PostalUtility.GetUspsShipmentTypeForUspsResellerType((UspsResellerType) account.UspsReseller);

            try
            {
                // Take this opportunity to update the contract type of the account
                uspsShipmentType.UpdateContractType(account);
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
                current.Text = StringUtility.FormatFriendlyCurrency(postageBalance.Value);
            }
            catch (UspsException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not retrieve the postage information for your account.\n\nError: " + ex.Message);

                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
