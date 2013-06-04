using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Properties;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// User control for getting ebay tokens
    /// </summary>
    public partial class NetworkSolutionsCreateTokenControl : UserControl
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(NetworkSolutionsCreateTokenControl));

        NetworkSolutionsStoreEntity store = null;

        /// <summary>
        /// The key returned by NetSol as part of the authorization process
        /// </summary>
        string userKey = "";

        /// <summary>
        /// Message to show when the import is complete
        /// </summary>
        string successText = "Your Network Solutions Token has been imported!";

        [Category("Appearance")]
        [DefaultValue("Create Login Token...")]
        public string TokenButtonText
        {
            get { return createTokenButton.Text; }
            set { createTokenButton.Text = value; }
        }

        [Category("Appearance")]
        [DefaultValue("Your Network Solutions Token has been imported!")]
        public string SuccessText
        {
            get { return successText; }
            set { successText = value; }
        }

        /// <summary>
        /// Raised when a token has been succesfully created and imported into ShipWorks
        /// </summary>
        public event EventHandler TokenImported;
						  
        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsCreateTokenControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a new user token 
        /// </summary>
        private void OnCreateToken(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            NetworkSolutionsWebClient webClient = new NetworkSolutionsWebClient(store);
            try
            {
                NetworkSolutionsUserKey netSolKey = webClient.FetchUserKey();

                // the GetUserKey call provides a url to redirect the user to where they
                // sign in and grant access to ShipWorks.  We then poll for their UserToken
                WebHelper.OpenUrl(netSolKey.LoginUrl, this);

                userKey = netSolKey.UserKey;

                // start polling for the User Token
                statusPicture.Image = Resources.indiciator_green;
                statusText.Text = "Waiting for you to finish authorizing ShipWorks...";

                statusPicture.Visible = true;
                statusText.Visible = true;

                timer.Start();
            }
            catch (NetworkSolutionsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Initialize the control for the given store
        /// </summary>
        public void InitializeForStore(NetworkSolutionsStoreEntity store)
        {
            if (this.store == null || this.store.StoreID != store.StoreID)
            {
                CancelWaiting();
            }

            this.store = store;
        }

        /// <summary>
        /// If the control is currently wiating on a token import, stop.  If not currently wiating, do nothing
        /// </summary>
        public void CancelWaiting()
        {
            timer.Stop();

            statusPicture.Visible = false;
            statusText.Visible = false;
        }

        /// <summary>
        /// Timer tick for polling for the user token
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
        {
            // if the user is on a different wizard page or store settings page, forget it...
            if (!Visible || TopLevelControl == null)
            {
                return;
            }

            // Vista bug - this stops running on vista after a window obscures
            statusPicture.Invalidate();

            try
            {
                NetworkSolutionsWebClient client = new NetworkSolutionsWebClient(store);

                string userToken = client.FetchUserToken(userKey);
                store.UserToken = userToken;

                statusPicture.Image = Resources.check16;
                statusText.Text = successText;

                if (TokenImported != null)
                {
                    TokenImported(this, EventArgs.Empty);
                }
            }
            catch(NetworkSolutionsException ex)
            {
                log.InfoFormat("Poll for token failure: {0}", ex.Message);
            }
        }
    }
}
