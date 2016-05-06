using System;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Security;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.AmeriCommerce.WizardPages
{
    /// <summary>
    /// Add Store Wizard page for configuring connection information
    /// </summary>
    public partial class AmeriCommerceAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            AmeriCommerceStoreEntity store = GetStore<AmeriCommerceStoreEntity>();

            if (usernameTextBox.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "The AmeriCommerce username is required.");
                e.NextPage = this;
                return;
            }

            if (passwordTextBox.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "The AmeriCommerce password is required.");
                e.NextPage = this;
                return;
            }

            string url = urlTextBox.Text.Trim();
            if (url.Length == 0)
            {
                MessageHelper.ShowInformation(this, "The AmeriCommerce store url is required.");
                e.NextPage = this;
                return;
            }

            // default to https if scheme isn't specified
            if (url.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                url = "https://" + url;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                MessageHelper.ShowError(this, "The specified url is not a valid address.");
                e.NextPage = this;
                return;
            }

            if (!url.StartsWith("https"))
            {
                MessageHelper.ShowMessage(this, "The url must be secure (https).");
                e.NextPage = this;
                return;
            }

            // credentials
            store.Username = usernameTextBox.Text;
            store.Password = SecureText.Encrypt(passwordTextBox.Text, store.Username);
            store.StoreUrl = url;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // try to connect
                AmeriCommerceWebClient webClient = new AmeriCommerceWebClient(store);
                webClient.TestConnection();
            }
            catch (AmeriCommerceException ex)
            {
                MessageHelper.ShowError(this, string.Format("Unable to connect to the AmeriCommerce store: {0}", ex.Message));
                e.NextPage = this;
                return;
            }
        }
    }
}
