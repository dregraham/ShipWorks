using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.Html;
using log4net;
using ShipWorks.Stores.Management;
using Interapptive.Shared.UI;
using System.IO;
using Interapptive.Shared.Utility;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShipWorks.Properties;
using System.Collections.Specialized;
using System.Web;
using ShipWorks.Stores.Platforms.Etsy.Enums;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Control to Manage Etsy Token
    /// </summary>
    public partial class EtsyTokenManageControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyTokenManageControl));

        EtsyWebClient webClient;
        EtsyStoreEntity store;

        bool showTokenInfo = true;

        /// <summary>
        /// Raised when a token has been succesfully created (or loaded from file) and imported into shipworks
        /// </summary>
        public event EventHandler TokenImported;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyTokenManageControl()
        {
            InitializeComponent();
        }

         /// <summary>
        /// Populates store and validates token
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            EtsyStoreEntity etsyStore = store as EtsyStoreEntity;
            if (etsyStore == null)
            {
                throw new ArgumentException("A non Etsy store was passed to EtsyTokenManageControl account settings.");
            }

            this.store = etsyStore;
            this.webClient = new EtsyWebClient(this.store);

            UpdateStatusDisplay();
        }

        /// <summary>
        /// The text to display on the create token button
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("Create Login Token...")]
        public string CreateTokenButtonText
        {
            get
            {
                return tokenButton.Text;
            }
            set
            {
                tokenButton.Text = value;
            }
        }


        /// <summary>
        /// Indicates if information about the imported token should be displayed
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowTokenInfo
        {
            get
            {
                return showTokenInfo;
            }
            set
            {
                showTokenInfo = value;

                UpdateStatusDisplay();
            }
        }

        /// <summary>
        /// Used to determine if the token is valid.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTokenValid 
        {
            get 
            {
                return store != null && !string.IsNullOrWhiteSpace(store.OAuthToken); 
            }
        }

        /// <summary>
        /// Prompts user to get a new token, retrieves it and authorizes it.
        /// </summary>
        private void OnAuthorizeShipWorks(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (HttpRedirectInterceptorDlg authenticateDlg = new HttpRedirectInterceptorDlg())
            {
                authenticateDlg.Text = "Authorize ShipWorks to Connect to Etsy";

                Uri callbackURL = authenticateDlg.StartListening();

                try
                {
                    authenticateDlg.InitialURL = webClient.GetRequestTokenURL(callbackURL);

                    if (authenticateDlg.ShowDialog(this) == DialogResult.OK)
                    {
                        ProcessListenerUrl(authenticateDlg.ResultURL);
                    }
                }
                catch (EtsyException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }   
            }
        }

        /// <summary>
        /// Process the URL handed back by Etsy after authentication and finalize  the authorizaion
        /// </summary>
        private void ProcessListenerUrl(Uri url)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                NameValueCollection queryStringCollection = HttpUtility.ParseQueryString(url.Query);

                string token = queryStringCollection["oauth_token"];
                string verifier = queryStringCollection["oauth_verifier"];

                webClient.AuthorizeToken(token, verifier);

                OnTokenImported();
            }
            catch (EtsyException ex)
            {
                log.Error(ex.Message, ex);
                MessageHelper.ShowError(this, ex.Message);
            }
        }

         /// <summary>
        /// Imports Token
        /// </summary>
        public void ImportToken()
        {
            EtsyTokenUtility tokenManager = new EtsyTokenUtility(store);

            if (tokenManager.ImportToken(this, webClient))
            {
                OnTokenImported();
            }
        }

        /// <summary>
        /// Display Valid Status
        /// </summary>
        private void OnTokenImported()
        {
            UpdateStatusDisplay();

            if (TokenImported != null)
            {
                TokenImported(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update the display of the information status
        /// </summary>
        private void UpdateStatusDisplay()
        {
            if (IsTokenValid && showTokenInfo)
            {
                statusText.Text = string.Format("Authorized to '{0}' with user '{1}'", store.EtsyStoreName, store.EtsyLoginName);
                statusPicture.Image = Resources.check16;

                panelStatus.Visible = true;
            }
            else
            {
                panelStatus.Visible = false;
            }
        }

        /// <summary>
        /// Exports a Token File
        /// </summary>
        public void ExportToken()
        {
            EtsyTokenUtility tokenManager = new EtsyTokenUtility(store);
            tokenManager.ExportToken(this);
        }
    }
}