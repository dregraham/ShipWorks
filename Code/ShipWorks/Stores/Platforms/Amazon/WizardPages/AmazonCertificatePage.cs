using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    /// <summary>
    /// Amazon Certificate Wizard Page
    /// </summary>
    public partial class AmazonCertificatePage : AddStoreWizardPage
    {
        public AmazonCertificatePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping to the next wizard page, perform validation and work here
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            AmazonStoreEntity amazonStore = GetStore<AmazonStoreEntity>();

            if (!importControl.ValidateCredentials())
            {
                // stay on the current page
                e.NextPage = this;
                return;
            }
            else
            {
                amazonStore.Certificate = importControl.ClientCertificate.Export();
                amazonStore.AccessKeyID = importControl.AccessKeyID;
            }
        }
    }
}
