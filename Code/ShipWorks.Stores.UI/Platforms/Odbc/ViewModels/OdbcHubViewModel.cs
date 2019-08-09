using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Warehouse;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    [Component(RegistrationType.Self)]
    public class OdbcHubViewModel : ViewModelBase
    {
        private readonly IOdbcStoreClient odbcStoreClient;
        private ObservableCollection<Store> stores;
        private Store selectedStore;
        private IDictionary<Guid, Store> odbcStoresFromHub;

        public OdbcHubViewModel(IOdbcStoreClient odbcStoreClient)
        {
            this.odbcStoreClient = odbcStoreClient;

            Stores = new ObservableCollection<Store>();
        }

        public ObservableCollection<Store> Stores
        {
            get => stores;
            set => Set(ref stores, value);
        }

        public Store SelectedStore
        {
            get => selectedStore;
            set => Set(ref selectedStore, value);
        }

        public ICommand NewStoreCommand { get; set; }

        public Action NewStoreAction { get; set; }

        public void Save(OdbcStoreEntity store)
        {
            if (SelectedStore != null)
            {
                Guid warehouseStoreId = odbcStoresFromHub.SingleOrDefault(x => x.Value == SelectedStore).Key;

                IOdbcFieldMap importMap = odbcStoreClient.GetImportMap(warehouseStoreId);
                IOdbcFieldMap uploadMap = odbcStoreClient.GetUploadMap(warehouseStoreId);

                store.StoreName = SelectedStore.Name;
                store.WarehouseStoreID = warehouseStoreId;

                store.ImportMap = importMap.Serialize();
                // todo: add other import fields
                store.UploadMap = uploadMap.Serialize();
                // todo: add other upload fields
            }
        }

        public void Load()
        {
            NewStoreCommand = new RelayCommand(NewStoreAction);

            odbcStoresFromHub = odbcStoreClient.GetStores();
            Stores = new ObservableCollection<Store>(odbcStoresFromHub.Values);
        }
    }
}
