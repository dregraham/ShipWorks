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
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using ShipWorks.Stores.Platforms.GenericFile.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    /// <summary>
    /// View model for the OdbcHubControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class GenericStoreHubViewModel : ViewModelBase
    {
        private readonly IGenericFileStoreClient storeClient;
        private ObservableCollection<Store> stores;
        private Store selectedStore;
        private IDictionary<Guid, Store> storesFromHub;
        private bool createNew;
        private string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreHubViewModel(IGenericFileStoreClient storeClient)
        {
            this.storeClient = storeClient;

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
        public async Task<Result> Save(GenericFileStoreEntity storeEntity)
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
                    var baseStore = storesFromHub.SingleOrDefault(x => x.Value == SelectedStore);

                    var storeResult = await storeClient.GetStore(baseStore.Key, baseStore.Value).ConfigureAwait(true);

                    if (storeResult.Failure)
                    {
                        return Result.FromError(storeResult.Message);
                    }

                    SaveStoreEntity(storeEntity, baseStore.Key, storeResult.Value);
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
        private void SaveStoreEntity(GenericFileStoreEntity storeEntity, Guid warehouseStoreId, GenericFileStore genericFileStore)
        {
            storeEntity.WarehouseStoreID = warehouseStoreId;
            storeEntity.StoreName = genericFileStore.Name;

            storeEntity.FileSource = (int) GenericFileSourceTypeCode.Warehouse;
            storeEntity.FileFormat = genericFileStore.FileFormat;
        }

        /// <summary>
        /// Clear the ODBC store entity details
        /// </summary>
        private void ClearStoreEntity(GenericFileStoreEntity storeEntity)
        {
            storeEntity.WarehouseStoreID = null;
            storeEntity.StoreName = null;

            storeEntity.FlatImportMap = string.Empty;
        }

        /// <summary>
        /// Load the list of stores from the hub
        /// </summary>
        public async Task LoadStoresFromHub()
        {
            try
            {
                Message = null;

                var result = await storeClient.GetStores().ConfigureAwait(true);

                if (result.Failure)
                {
                    Message = result.Message;
                    storesFromHub = new Dictionary<Guid, Store>();
                } else
                {
                    storesFromHub = result.Value;
                }

                Stores = new ObservableCollection<Store>(storesFromHub.Values);

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
