using System;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using log4net;

namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    /// <summary>
    /// Wizard page for configuring Volusion access
    /// </summary>
    public partial class VolusionAccountPage : VolusionAddStoreWizardPage
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionAccountPage));

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User is entering this page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (SetupState.Configuration == VolusionSetupConfiguration.Automatic)
            {
                configurationType.SelectedIndex = 0;
            }
            else
            {
                configurationType.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// User selected a different configuration type
        /// </summary>
        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            if (configurationType.SelectedIndex == 0)
            {
                SetupState.Configuration = VolusionSetupConfiguration.Automatic;
                UpdateConfigurationDisplay();
            }
            else
            {
                SetupState.Configuration = VolusionSetupConfiguration.Manual;
                UpdateConfigurationDisplay();
            }
        }

        /// <summary>
        /// Updates the display of configuration styles/information
        /// </summary>
        private void UpdateConfigurationDisplay()
        {
            bool isAutomatic = SetupState.Configuration == VolusionSetupConfiguration.Automatic;

            if (!isAutomatic)
            {
                // reposition the manual config panel
                manualConfigPanel.Top = autoConfigPanel.Top;
                manualConfigPanel.Left = autoConfigPanel.Left;
            }

            autoConfigPanel.Visible = isAutomatic;
            manualConfigPanel.Visible = !isAutomatic;
        }

        /// <summary>
        /// User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (urlTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your Volusion store url.");
                e.NextPage = this;
                return;
            }

            if (emailTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your Volusion login email address.");
                e.NextPage = this;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            // default timezone data
            TimeZoneInfo timeZone = TimeZoneInfo.Local;
            store.ServerTimeZone = timeZone.Id;

            store.StoreUrl = urlTextBox.Text;

            // Make sure it includes the protocol
            if (!store.StoreUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                store.StoreUrl = "http://" + store.StoreUrl;
            }

            store.WebUserName = emailTextBox.Text;

            // Automatic configuration
            if (SetupState.Configuration == VolusionSetupConfiguration.Automatic)
            {
                if (passwordTextBox.Text.Length == 0)
                {
                    MessageHelper.ShowError(this, "Please enter the password used to login to your Volusion store.");
                    e.NextPage = this;
                    return;
                }

                store.WebPassword = SecureText.Encrypt(passwordTextBox.Text, store.WebUserName);

                // attempt to auto-configure
                if (RunAutoConfigure())
                {
                    if (!SetupState.AutoDownloadedPaymentMethods || !SetupState.AutoDownloadedShippingMethods)
                    {
                        MessageHelper.ShowWarning(this,
                            "ShipWorks successfully connected to Volusion but was unable to download all of the necessary configuration.\n\n" +
                            "You will be prompted for this information on the next page.");
                    }
                }
                else
                {
                    // something really bad happened, an error would have been shown to the user already.  stay on page.
                    e.NextPage = this;
                    return;
                }
            }
            // Manual configuration
            else
            {
                if (apiPasswordTextBox.Text.Length == 0)
                {
                    MessageHelper.ShowError(this, "The encrypted password for your store is required.");
                    e.NextPage = this;
                    return;
                }

                store.ApiPassword = apiPasswordTextBox.Text;

                // validate the credentials
                VolusionWebClient client = new VolusionWebClient(store);
                if (!client.ValidateCredentials())
                {
                    MessageHelper.ShowError(this, "ShipWorks was unable to connect to Volusion using the provided settings.\n\nPlease check that you entered the Encrypted Password, and not your regular login password.");
                    e.NextPage = this;
                    return;
                }
            }
        }

        /// <summary>
        /// Goes through the auto-configuration process.
        /// </summary>
        private bool RunAutoConfigure()
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            try
            {
                log.Info("Starting Volusion AutoConfiguration");
                string webPassword = SecureText.Decrypt(store.WebPassword, store.WebUserName);

                VolusionWebSession session = new VolusionWebSession(store.StoreUrl);

                if (session.LogOn(store.WebUserName, webPassword))
                {
                    // attempt to get the Encrypted Api Password from the website
                    string encryptedPassword = session.RetrieveEncryptedPassword();

                    if (encryptedPassword.Length > 0)
                    {
                        // got the encrypted password
                        store.ApiPassword = encryptedPassword;

                        // test the encrypted password
                        VolusionWebClient webClient = new VolusionWebClient(store);
                        if (!webClient.ValidateCredentials())
                        {
                            MessageHelper.ShowError(this, "ShipWorks located the encrypted password for your Volusion store, but it appears to be invalid.");
                            return false;
                        }

                        // default to nothing having worked yet
                        SetupState.AutoDownloadedPaymentMethods = false;
                        SetupState.AutoDownloadedShippingMethods = false;

                        // now attempt to download Payment Method and Shipping Methods
                        DownloadStoreDetails(session);

                        return true;
                    }
                    else
                    {
                        // unable to locate encrypted password
                        MessageHelper.ShowError(this, "ShipWorks successfully logged in to your store's Admin Area, but was unable " +
                            "to determine your encrypted password.\n\nYou will need to setup your Volusion settings manually.");

                        SetupState.Configuration = VolusionSetupConfiguration.Manual;
                        UpdateConfigurationDisplay();
                        apiPasswordTextBox.Focus();

                        return false;
                    }
                }
                else
                {
                    // login failure
                    MessageHelper.ShowError(this,
                        "ShipWorks was unable to login to your store's admin area with the provided settings.\n\n" +
                        "Please verify your settings or try manual configuration.");

                    return false;
                }
            }
            catch (VolusionException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Tries to download both the Paymetn Method and Shipping Methods
        /// from the store's admin area web site.
        /// </summary>
        private void DownloadStoreDetails(VolusionWebSession session)
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            // try downloading shipping methods
            try
            {
                string shipmentMethods = session.RetrieveShippingMethods();
                if (shipmentMethods.Length > 0)
                {
                    VolusionShippingMethods methods = new VolusionShippingMethods(store);
                    methods.ImportCsv(shipmentMethods);

                    SetupState.AutoDownloadedShippingMethods = true;
                }
            }
            catch (VolusionException ex)
            {
                log.ErrorFormat("Unable to get Shipping Methods from the Volusion Store: {0}", ex.Message);
                store.ShipmentMethods = "";
            }

            // try downloading payment methods
            try
            {
                string paymentMethods = session.RetrievePaymentMethods();
                if (paymentMethods.Length > 0)
                {
                    VolusionPaymentMethods methods = new VolusionPaymentMethods(store);
                    methods.ImportCsv(paymentMethods);

                    SetupState.AutoDownloadedPaymentMethods = true;
                }
            }
            catch (VolusionException ex)
            {
                log.ErrorFormat("Unable to get Payment Methods from the Volusion Store: {0}", ex.Message);
                store.PaymentMethods = "";
            }
        }
    }
}
