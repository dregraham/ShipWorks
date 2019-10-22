using System;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Properties;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens
{
    /// <summary>
    /// User control for getting and renewing ebay tokens
    /// </summary>
    public partial class EbayTokenCreateControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(LogManager));

        EbayStoreEntity store = null;
        EbayToken token;

        // Text to display when the token is imported
        string successText = "Your eBay Login Token has been imported!";

        /// <summary>
        /// Raised when a token has been succesfully created (or loaded from file) and imported into shipworks
        /// </summary>
        public event EventHandler TokenImported;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayTokenCreateControl()
        {
            InitializeComponent();
            token = new EbayToken();

            if (InterapptiveOnly.IsInterapptiveUser && !EbayUrlUtilities.UseLiveServer)
            {
                fakeToken.Visible = true;
                fakeTokenLabel.Visible = true;
                this.Size = new System.Drawing.Size(411, 60);
            }
        }

        [Category("Appearance")]
        [DefaultValue("Create Login Token...")]
        public string TokenButtonText
        {
            get { return createTokenButton.Text; }
            set { createTokenButton.Text = value; }
        }

        [Category("Appearance")]
        [DefaultValue("Your eBay Login Token has been imported!")]
        public string SuccessText
        {
            get { return successText; }
            set { successText = value; }
        }

        /// <summary>
        /// Initiate the eBay token creation process.
        /// </summary>
        private void OnCreateToken(object sender, EventArgs e)
        {
            WebHelper.OpenUrl(EbayUrlUtilities.GetTokenUrl(), this);

            statusPicture.Image = Resources.indiciator_green;
            statusText.Text = "Waiting for you to finish authorizing ShipWorks...";

            statusPicture.Visible = true;
            statusText.Visible = true;

            timer.Start();
        }

        /// <summary>
        /// Initialize the control for the given store
        /// </summary>
        public void InitializeForStore(EbayStoreEntity store)
        {
            // If this is the first time or a differnt store, reset our state
            if (this.store == null || this.store.StoreID != store.StoreID)
            {
                CancelWaiting();
            }

            this.store = store;
        }

        /// <summary>
        /// If the control is currently waiting on a token import, stop.  If not currently waiting, this does nothing.
        /// </summary>
        public void CancelWaiting()
        {
            timer.Stop();

            statusPicture.Visible = false;
            statusText.Visible = false;
        }

        /// <summary>
        /// The timer for waiting for an import has ticked
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
        {
            // If the user is on a different wizard page or store settings page forget it...
            if (!Visible || TopLevelControl == null)
            {
                return;
            }

            // Vista bug - this stops running on vista after a window obscures.
            statusPicture.Invalidate();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Try to load the authorization token for this instance of ShipWorks and 
                // update the store's eBay token properties accordingly. 
                EbayTangoTokenUtility tangoTokenUtility = new EbayTangoTokenUtility();
                token = tangoTokenUtility.GetTokenData();

                // Special case for freemium - can't change ebay users
                if (EditionSerializer.Restore(store) is FreemiumFreeEdition)
                {
                    if (store.EBayUserID.Length > 0 && store.EBayUserID != token.UserId)
                    {
                        throw new EbayException("You cannot switch to a different eBay user in the free edition of ShipWorks.");
                    }
                }

                store.EBayToken = token.Token;
                store.EBayUserID = token.UserId;
                store.EBayTokenExpire = token.ExpirationDate;

                timer.Stop();

                statusPicture.Image = Resources.check16;
                statusText.Text = successText;

                if (TokenImported != null)
                {
                    TokenImported(this, EventArgs.Empty);
                }
            }
            catch (EbayException ex)
            {
                log.InfoFormat("Poll for token failure: {0}", ex.Message);
            }
        }
    }
}
