using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Autofac;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcImportFieldMappingDlgViewModel : IOdbcImportFieldMappingDlgViewModel, INotifyPropertyChanged
    {
        private OdbcTable selectedTable;
        private IEnumerable<OdbcColumn> columns;
        private OdbcFieldMap selectedFieldMap;
        protected readonly PropertyChangedHandler Handler;
        private OdbcStoreEntity store;
        public event PropertyChangedEventHandler PropertyChanged;


        public OdbcImportFieldMappingDlgViewModel(IOdbcFieldMapFactory fieldMapFactory)
        {
            OrderFieldMap = fieldMapFactory.CreateOrderFieldMap();
            AddressFieldMap = fieldMapFactory.CreateAddressFieldMap();
            ItemFieldMap = fieldMapFactory.CreateOrderItemFieldMap();
            FieldMaps = new List<OdbcFieldMap>() { OrderFieldMap, AddressFieldMap, ItemFieldMap };
            selectedFieldMap = OrderFieldMap;
            LoadMapCommand = new RelayCommand(LoadMap);
            SaveMapCommand = new RelayCommand(SaveMap);
            Handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }
        private void SaveMap()
        {

        }

        private void LoadMap()
        {

        }

        public IEnumerable<OdbcTable> Tables { get; set; }

        public OdbcTable SelectedTable
        {
            get { return selectedTable; }
            set
            {
                Handler.Set(nameof(SelectedTable), ref selectedTable, value);
                Columns = selectedTable.Columns;
            }
        }

        public IEnumerable<OdbcColumn> Columns
        {
            get { return columns; }
            set { Handler.Set(nameof(Columns), ref columns, value); }
        }

        public IEnumerable<OdbcFieldMap> FieldMaps { get; set; }

        public OdbcFieldMap OrderFieldMap { get; set; }
        public OdbcFieldMap AddressFieldMap { get; set; }
        public OdbcFieldMap ItemFieldMap { get; set; }

        public OdbcFieldMap SelectedFieldMap
        {
            get { return selectedFieldMap; }
            set { Handler.Set(nameof(SelectedFieldMap), ref selectedFieldMap, value); }
        }

        public ICommand LoadMapCommand { get; set; }

        public string MapName { get; set; }

        public ICommand SaveMapCommand { get; set; }

        public void Load(List<OdbcTable> tables)
        {
            Tables = tables;
        }

        public void LoadStore(OdbcStoreEntity store)
        {
            this.store = store;
        }

        public void LoadTables()
        {
            using (var scope = IoC.BeginLifetimeScope())
            {
                OdbcDataSource dataSource = scope.Resolve<OdbcDataSource>();
                dataSource.Restore(store.ConnectionString);

                IOdbcSchema schema = scope.Resolve<IOdbcSchema>();

                schema.Load(dataSource);

                Tables = schema.Tables;
            }
        }
    }
}