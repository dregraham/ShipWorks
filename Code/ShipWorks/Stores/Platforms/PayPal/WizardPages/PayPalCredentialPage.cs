using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using System.Text.RegularExpressions;
using ShipWorks.Stores.Management;
using log4net;

namespace ShipWorks.Stores.Platforms.PayPal.WizardPages
{
    /// <summary>
    /// Wizard page for gathering paypal credentials
    /// </summary>
    public partial class PayPalCredentialPage : AddStoreWizardPage
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PayPalCredentialPage));

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalCredentialPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            PayPalStoreEntity store = GetStore<PayPalStoreEntity>();
            PayPalAccountAdapter adapter = new PayPalAccountAdapter(store, "");

            // save user credentials
            if (!accountSettings.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            // make a request to get some store information
            PayPalWebClient client = new PayPalWebClient(adapter);
            PaymentTransactionType transaction = null;
            try
            {
                transaction = client.GetMostRecentTransaction();
            }
            catch (PayPalException ex)
            {
                log.Warn("Could not get PayPal most recent transaction.", ex);
            }

            if (transaction != null)
            {
                if (transaction.ReceiverInfo.Business.Length > 0)
                {
                    store.StoreName = transaction.ReceiverInfo.Business;
                    store.Company = transaction.ReceiverInfo.Business;
                }
                else
                {
                    store.StoreName = transaction.ReceiverInfo.Receiver;
                }
            }

            // if we didn't get anything from the transaction, just use credentials for the store name
            if (store.StoreName == null || store.StoreName.Length == 0)
            {
                if (adapter.CredentialType == PayPalCredentialType.Signature)
                {
                    string accountName = Regex.Replace(adapter.ApiUserName, @"_api\d+\.", "@", RegexOptions.IgnoreCase);

                    store.StoreName = "PayPal - " + accountName;
                }
                else
                {
                    store.StoreName = "PayPal";
                }
            }

        }

        /// <summary>
        /// The page is being navigated to, load defaults
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            accountSettings.LoadStore(((AddStoreWizard)Wizard).Store);
        }
    }
}
