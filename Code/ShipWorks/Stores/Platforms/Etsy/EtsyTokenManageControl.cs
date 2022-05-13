using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.UI.Controls.Html;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Control to Manage Etsy Token
    /// </summary>
    public partial class EtsyTokenManageControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyTokenManageControl));

        IEtsyWebClient webClient;
        EtsyStoreEntity store;

        bool showTokenInfo = true;

        /// <summary>
        /// Raised when a token has been successfully created (or loaded from file) and imported into shipworks
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
            this.webClient = IoC.UnsafeGlobalLifetimeScope.Resolve<IEtsyWebClient>(TypedParameter.From(etsyStore));

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
        /// Used to determine if a new token has been entered
        /// </summary>
        public bool HasUserUpdatedToken => !string.IsNullOrEmpty(tokenInput.Text);

        /// <summary>
        /// Prompts user to get a new token, retrieves it and authorizes it.
        /// </summary>
        private void OnAuthorizeShipWorks(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                var warehouseUrl = new Uri(scope.Resolve<WebClientEnvironmentFactory>().SelectedEnvironment.WarehouseUrl);
                
                var callbackURL = new Uri(warehouseUrl, "callbacks/etsy/auth");
                var authLink = webClient.GetRequestTokenURL(callbackURL);
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = authLink.ToString()
                };
                System.Diagnostics.Process.Start(psi);
            }    
        }

        /// <summary>
        /// Verify the verification token with Etsy and get the actual OAuthToken
        /// </summary>
        public void VerifyToken()
        {
            try
            {
                if(tokenInput.Text.Length == 0)
                {
                    return;
                }

                var parts = tokenInput.Text.Split('-');
                if (parts.Length != 2)
                {
                    MessageHelper.ShowError(this, "Token was incorrectly formatted. Please attempt to fetch another token from Etsy.");
                    return;
                }
                var token = parts[0];
                var verifier = parts[1];
                webClient.AuthorizeToken(token, verifier);
            }
            catch (Exception ex)
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

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            webClient = null;

            base.Dispose(disposing);
        }
    }
}