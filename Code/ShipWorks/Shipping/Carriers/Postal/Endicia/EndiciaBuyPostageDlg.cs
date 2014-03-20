using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.UI;
using log4net;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Window for buying endicia postage
    /// </summary>
    public partial class EndiciaBuyPostageDlg : Form, IExpress1PurchasePostageDlg
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EndiciaBuyPostageDlg));

        EndiciaAccountEntity account;
        private readonly EndiciaApiClient endiciaApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaBuyPostageDlg"/> class.
        /// </summary>
        public EndiciaBuyPostageDlg()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaBuyPostageDlg"/> class.
        /// </summary>
        /// <param name="account">The account.</param>
        public EndiciaBuyPostageDlg(EndiciaAccountEntity account)
        {
            InitializeComponent();
            this.account = account;
            endiciaApiClient = new EndiciaApiClient();
        }

        /// <summary>
        /// The window has been shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Refresh();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                EndiciaAccountStatus status = endiciaApiClient.GetAccountStatus(account);

                current.Text = status.PostageBalance.ToString("c");
            }
            catch (EndiciaException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not load the balance of the account.\n\nError: " + ex.Message);

                current.Text = "Error";
                postage.Enabled = false;
                purchase.Enabled = false;
            }
        }

        /// <summary>
        /// Purchase postage
        /// </summary>
        private void OnPurchase(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                endiciaApiClient.BuyPostage(account, postage.Amount);

                MessageHelper.ShowInformation(this,
                    String.Format("The purchase request has been submitted to {0}.", EndiciaAccountManager.GetResellerName((EndiciaReseller)account.EndiciaReseller)));

                DialogResult = DialogResult.OK;
            }
            catch (EndiciaException ex)
            {
                log.Error("Endicia purchase postage", ex);

                MessageHelper.ShowError(this, ex.Message);
            }
        }        

        /// <summary>
        /// This will show the dialog using the information for the given Stamps account entity provided.
        /// </summary>
        /// <exception cref="EndiciaException">ShipWorks could not find information for this account.</exception>
        public DialogResult ShowDialog(IWin32Window owner, long accountID)
        {
            EndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccount(accountID);
            if (endiciaAccountEntity == null)
            {
                // The account could have been deleted by another user/process
                throw new EndiciaException("ShipWorks could not find information for this account.");
            }

            // We have a valid endicia account, so we can use it to initialize the account info
            // and show the dialog
            // InitializeAccountInfo(endiciaAccountEntity);
            this.account = endiciaAccountEntity;
            return ShowDialog(owner);
        }
    }
}
