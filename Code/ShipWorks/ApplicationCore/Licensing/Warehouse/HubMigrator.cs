using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Management;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public HubMigrator(IStoreManager storeManager, IStoreTypeManager storeTypeManager, IMessageHelper messageHelper, IWarehouseStoreClient warehouseStoreClient)
        {
            this.storeManager = storeManager;
            this.storeTypeManager = storeTypeManager;
            this.messageHelper = messageHelper;
            this.warehouseStoreClient = warehouseStoreClient;
        }

        /// <summary>
        /// If the user has any stores that need to be migrated to the hub, prompt them to do so. If they accept, upload
        /// the stores to the hub
        /// </summary>
        public async Task MigrateStores()
        {
            IEnumerable<StoreEntity> storesToMigrate =
                storeManager.GetAllStores()
                    .Where(s => s.WarehouseStoreID == null && storeTypeManager.GetType(s).ShouldUseHub(s));

            List<string> storesThatFailedMigration = new List<string>();

            if (storesToMigrate.Any())
            {
                DialogResult dialogResult = messageHelper.ShowQuestion(
                    "ShipWorks has detected stores that need to be migrated to the Hub. Would you like to perform the migration now?");

                if (dialogResult == DialogResult.OK)
                {
                    foreach (StoreEntity store in storesToMigrate)
                    {
                        Result result = await warehouseStoreClient.UploadStoreToWarehouse(store);

                        if (result.Success)
                        {
                            storeManager.SaveStore(store);
                        }
                        else
                        {
                            storesThatFailedMigration.Add(store.StoreName);
                        }
                    }

                    if (storesThatFailedMigration.Any())
                    {
                        string errorMessage = CreateStoreMigrationErrorMessage(storesThatFailedMigration);
                        messageHelper.ShowError(errorMessage);
                    }
                    else
                    {
                        messageHelper.ShowInformation("ShipWorks successfully migrated all of your supported stores to the Hub!");
                    }
                }
            }
        }

        /// <summary>
        /// Create a error message that includes the list of stores that failed to upload
        /// </summary>
        private static string CreateStoreMigrationErrorMessage(IEnumerable<string> storesThatFailedMigration)
        {
            StringBuilder stringBuilder = new StringBuilder("The following stores failed to migrate to the hub");
            stringBuilder.AppendLine();
            foreach (string storeName in storesThatFailedMigration)
            {
                stringBuilder.AppendLine($"\t- {storeName}");
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine(
                "ShipWorks will attempt to migrate these stores to the hub on the next login. If this issue continues to occur, please contact ShipWorks support.");
            return stringBuilder.ToString();
        }
    }
}
