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
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;
using ShipWorks.Stores;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Page for importing SSL certificates that couldn't be transferred during the upgrade
    /// </summary>
    public partial class ImportCertificatesWizardPage : WizardPage
    {
        // collection of stores that need to have their certificate/credentials fixed
        static List<StoreEntity> configurationMissing = null;

        // The Store this page is dealing with
        StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportCertificatesWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User is arriving here
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (store == null)
            {
                if (configurationMissing == null)
                {
                    LocateStoresMissingConfiguration();
                }

                // if there's nothing to configure, or nothing remaining to configure move on
                if (configurationMissing.Count == 0)
                {
                    e.Skip = true;
                    return;
                }
                else
                {
                    // This page will be the first missing store
                    store = configurationMissing[0];

                    // Add a page for each subsequent one
                    for (int i = 1; i < configurationMissing.Count; i++)
                    {
                        Wizard.Pages.Insert(Wizard.Pages.IndexOf(this) + 1, new ImportCertificatesWizardPage() { store = configurationMissing[i] });
                    }
                }
            }

            // prepare
            storeNameLabel.Text = String.Format("{0} Store: {1}", StoreTypeManager.GetType(store).StoreTypeName, store.StoreName);

            PayPalAccountAdapter accountAdapter = CreateAccountAdapter();
            payPalCredentials.LoadCredentials(accountAdapter);
        }

        /// <summary>        
        /// Constructs a store-appropriate PayPalAccountAdapter
        /// </summary>
        private PayPalAccountAdapter CreateAccountAdapter()
        {
            // prepare an adapter
            string prefix = "";

            // eBay stores' paypal fields are prefixed with PayPal
            if (store is EbayStoreEntity)
            {
                prefix = "PayPal";
            }
            else
            {
                // no prefixing for actual PayPal stores
            }

            PayPalAccountAdapter accountAdapter = new PayPalAccountAdapter(store, prefix);
            return accountAdapter;
        }


        /// <summary>
        /// Determines which stores need to be configured the rest of the way.
        /// </summary>
        private static void LocateStoresMissingConfiguration()
        {
            configurationMissing = new List<StoreEntity>();

            // find all stores that need a certificate: ebay, paypal, amazon
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = "SELECT StoreID FROM ebaystore where PayPalApiCredentialType = 1 AND PayPalApiCertificate IS NULL" +
                                      " UNION " +
                                      "SELECT StoreID FROM PayPalStore WHERE ApiCredentialType = 1 AND ApiCertificate IS NULL";

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            configurationMissing.Add(StoreManager.GetStore((long) reader[0]));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!skipCheckBox.Checked)
            {
                PayPalAccountAdapter adapter = CreateAccountAdapter();
                if (payPalCredentials.SaveCredentials(adapter))
                {
                    StoreManager.SaveStore(store);
                }
                else
                {
                    // stay on this page, an error would have been displayed already
                    e.NextPage = this;
                }
            }
        }

        /// <summary>
        /// Toggle the enabled/disabled state of the controls
        /// </summary>
        private void OnSkipChanged(object sender, EventArgs e)
        {
            payPalCredentials.Enabled = !skipCheckBox.Checked;
        }
    }
}