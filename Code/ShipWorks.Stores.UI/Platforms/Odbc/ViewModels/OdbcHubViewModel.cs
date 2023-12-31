using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.Platforms.Odbc.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// View model for the OdbcHubControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OdbcHubViewModel : ViewModelBase
    {
        private readonly IOdbcStoreClient odbcStoreClient;
        private ObservableCollection<Store> stores;
        private Store selectedStore;
        private IDictionary<Guid, Store> odbcStoresFromHub;
        private bool createNew;
        private string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcHubViewModel(IOdbcStoreClient odbcStoreClient)
        {
            this.odbcStoreClient = odbcStoreClient;

            Stores = new ObservableCollection<Store>();
        }

        /// <summary>
        /// List of existing odbc stores from the hub
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<Store> Stores
        {
            get => stores;
            set => Set(ref stores, value);
        }

        /// <summary>
        /// The currently selected store in the list
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Store SelectedStore
        {
            get => selectedStore;
            set => Set(ref selectedStore, value);
        }

        /// <summary>
        /// Whether or not to create a new store
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CreateNew
        {
            get => createNew;
            set => Set(ref createNew, value);
        }

        /// <summary>
        /// Message to show the user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message
        {
            get => message;
            set => Set(ref message, value);
        }

        /// <summary>
        /// Save the selected store to a store entity
        /// </summary>
        public async Task<Result> Save(OdbcStoreEntity storeEntity)
        {
            try
            {
                if (CreateNew)
                {
                    ClearStoreEntity(storeEntity);
                    return Result.FromSuccess();
                }

                if (Stores.Any() && SelectedStore == null)
                {
                    return Result.FromError("Please select a store to connect to");
                }

                if (SelectedStore != null)
                {
                    var baseStore = odbcStoresFromHub.SingleOrDefault(x => x.Value == SelectedStore);

                    var odbcStoreResult = await odbcStoreClient.GetStore(baseStore.Key, baseStore.Value).ConfigureAwait(true);

                    if (odbcStoreResult.Failure)
                    {
                        return Result.FromError(odbcStoreResult.Message);
                    }

                    SaveStoreEntity(storeEntity, baseStore.Key, odbcStoreResult.Value);
                }

                return Result.FromSuccess();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Result.FromError(e);
            }
        }

        /// <summary>
        /// Save the odbc store details to the OdbcStoreEntity
        /// </summary>
        private void SaveStoreEntity(OdbcStoreEntity storeEntity, Guid warehouseStoreId, OdbcStore odbcStore)
        {
            storeEntity.WarehouseStoreID = warehouseStoreId;
            storeEntity.StoreName = odbcStore.Name;

            storeEntity.ImportMap = odbcStore.ImportMap;
            storeEntity.ImportStrategy = odbcStore.ImportStrategy;
            storeEntity.ImportColumnSourceType = odbcStore.ImportColumnSourceType;
            storeEntity.ImportColumnSource = odbcStore.ImportColumnSource;
            storeEntity.ImportOrderItemStrategy = odbcStore.ImportOrderItemStrategy;

            storeEntity.UploadMap = odbcStore.UploadMap;
            storeEntity.UploadStrategy = odbcStore.UploadStrategy;
            storeEntity.UploadColumnSourceType = odbcStore.UploadColumnSourceType;
            storeEntity.UploadColumnSource = odbcStore.UploadColumnSource;
        }

        /// <summary>
        /// Clear the ODBC store entity details
        /// </summary>
        private void ClearStoreEntity(OdbcStoreEntity storeEntity)
        {
            storeEntity.WarehouseStoreID = null;
            storeEntity.StoreName = null;

            storeEntity.ImportMap = string.Empty;
            storeEntity.ImportStrategy = (int) OdbcImportStrategy.ByModifiedTime;
            storeEntity.ImportColumnSourceType = (int) OdbcColumnSourceType.Table;
            storeEntity.ImportColumnSource = string.Empty;
            storeEntity.ImportOrderItemStrategy = (int) OdbcImportOrderItemStrategy.SingleLine;

            storeEntity.UploadMap = string.Empty;
            storeEntity.UploadStrategy = (int) OdbcShipmentUploadStrategy.DoNotUpload;
            storeEntity.UploadColumnSourceType = (int) OdbcColumnSourceType.Table;
            storeEntity.UploadColumnSource = string.Empty;
        }

        /// <summary>
        /// Load the list of stores from the hub
        /// </summary>
        public async Task LoadStoresFromHub()
        {
            try
            {
                Message = null;

                var result = await odbcStoreClient.GetStores().ConfigureAwait(true);

                if (result.Failure)
                {
                    Message = result.Message;
                }

                odbcStoresFromHub = result.Value;
                Stores = new ObservableCollection<Store>(odbcStoresFromHub.Values);

                if (Stores.Any())
                {
                    SelectedStore = Stores.FirstOrDefault();
                }
                else
                {
                    Message = "No existing ODBC stores were found in the Hub. Please select \"Create a new ODBC store\" and click next.";
                }
            }
            catch (Exception e)
            {
                Message = $"Failed to load existing ODBC stores from the Hub.{Environment.NewLine}{Environment.NewLine}{e.Message}";
            }
        }
    }
}
