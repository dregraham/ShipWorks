using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Management;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Migrates users that were previously e-commerce customers to be hub customers
    /// </summary>
    [Component(RegistrationType.Self)]
    public class HubMigrator
    {
        private readonly IStoreManager storeManager;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly IMessageHelper messageHelper;
        private readonly IWarehouseStoreClient warehouseStoreClient;
        private readonly IConfigurationData configurationData;
        private readonly IUserSession userSession;
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public HubMigrator(IStoreManager storeManager, IStoreTypeManager storeTypeManager, IMessageHelper messageHelper,
                           IWarehouseStoreClient warehouseStoreClient, IConfigurationData configurationData,
                           IUserSession userSession, 
                           IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient,
                           Func<Type, ILog> logFactory)
        {
            this.storeManager = storeManager;
            this.storeTypeManager = storeTypeManager;
            this.messageHelper = messageHelper;
            this.warehouseStoreClient = warehouseStoreClient;
            this.configurationData = configurationData;
            this.userSession = userSession;
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;

            log = logFactory(typeof(HubMigrator));
        }

        /// <summary>
        /// If the user has any stores that need to be migrated to the hub, prompt them to do so. If they accept, upload
        /// the stores to the hub
        /// </summary>
        public void MigrateStores(IWin32Window owner)
        {
            // Only allow admin users to perform migration. Also require the database to be linked to warehouse.
            if (!string.IsNullOrWhiteSpace(configurationData.FetchReadOnly().WarehouseID) && userSession.User.IsAdmin)
            {
                IEnumerable<StoreEntity> storesToMigrate =
                    storeManager.GetAllStores()
                        .Where(s => s.WarehouseStoreID == null && storeTypeManager.GetType(s).ShouldUseHub(s) && s.ShouldMigrate);

                // Only prompt user to migrate if they have stores that need to be migrated
                if (storesToMigrate.Any())
                {
                    DialogResult dialogResult = messageHelper.ShowQuestion(owner,
                        "ShipWorks has detected stores that need to be migrated to the ShipWorks Hub. Would you like to perform the migration now?");

                    if (dialogResult == DialogResult.OK)
                    {
                        ProgressProvider progressProvider = new ProgressProvider();
                        IProgressReporter storeProgress = progressProvider.AddItem("Migrating stores");
                        storeProgress.CanCancel = false;
                        using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
                        {
                            progressDialog.Title = "Migrating to ShipWorks Hub";
                            progressDialog.AllowCloseWhenRunning = false;
                            progressDialog.AutoCloseWhenComplete = false;

                            UploadStoresToHub(storesToMigrate, storeProgress);

                            progressDialog.ShowDialog(owner);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Upload stores to the hub, updating progress along the way
        /// </summary>
        private void UploadStoresToHub(IEnumerable<StoreEntity> storesToMigrate, IProgressReporter storeProgress) =>
            Task.Run(async () =>
            {
                List<string> storesThatFailedMigration = new List<string>();
                int index = 1;
                int totalStoresToMigrate = storesToMigrate.Count();
                storeProgress.Starting();
                storeProgress.PercentComplete = 0;

                foreach (StoreEntity store in storesToMigrate)
                {
                    storeProgress.Detail = $"Migrating '{store.StoreName}' ({index} of {totalStoresToMigrate})";
                    Result result = await warehouseStoreClient.UploadStoreToWarehouse(store, false)
                        .ConfigureAwait(false);

                    if (result.Success)
                    {
                        storeManager.SaveStore(store);
                    }
                    else
                    {
                        log.Error($"Failed to migrate {store.StoreName} to ShipWorks Hub, {result.Message}");
                        storesThatFailedMigration.Add(store.StoreName);
                    }

                    storeProgress.PercentComplete = 100 * index / totalStoresToMigrate;
                    index++;
                }

                storeProgress.Detail = "Done";
                if (storesThatFailedMigration.Any())
                {
                    string errorMessage = CreateStoreMigrationErrorMessage(storesThatFailedMigration);
                    storeProgress.Failed(new Exception(errorMessage));
                }
                else
                {
                    storeProgress.Completed();
                }
            });

        /// <summary>
        /// Create a error message that includes the list of stores that failed to upload
        /// </summary>
        private static string CreateStoreMigrationErrorMessage(IEnumerable<string> storesThatFailedMigration)
        {
            StringBuilder stringBuilder = new StringBuilder("The following stores failed to migrate to ShipWorks Hub");
            stringBuilder.AppendLine();
            foreach (string storeName in storesThatFailedMigration)
            {
                stringBuilder.AppendLine($"    - {storeName}");
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine(
                "ShipWorks will attempt to migrate these stores on the next login. If this issue continues to occur, please contact ShipWorks support.");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Upload SQL Config to hub for this warehouse
        /// </summary>
        public async Task MigrateSqlConfigToHub()
        {
            string warehouseId = configurationData.FetchReadOnly().WarehouseID;

            // If no warehouse is linked, just return.
            if (string.IsNullOrWhiteSpace(warehouseId))
            {
                return;
            }

            // If SqlSession isn't configured or can't connect, just return.
            if (!SqlSession.IsConfigured || !SqlSession.Current.CanConnect())
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(SqlSessionConfiguration.SettingsFile);

            string json = JsonConvert.SerializeXmlNode(doc);

            SqlConfigChangedDto payload = new SqlConfigChangedDto()
            {
                WarehouseData = new WarehouseData()
                {
                    Id = warehouseId,
                    SqlConfig = json
                }
            };

            var request = warehouseRequestFactory.Create(WarehouseEndpoints.SqlConfig, Method.POST, payload);
            var response = await warehouseRequestClient.MakeRequest(request, "SetSqlConfig").ConfigureAwait(false);

            if (response.Success)
            {
                log.Info("MigrateSqlConfigToHub succeeded");
            }
            else
            {
                if (response.Exception == null)
                    log.Info($"MigrateSqlConfigToHub failed : {response.Message}");
                else
                    log.Error($"MigrateSqlConfigToHub failed : {response.Message}", response.Exception);
            }
        }
    }
}
