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
    /// Page for entering Amazon user credentials
    /// </summary>
    public partial class AmazonCredentialsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCredentialsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User is navigating to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            AmazonStoreEntity store = GetStore<AmazonStoreEntity>();

            Cursor.Current = Cursors.WaitCursor;
            if (!accountSettings.SaveToEntity(store))
            {
                // there was an error, stay on this page
                e.NextPage = this;
                return;
            }

            // set the store name here
            store.StoreName = store.MerchantName;

            // this store is using the old APIs
            store.AmazonApi = (int)AmazonApi.LegacySoap;
        }
    }
}
