using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using System.Net;
using Interapptive.Shared.Net;
using log4net;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Wizard for creating a new shopify token
    /// </summary>
    public partial class ShopifyCreateTokenWizard : WizardForm
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyCreateTokenWizard));

        PortListener loginCallbackListener;

        string browserDisplayedShopName;
        string accessToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCreateTokenWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization of the wizard
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            loginCallbackListener = new PortListener();
            loginCallbackListener.StartListening();
            loginCallbackListener.BeginRequest += new EventHandler<PortListenerRequestBeginEventArgs>(OnInterceptLoginCallback);
        }

        /// <summary>
        /// The ShopName the user entered and validated - only valid if the DialogResult is OK.
        /// </summary>
        public string ShopUrlName
        {
            get
            {
                return shopUrlName.Text.Trim();
            }
        }

        /// <summary>
        /// The access token that has been generated.  Only valid if DialogResult is OK
        /// </summary>
        public string ShopAccessToken
        {
            get
            {
                return accessToken;
            }
        }

        /// <summary>
        /// Stepping next from the shop address page
        /// </summary>
        private void OnStepNextShopAddress(object sender, WizardStepEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(shopUrlName.Text))
            {
                MessageHelper.ShowMessage(this, "Please enter the name of your Shopify shop.");
                e.NextPage = CurrentPage;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // See if we get the login page or the invalid store page
            if (!ShopifyWebClient.IsRealShopifyShopUrlName(ShopUrlName))
            {
                MessageHelper.ShowMessage(this, "The shop name you entered does not appear to be valid.");
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping into the authenticate page
        /// </summary>
        private void OnSteppingIntoAuthenticatePage(object sender, WizardSteppingIntoEventArgs e)
        {
            NextEnabled = false;
        }

        /// <summary>
        /// The authentication page has been shown
        /// </summary>
        private void OnPageShownAuthenticatePage(object sender, WizardPageShownEventArgs e)
        {
            if (browserDisplayedShopName != ShopUrlName)
            {
                this.Cursor = Cursors.WaitCursor;

                webBrowser.Visible = false;
                browserDisplayedShopName = ShopUrlName;

                // Make sure we are logged out first, so the authentication process doesn't get completely skipped over and look goofy
                webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(OnBrowserLogoutCompleted);
                webBrowser.Navigate(new ShopifyEndpoints(ShopUrlName).ApiLogoutUrl);
            }
        }

        /// <summary>
        /// The document has completed loading
        /// </summary>
        private void OnBrowserLogoutCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            webBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(OnBrowserLogoutCompleted);

            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(OnBrowserDocumentCompleted);
            webBrowser.Navigate(new ShopifyEndpoints(ShopUrlName).GetApiAuthorizeUrl(loginCallbackListener.Port));
        }

        /// <summary>
        /// The document has completed loading
        /// </summary>
        private void OnBrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.Visible = true;
            this.Cursor = Cursors.Default;

            webBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(OnBrowserDocumentCompleted);
        }

        /// <summary>
        /// Intercept the callback after a succesful login
        /// </summary>
        private void OnInterceptLoginCallback(object sender, PortListenerRequestBeginEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { OnInterceptLoginCallback(sender, e); }));
                return;
            }

            if (e.RequestedUrl.PathAndQuery.Contains("access_denied"))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            try
            {
                // We've got the request token now, so go ask for the actual Access Token that will be persisted.
                accessToken = ShopifyWebClient.GetAccessToken(ShopUrlName, e.RequestedUrl);

                MoveNext();
            }
            catch (ShopifyException ex)
            {
                MessageHelper.ShowError(this, string.Format("ShipWorks was not authenticated to connect to Shopify:\n\n{0}", ex.Message));

                DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            webBrowser.Dispose();
            loginCallbackListener.Dispose();
        }
    }
}
