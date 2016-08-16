using System;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Security;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using log4net;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Control for configuring account credentials for communication
    /// </summary>
    public partial class VolusionAccountSettingsControl : AccountSettingsControlBase
    {
        private static ILog log = LogManager.GetLogger(typeof (VolusionAccountSettingsControl));

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load store details into the UI from the entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            VolusionStoreEntity volusionStore = store as VolusionStoreEntity;
            if (volusionStore == null)
            {
                throw new InvalidOperationException("A non Volusion Store was passed to the Volusion Account Settings control");
            }

            urlTextBox.Text = volusionStore.StoreUrl;
            emailTextBox.Text = volusionStore.WebUserName;
            passwordTextBox.Text = SecureText.Decrypt(volusionStore.WebPassword, volusionStore.WebUserName);
            encryptedPasswordTextBox.Text = volusionStore.ApiPassword;
        }

        /// <summary>
        /// Save UI values to the backing entity
        /// </summary>
        [NDependIgnoreLongMethod]
        public override bool SaveToEntity(StoreEntity store)
        {
            VolusionStoreEntity volusionStore = store as VolusionStoreEntity;
            if (volusionStore == null)
            {
                throw new InvalidOperationException("A non Volusion Store was passed to the Volusion Account Settings control");
            }


            // validate everything
            //   Url, email, and encrypted password are all that's essential
            if (urlTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "The Url of your Volusion store is required.");
                return false;
            }

            if (emailTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "The email login to your Volusion store is required.");
                return false;
            }

            if (encryptedPasswordTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "The Encrypted Password for your store is required to download orders.");
                return false;
            }

            volusionStore.StoreUrl = urlTextBox.Text;
            volusionStore.WebUserName = emailTextBox.Text;
            volusionStore.ApiPassword = encryptedPasswordTextBox.Text;

            string webPassword = passwordTextBox.Text;
            volusionStore.WebPassword = SecureText.Encrypt(webPassword, volusionStore.WebUserName);

            // Make sure it includes the protocol
            if (!volusionStore.StoreUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                volusionStore.StoreUrl = "http://" + volusionStore.StoreUrl;
            }

            if (volusionStore.IsDirty)
            {
                Cursor.Current = Cursors.WaitCursor;

                // test url, username, api password with the web client
                VolusionWebClient client = new VolusionWebClient(volusionStore);

                if (!client.ValidateCredentials())
                {
                    MessageHelper.ShowError(this, "ShipWorks could not communicate with Volusion using the settings provided.");
                    return false;
                }

                if (webPassword.Length > 0)
                {
                    // test logging into the store as a web user
                    VolusionWebSession session = new VolusionWebSession(volusionStore.StoreUrl);

                    if (!session.LogOn(volusionStore.WebUserName, webPassword))
                    {
                        MessageHelper.ShowError(this, "ShipWorks was unable to logon to the Volusion website using the provided settings.");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Tries to auto-locate the user's Encrypted Password on the Volusion website
        /// </summary>
        private void OnReloadEncryptedPassword(object sender, EventArgs e)
        {
            // validate everything
            //   Url, email, and encrypted password are all that's essential
            if (urlTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "The Url of your Volusion store is required.");
                return;
            }

            if (emailTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "The email login to your Volusion store is required.");
                return;
            }

            // display an informational message if web password is left blank
            if (passwordTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "The website password for your store is required.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                VolusionWebSession webSession = new VolusionWebSession(urlTextBox.Text);
                if (webSession.LogOn(emailTextBox.Text, passwordTextBox.Text))
                {
                    string encryptedPassword = webSession.RetrieveEncryptedPassword();

                    if (encryptedPassword.Length > 0)
                    {
                        encryptedPasswordTextBox.Text = encryptedPassword;

                        MessageHelper.ShowInformation(this, "ShipWorks successfully located your Encrypted Password.");
                    }
                    else
                    {
                        MessageHelper.ShowError(this, "ShipWorks was able to login to your Volusion store but could not locate the Encrypted Password.");
                    }
                }
                else
                {
                    MessageHelper.ShowError(this, "ShipWorks was unable to login to your Volusion store with the provided settings.");
                }
            }
            catch (VolusionException ex)
            {
                // The RetrieveEncryptedPassword method could throw a VolusionException if the
                // password could not be found
                log.Error(ex);
                MessageHelper.ShowError(this, "ShipWorks could not locate the Encrypted Password for your Volusion store.");
            }
        }
    }
}
