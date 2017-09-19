using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// User control for getting and renewing pro stores tokens
    /// </summary>
    public partial class ProStoresTokenCreateControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(LogManager));

        ProStoresStoreEntity store = null;

        // Text to display when the token is imported
        string successText = "Your ProStores Token has been imported!";

        /// <summary>
        /// Raised when a token has been succesfully created (or loaded from file) and imported into shipworks
        /// </summary>
        public event EventHandler TokenImported;

        // The ticket pending import into a token
        string pendingTicket = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresTokenCreateControl()
        {
            InitializeComponent();
        }

        [Category("Appearance")]
        [DefaultValue("Create Login Token...")]
        public string TokenButtonText
        {
            get { return createTokenButton.Text; }
            set { createTokenButton.Text = value; }
        }

        [Category("Appearance")]
        [DefaultValue("Your ProStores Token has been imported!")]
        public string SuccessText
        {
            get { return successText; }
            set { successText = value; }
        }

        /// <summary>
        /// Initiate the ProStores token creation process.
        /// </summary>
        private void OnCreateToken(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var webClient = lifetimeScope.Resolve<IProStoresWebClient>();
                    string loginUrl = webClient.CreateApiLogonUrl(store, out pendingTicket);
                    WebHelper.OpenUrl(loginUrl, this);
                }
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    MessageHelper.ShowError(this, ex.Message);
                    return;
                }
                else
                {
                    throw;
                }
            }

            statusPicture.Image = Resources.indiciator_green;
            statusText.Text = "Waiting for you to finish authorizing ShipWorks...";

            statusPicture.Visible = true;
            statusText.Visible = true;

            timer.Start();
        }

        /// <summary>
        /// Initialize the control for the given store
        /// </summary>
        public void InitializeForStore(ProStoresStoreEntity store)
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
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var webClient = lifetimeScope.Resolve<IProStoresWebClient>();
                    XmlDocument response = webClient.GetTokenFromTicket(store, pendingTicket);
                    store.ApiToken = response.SelectSingleNode("//Token").InnerText;
                    store.Username = response.SelectSingleNode("//UserName").InnerText;
                }

                timer.Stop();

                statusPicture.Image = Resources.check16;
                statusText.Text = successText;

                if (TokenImported != null)
                {
                    TokenImported(this, EventArgs.Empty);
                }
            }
            catch (ProStoresException ex)
            {
                log.InfoFormat("Poll for token failure: {0}", ex.Message);
            }
        }
    }
}
