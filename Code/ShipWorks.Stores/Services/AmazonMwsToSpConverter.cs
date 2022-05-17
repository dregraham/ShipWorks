using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Logging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon.DTO;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Class to convert Amazon MWS stores to SP
    /// </summary>
    [Component]
    public class AmazonMwsToSpConverter : IAmazonMwsToSpConverter
    {
        private readonly IStoreManager storeManager;
        private readonly IWarehouseRequestFactory requestFactory;
        private readonly IWarehouseRequestClient warehouseClient;
        private readonly IDownloadStartingPoint downloadStartingPoint;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsToSpConverter(IStoreManager storeManager,
            IWarehouseRequestFactory requestFactory,
            IWarehouseRequestClient warehouseClient,
            IDownloadStartingPoint downloadStartingPoint,
            Func<Type, ILog> logFactory)
        {
            this.storeManager = storeManager;
            this.requestFactory = requestFactory;
            this.warehouseClient = warehouseClient;
            this.downloadStartingPoint = downloadStartingPoint;
            log = logFactory(typeof(AmazonMwsToSpConverter));
        }

        /// <summary>
        /// Convert Amazon MWS stores to SP
        /// </summary>
        public void ConvertStores(IWin32Window owner)
        {
            log.Info("Beginning migration of Amazon MWS stores to SP");

            var storesToMigrate = storeManager.GetAllStores().Where(x => x.StoreTypeCode == StoreTypeCode.Amazon &&
                x.OrderSourceID.IsNullOrWhiteSpace() &&
                x.WarehouseStoreID == null)
                .Cast<AmazonStoreEntity>();

            if (storesToMigrate.Any())
            {
                ProgressProvider progressProvider = new ProgressProvider();
                IProgressReporter storeProgress = progressProvider.AddItem("Migrating Amazon Stores");
                storeProgress.CanCancel = false;
                using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
                {
                    progressDialog.Title = "Migrating Amazon MWS Stores to Amazon SP";
                    progressDialog.AllowCloseWhenRunning = false;
                    progressDialog.AutoCloseWhenComplete = true;

                    Task.Run(async () =>
                    {
                        await MigrateStores(storeProgress, storesToMigrate).ConfigureAwait(false);
                    });

                    progressDialog.ShowDialog(owner);
                }
            }
        }

        /// <summary>
        /// Perform the store migration
        /// </summary>
        private async Task MigrateStores(IProgressReporter storeProgress, IEnumerable<AmazonStoreEntity> storesToMigrate)
        {
            storeProgress.Starting();
            storeProgress.PercentComplete = 0;

            var totalStores = storesToMigrate.Count();
            var storesFailed = false;

            try
            {
                var index = 1;
                foreach (var store in storesToMigrate)
                {
                    storeProgress.Detail = $"Migrating store {index} of {totalStores}";
                    storeProgress.PercentComplete = Math.Min((index * 100) / totalStores, 100);

                    var body = new MigrateMwsToSpRequest
                    {
                        CountryCode = store.AmazonApiRegion,
                        SellingPartnerId = store.MerchantID,
                        MwsAuthToken = store.AuthToken,
                        LastModifiedDate = await downloadStartingPoint.OnlineLastModified(store) ?? DateTime.UtcNow.AddDays(-30)
                    };

                    var request = requestFactory.Create(WarehouseEndpoints.MigrateAmazonStore, Method.POST, body);

                    try
                    {
                        var response = await warehouseClient.MakeRequest<MigrateMwsToSpResponse>(request, "MigrateAmazonStore");
                        store.OrderSourceID = response.OrderSourceId;
                        await storeManager.SaveStoreAsync(store);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Amazon store {store.StoreName} failed to migrate to SP: {ex.Message}", ex);
                        storesFailed = true;
                    }

                    index++;
                }

                storeProgress.Detail = "Done";

                if (storesFailed)
                {
                    storeProgress.Failed(new Exception("Some stores failed to migrate. See the log for details."));
                }
                else
                {
                    storeProgress.Completed();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Migrating Amazon stores to SP failed: {ex.Message}", ex);
                storeProgress.Detail = "Done";
                storeProgress.Failed(ex);
            }
        }
    }
}
