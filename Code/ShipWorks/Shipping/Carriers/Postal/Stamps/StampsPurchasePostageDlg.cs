using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using ShipWorks.UI;
using log4net;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Window for purchasing more stamps.com postage
    /// </summary>
    public partial class StampsPurchasePostageDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(StampsPurchasePostageDlg));

        StampsAccountEntity account;
        AccountInfo accountInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsPurchasePostageDlg(StampsAccountEntity account, AccountInfo accountInfo)
        {
            InitializeComponent();

            this.account = account;
            this.accountInfo = accountInfo;

            current.Text = accountInfo.PostageBalance.AvailablePostage.ToString("c");
        }

        /// <summary>
        /// Purchase the postage entered in the control
        /// </summary>
        private void OnPurchase(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                StampsApiSession.PurchasePostage(account, postage.Amount, accountInfo.PostageBalance.ControlTotal);

                MessageHelper.ShowInformation(this, 
                    "The purchase request has been submitted to Stamps.com.\n\n" +
                    "It may take a few minutes before the amount is reflected in your available balance.");

                DialogResult = DialogResult.OK;
            }
            catch (StampsException ex)
            {
                log.Error("Stamps.com purchase postage", ex);

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
                accountInfo = StampsApiSession.GetAccountInfo(account);

                current.Text = accountInfo.PostageBalance.AvailablePostage.ToString("c");
            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not retrieve the postage information for your account.\n\nError: " + ex.Message);

                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
