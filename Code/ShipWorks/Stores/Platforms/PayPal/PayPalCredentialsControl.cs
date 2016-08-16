using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System.Web.Services.Protocols;
using ShipWorks.UI;
using System.Text.RegularExpressions;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using System.Security.Cryptography;
using log4net;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Control for a user to specify their PayPal credentials
    /// </summary>
    public partial class PayPalCredentialsControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PayPalCredentialsControl));

        ClientCertificate apiCertificate;

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalCredentialsControl()
        {
            InitializeComponent();

            linkHelp.Url = PayPalStoreType.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// Load the UI from the backing entity
        /// </summary>
        public void LoadCredentials(PayPalAccountAdapter account)
        {
            if (account.ApiCertificate != null)
            {
                try
                {
                    apiCertificate = new ClientCertificate();
                    apiCertificate.Import(account.ApiCertificate);
                }
                catch (CryptographicException ex)
                {
                    log.Error("Could not load PayPal certficate", ex);

                    apiCertificate = null;
                }
            }

            // load the remainder of the UI
            credentialType.SelectedIndex = (account.CredentialType == PayPalCredentialType.Signature ? 0 : 1);
            usernameTextBox.Text = account.ApiUserName;
            certificateTextBox.Text = (account.CredentialType == PayPalCredentialType.Certificate) ?  account.ApiUserName : "";
            signaturePasswordTextBox.Text = account.ApiPassword;
            certificatePasswordTextBox.Text = account.ApiPassword;
            signatureTextBox.Text = account.ApiSignature;
        }

        /// <summary>
        /// Save the UI values to the provided entity
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool SaveCredentials(PayPalAccountAdapter account)
        {
            // create a temporary entity to hold the values
            PayPalAccountAdapter tempAdapter = account.Clone();
            
            // populate the temp object with UI values 
            if (credentialType.SelectedIndex == 0)
            {
                tempAdapter.CredentialType = PayPalCredentialType.Signature;
                tempAdapter.ApiUserName = usernameTextBox.Text;
                tempAdapter.ApiPassword = signaturePasswordTextBox.Text;
                tempAdapter.ApiSignature = signatureTextBox.Text;
                tempAdapter.ApiCertificate = null;
            }
            else
            {
                tempAdapter.CredentialType = PayPalCredentialType.Certificate;
                tempAdapter.ApiUserName = certificateTextBox.Text;
                tempAdapter.ApiPassword = certificatePasswordTextBox.Text;
                tempAdapter.ApiCertificate = apiCertificate == null ? null : apiCertificate.Export();
                tempAdapter.ApiSignature = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            // validate UI values
            try
            {
                PayPalWebClient client = new PayPalWebClient(tempAdapter);
                client.ValidateCredentials();
            }
            catch (PayPalException ex)
            {
                MessageHelper.ShowError(this, "Unable to validate credentials.\n\n" + ex.Message);
                return false;
            }

            // success, so keep the temporary values
            account.CredentialType = tempAdapter.CredentialType;
            account.ApiUserName = tempAdapter.ApiUserName;
            account.ApiPassword = tempAdapter.ApiPassword;
            account.ApiSignature = tempAdapter.ApiSignature;
            account.ApiCertificate = tempAdapter.ApiCertificate;

            return true;
        }

        /// <summary>
        /// Api credential type was changed
        /// </summary>
        private void OnCredentialTypeChanged(object sender, EventArgs e)
        {
            UpdateCredentialsUI();
        }

        /// <summary>
        /// Displays/Hides data entry panels based on the credential type selected
        /// </summary>
        private void UpdateCredentialsUI()
        {
            bool signature = (credentialType.SelectedIndex == 0);

            panelSignature.Visible = signature;
            panelCertificate.Visible = !signature;
        }

        /// <summary>
        /// Import a new certificate
        /// </summary>
        private void OnImportClick(object sender, EventArgs e)
        {
            // load, and create a new apiCertificate object
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "PayPal API Certificate (*.txt)|*.txt";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // import the file
                        ClientCertificate newCertificate = new ClientCertificate();
                        newCertificate.LoadFromPemFile(dlg.FileName);

                        certificateTextBox.Text = newCertificate.Name;
                        apiCertificate = newCertificate;
                    }
                    catch (CryptographicException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                }
            }
        }
    }
}
