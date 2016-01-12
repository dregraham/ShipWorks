using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using ShipWorks.UI;
using Interapptive.Shared.UI;
using System.Security.Cryptography;
using Interapptive.Shared;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Reusable control for importing the Amazon certificate
    /// </summary>
    public partial class AmazonCertificateImportControl : UserControl
    {
        ClientCertificate clientCertificate = new ClientCertificate();

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCertificateImportControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The imported client certificate
        /// </summary>
        public ClientCertificate ClientCertificate
        {
            get { return clientCertificate; }
        }

        /// <summary>
        /// The user entered access key
        /// </summary>
        public string AccessKeyID
        {
            get { return accessKey.Text.Trim(); }
            set { accessKey.Text = value; }
        }

        /// <summary>
        /// Open the Amazon Web Services link in the form
        /// </summary>
        private void OnAwsLinkClicked(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://aws-portal.amazon.com/gp/aws/developer/account/index.html?action=access-key", this);
        }

        /// <summary>
        /// Perform a load of the certificates
        /// </summary>
        [NDependIgnoreLongMethod]
        public bool ValidateCredentials()
        {
            if (accessKey.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter the Access Key ID.");

                return false;
            }

            if (publicCertificateFile.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter the X.509 Certificate.");

                return false;
            }

            if (privateKeyFile.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter the Private Key.");

                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Load the certificate
            try
            {
                clientCertificate.LoadFromPemFiles(publicCertificateFile.Text, privateKeyFile.Text);
            }
            catch (FileNotFoundException ex)
            {
                MessageHelper.ShowError(this, "The certificate file could not be found:\n\n" + ex.FileName);

                return false;
            }
            catch (IOException ex)
            {
                MessageHelper.ShowError(this, "The certificate file could not be read:\n\n" + ex.Message);

                return false;
            }
            catch (CryptographicException ex)
            {
                MessageHelper.ShowError(this, "The certificate could not be imported:\n\nError: " + ex.Message);

                return false;
            }

            if (clientCertificate.X509Certificate == null)
            {
                MessageHelper.ShowError(this, "ShipWorks was unable to import the certificate files.");

                return false;
            }

            // test the certificate with Amazon
            try
            {
                AmazonEnsClient.TestConnection(clientCertificate);
            }
            catch (AmazonException ex)
            {
                MessageHelper.ShowError(this, "Unable to authenticate with the certificate provided:\n\nMessage: " + ex.Message);

                return false;
            }

            // test the access key id
            try
            {
                AmazonAssociatesWebClient.TestConnection(accessKey.Text.Trim(), clientCertificate);
            }
            catch (AmazonException ex)
            {
                MessageHelper.ShowError(this, "Unable to connect to Amazon with the Access Key ID and certificate provided.:\n\nMessage: " + ex.Message);

                return false;
            }

            Cursor.Current = Cursors.Default;

            return true;
        }

        /// <summary>
        /// Browse for the public certificate
        /// </summary>
        private void OnBrowseCertClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                publicCertificateFile.Text = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Browse for the private key file
        /// </summary>
        private void OnBrowseKeyClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                privateKeyFile.Text = openFileDialog.FileName;
            }
        }
    }
}
