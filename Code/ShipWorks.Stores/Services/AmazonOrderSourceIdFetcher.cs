using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Class to fetch missing order source IDs from the Hub
    /// </summary>
    [Component]
    public class AmazonOrderSourceIdFetcher : IAmazonOrderSourceIdFetcher
    {
        private readonly IStoreManager storeManager;
        private readonly IWarehouseRequestFactory requestFactory;
        private readonly IWarehouseRequestClient warehouseClient;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonOrderSourceIdFetcher(IStoreManager storeManager,
            IWarehouseRequestFactory requestFactory,
            IWarehouseRequestClient warehouseClient,
            Func<Type, ILog> logFactory)
        {
            this.storeManager = storeManager;
            this.requestFactory = requestFactory;
            this.warehouseClient = warehouseClient;
            log = logFactory(typeof(AmazonOrderSourceIdFetcher));
        }

        /// <summary>
        /// Fetch order source IDs from the Hub
        /// </summary>
        public void FetchOrderSourceIds(IWin32Window owner)
        {
            log.Info("Fetching missing OrderSourceIds");

            var storesToFetch = storeManager.GetAllStores().Where(x => x.StoreTypeCode == StoreTypeCode.Amazon &&
                x.OrderSourceID.IsNullOrWhiteSpace() &&
                x.WarehouseStoreID != null)
                .Cast<AmazonStoreEntity>();

            if (storesToFetch.Any())
            {
                ProgressProvider progressProvider = new ProgressProvider();
                IProgressReporter storeProgress = progressProvider.AddItem("Updating Amazon Store Order Source IDs");
                storeProgress.CanCancel = false;
                using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
                {
                    progressDialog.Title = "Updating Amazon Store Order Source IDs";
                    progressDialog.AllowCloseWhenRunning = false;
                    progressDialog.AutoCloseWhenComplete = true;

                    Task.Run(async () =>
                    {
                        await FetchAndSaveIds(storeProgress, storesToFetch).ConfigureAwait(true);
                    });

                    progressDialog.ShowDialog(owner);
                }
            }
        }

        /// <summary>
        /// Fetch the order source IDs and save them
        /// </summary>
        private async Task FetchAndSaveIds(IProgressReporter storeProgress, IEnumerable<AmazonStoreEntity> storesToFetch)
        {
            storeProgress.Starting();
            storeProgress.PercentComplete = 0;

            var totalStores = storesToFetch.Count();
            var storesFailed = false;

            try
            {
                var index = 1;

                storeProgress.Detail = $"Fetching ID {index} of {totalStores}";

                var body = new FetchOrderSourceIdsRequest
                {
                    WarehouseStoreIds = storesToFetch.Select(x => x.WarehouseStoreID.ToString()).ToList(),
                };

                var request = requestFactory.Create(WarehouseEndpoints.FetchOrderSourceIds, Method.POST, body);

                List<FetchOrderSourceIdsResponse> response = new List<FetchOrderSourceIdsResponse>();

                try
                {
                    response = await warehouseClient.MakeRequest<List<FetchOrderSourceIdsResponse>>(request, "FetchAmazonOrderSourceIDs").ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to fetch list of OrderSourceIDs: {ex.Message}", ex);
                    storesFailed = true;
                }

                foreach (var idResponse in response)
                {
                    try
                    {
                        if (!idResponse.OrderSourceId.HasValue())
                        {
                            log.Error($"Could not find OrderSourceId for store with WarehouseID {idResponse.WarehouseStoreId}");
                            storesFailed = true;
                            index++;
                            storeProgress.Detail = $"Fetching ID {index} of {totalStores}";
                            storeProgress.PercentComplete = Math.Min((index * 100) / totalStores, 100);

                            continue;
                        }

                        var store = storesToFetch.FirstOrDefault(x => x.WarehouseStoreID.ToString() == idResponse.WarehouseStoreId);

                        if (store != null)
                        {
                            store.OrderSourceID = idResponse.OrderSourceId;
                            await storeManager.SaveStoreAsync(store).ConfigureAwait(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Failed to save OrderSourceId {idResponse.OrderSourceId} to store with WarehouseID {idResponse.WarehouseStoreId}: {ex.Message}", ex);
                        storesFailed = true;
                    }

                    index++;
                    storeProgress.Detail = $"Fetching ID {index} of {totalStores}";
                    storeProgress.PercentComplete = Math.Min((index * 100) / totalStores, 100);
                }

                storeProgress.Detail = "Done";

                if (storesFailed)
                {
                    storeProgress.Failed(new Exception("Failed to fetch OrderSourceID for some stores. See log for details."));
                }
                else
                {
                    storeProgress.Completed();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Fetching OrderSourceIDs failed: {ex.Message}", ex);
                storeProgress.Detail = "Done";
                storeProgress.Failed(ex);
            }
        }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class FetchOrderSourceIdsRequest
    {
        public List<string> WarehouseStoreIds { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class FetchOrderSourceIdsResponse
    {
        public string WarehouseStoreId { get; set; }

        public string OrderSourceId { get; set; }
    }
}
