using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using log4net;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Window for buying endicia postage
    /// </summary>
    public partial class EndiciaBuyPostageDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EndiciaBuyPostageDlg));

        EndiciaAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaBuyPostageDlg(EndiciaAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
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
                EndiciaAccountStatus status = EndiciaApiClient.GetAccountStatus(account);

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
                EndiciaApiClient.BuyPostage(account, postage.Amount);

                MessageHelper.ShowInformation(this,
                    String.Format("The purchase request has been submitted to {0}.", EndiciaAccountManager.GetResellerName((EndiciaReseller)account.EndiciaReseller)));

                DialogResult = DialogResult.OK;
            }
            catch (EndiciaException ex)
            {
                log.Error("Endiciapurchase postage", ex);

                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
