using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// UI for allowing users to import new credentials/certificates for Amazon Web Services
    /// </summary>
    public partial class AmazonCertificateImportForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCertificateImportForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor for specifying starting values
        /// </summary>
        public AmazonCertificateImportForm(string accessKeyID)
            : this()
        {
            importControl.AccessKeyID = accessKeyID;
        }

        /// <summary>
        /// Gets the Amazon Access Key ID.  Only valid if DialogResult is OK
        /// </summary>
        public string AccessKeyID
        {
            get 
            {
                if (DialogResult == DialogResult.OK)
                {
                    return importControl.AccessKeyID;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// When DialogResult is OK, contains the imported, validated client certificate
        /// </summary>
        public ClientCertificate ClientCertificate
        {
            get 
            {
                if (DialogResult == DialogResult.OK)
                {
                    return importControl.ClientCertificate;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Perform the certificate import
        /// </summary>
        private void OnImportClick(object sender, EventArgs e)
        {
            if (importControl.ValidateCredentials())
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Cancel the import
        /// </summary>
        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}