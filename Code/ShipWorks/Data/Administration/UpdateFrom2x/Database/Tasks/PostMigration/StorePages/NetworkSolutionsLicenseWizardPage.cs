using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Threading;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.NetworkSolutions;
using ShipWorks.Stores.Platforms.NetworkSolutions.WebServices;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    /// <summary>
    /// V3 uses the store URL as the license key, not the api key like version 2
    /// </summary>
    public partial class NetworkSolutionsLicenseWizardPage : WizardPage
    {
        // Stores to be configured
        List<NetworkSolutionsStoreEntity> netSolStores = new List<NetworkSolutionsStoreEntity>();

        // Ids for stores that errored out
        List<long> failedIds = new List<long>();

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsLicenseWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Page is being visited
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            failedIds.Clear();
            LoadUnconfiguredStores();

            panelUpdating.Visible = false;

            // if nothing is left to re-link, skip
            if (netSolStores.Count == 0)
            {
                e.Skip = true;
            }
        }

        /// <summary>
        /// Load the list of stores that still need there StoreURL configured
        /// </summary>
        private void LoadUnconfiguredStores()
        {
            netSolStores.Clear();

            // lookup the stores that need to be re-linked in tango and have their local store url populated correctly
            using (SqlAdapter adapter = new SqlAdapter())
            {
                using (NetworkSolutionsStoreCollection collection = new NetworkSolutionsStoreCollection())
                {
                    // the StoreUrl is set to blank in migration scripts
                    adapter.FetchEntityCollection(collection, new RelationPredicateBucket(NetworkSolutionsStoreFields.StoreUrl == ""));
                    netSolStores = new List<NetworkSolutionsStoreEntity>(collection);
                }
            }

            // Remove all the ones we know to have already failed
            netSolStores.RemoveAll(s => failedIds.Contains(s.StoreID));
        }

        /// <summary>
        /// Method meant to be called from an asycn invoker to update the database in the background
        /// </summary>
        private void AsyncPerformSteps(IProgressProvider progressProvider)
        {
            List<IProgressReporter> progressItems = new List<IProgressReporter>();

            // add all of the progress items
            foreach (NetworkSolutionsStoreEntity store in netSolStores)
            {
                progressItems.Add(progressProvider.AddItem(store.StoreName));
            }

            int i = 0;
            foreach (NetworkSolutionsStoreEntity store in netSolStores)
            {
                IProgressReporter currentProgress = progressItems[i];

                // do the work for this store
                UpdateLicenseData(store, currentProgress);

                i++;
            }
        }

        /// <summary>
        /// Updates the local Store Url to that returned by the NetSol API.  A call to Tango needs to be made to
        /// link this user's license key to the new license identifier
        /// </summary>
        private void UpdateLicenseData(NetworkSolutionsStoreEntity store, IProgressReporter progress)
        {
            // begin
            progress.Starting();

            NetworkSolutionsWebClient webClient = new NetworkSolutionsWebClient(store);

            try
            {
                progress.Detail = "Retrieving Store URL..";
                // retrieve the Store Url to be used as the license identifier
                SiteSettingType site = webClient.GetSiteSettings();
                store.StoreUrl = site.StoreUrl;

                if (string.IsNullOrEmpty(store.StoreUrl))
                {
                    throw new NetworkSolutionsException("ShipWorks was able to connect to your store, but the Store URL returned was empty. Contact Support.");
                }

                // save the store only after everything is updated in tango
                progress.PercentComplete = 66;
                progress.Detail = "Saving store details...";
                StoreManager.SaveStore(store);

                progress.PercentComplete = 100;
                progress.Detail = "Done";
                progress.Completed();
            }
            catch (NetworkSolutionsException ex)
            {
                failedIds.Add(store.StoreID);
                progress.Failed(ex);
            }
        }

        /// <summary>
        /// User is clicking Next to proceed
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            LoadUnconfiguredStores();
            if (netSolStores.Count == 0)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the upgrading message up and dislabe and the browsing buttons
            panelUpdating.Visible = true;
            panelUpdating.BringToFront();
            Wizard.NextEnabled = false;
            Wizard.BackEnabled = false;
            Wizard.CanCancel = false;

            // Stay on this page
            e.NextPage = this;

            // Create the progress provider and window
            ProgressProvider progressProvider = new ProgressProvider();
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Updating Licensing Details";
            progressDlg.Description = "ShipWorks is updating your Network Solutions store.";
            progressDlg.Show(this);

            // Used for async invoke
            MethodInvoker<IProgressProvider> invoker = new MethodInvoker<IProgressProvider>(AsyncPerformSteps);

            // Pass along user state
            Dictionary<string, object> userState = new Dictionary<string, object>();
            userState["invoker"] = invoker;
            userState["progressDlg"] = progressDlg;

            // Kick off the async upgrade process
            invoker.BeginInvoke(progressDlg.ProgressProvider, new AsyncCallback(OnAsyncUpdateComplete), userState);
        }

        /// <summary>
        /// The finalization steps have completed
        /// </summary>
        private void OnAsyncUpdateComplete(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new AsyncCallback(OnAsyncUpdateComplete), result);
                return;
            }

            Dictionary<string, object> userState = (Dictionary<string, object>) result.AsyncState;
            MethodInvoker<ProgressProvider> invoker = (MethodInvoker<ProgressProvider>) userState["invoker"];
            ProgressDlg progressDlg = (ProgressDlg) userState["progressDlg"];

            // complete
            invoker.EndInvoke(result);

            // go to the next page
            progressDlg.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                Wizard.MoveNext();
                progressDlg.Dispose();
            };
        }
    }
}
