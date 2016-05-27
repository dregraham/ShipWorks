using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;


namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Window for buying endicia postage
    /// </summary>
    public partial class EndiciaBuyPostageDlg : Form, IExpress1PurchasePostageDlg
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EndiciaBuyPostageDlg));

        EndiciaAccountEntity account;

        private readonly bool purchaseRestricted;
        private readonly ITangoWebClient tangoWebClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaBuyPostageDlg"/> class.
        /// </summary>
        /// <param name="account">The account.</param>
        public EndiciaBuyPostageDlg(ILicenseService licenseService, ITangoWebClient tangoWebClient)
        {
            this.tangoWebClient = tangoWebClient;

            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.PurchasePostage, ShipmentTypeCode.Endicia);

            // If purchasing is restricted for Endicia, set the variable
            if (!IsExpress1() && restrictionLevel == EditionRestrictionLevel.Forbidden)
            {
                purchaseRestricted = true;
            }

            InitializeComponent();
        }

        /// <summary>
        /// Load the account
        /// </summary>
        public void LoadAccount(EndiciaAccountEntity account)
        {
            this.account = account;
        }

        /// <summary>
        /// Determine if this is for express1
        /// </summary>
        /// <returns>False if account is null or account is not Express1.  True otherwise.</returns>
        private bool IsExpress1()
        {
            return account != null && (EndiciaReseller)account.EndiciaReseller == EndiciaReseller.Express1;
        }

        /// <summary>
        /// The window has been shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Refresh();

            // Show any purchase nudges if there are any
            ShowPurchasePostageNudges();

            // Close out if purchasing is restricted.
            if (purchaseRestricted)
            {
                DialogResult = DialogResult.None;
                Close();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                current.Text = (new PostageBalance(new EndiciaPostageWebClient(account), tangoWebClient)).Value.ToString("c");
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
                (new PostageBalance(new EndiciaPostageWebClient(account), tangoWebClient)).Purchase(postage.Amount);

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
        /// This will show the dialog using the information for the given Endicia account entity provided.
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

            // We have a valid endicia account, so we can use it to initialize the account info and show the dialog
            return ShowDialog(owner, endiciaAccountEntity);
        }

        /// <summary>
        /// This will show the dialog using the information for the given Endicia account entity provided.
        /// </summary>
        public DialogResult ShowDialog(IWin32Window owner, EndiciaAccountEntity account)
        {
            this.account = account;
            return ShowDialog(owner);
        }

        /// <summary>
        /// Checks for any purchase postage nudges
        /// </summary>
        private void ShowPurchasePostageNudges()
        {
            // If there is an Endicia shipment in the list, check for PurchasesEndicia nudges
            Nudge nudge = NudgeManager.GetFirstNudgeOfType(NudgeType.PurchaseEndicia);
            if (nudge != null)
            {
                NudgeManager.ShowNudge(this, nudge);
            }
            else if (purchaseRestricted)
            {
                MessageHelper.ShowError(this, string.Format("ShipWorks can no longer purchase postage for Endicia."));
            }
        }
    }
}
