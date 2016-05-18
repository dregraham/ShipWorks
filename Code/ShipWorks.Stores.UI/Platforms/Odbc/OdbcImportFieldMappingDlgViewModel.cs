using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="OdbcImportFieldMappingControl"/>
    /// </summary>
    public class OdbcImportFieldMappingDlgViewModel : IOdbcImportFieldMappingDlgViewModel, INotifyPropertyChanged
    {
        private readonly IOdbcFieldMapFactory fieldMapFactory;
        private readonly OdbcDataSource dataSource;
        private readonly IOdbcSchema schema;
        private OdbcTable selectedTable;
        private ObservableCollection<OdbcColumn> columns;
        private OdbcFieldMap selectedFieldMap;
        protected readonly PropertyChangedHandler Handler;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingDlgViewModel"/> class.
        /// </summary>
        public OdbcImportFieldMappingDlgViewModel(IOdbcFieldMapFactory fieldMapFactory, OdbcDataSource dataSource, IOdbcSchema schema)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.dataSource = dataSource;
            this.schema = schema;
            OrderFieldMap = fieldMapFactory.CreateOrderFieldMap();
            AddressFieldMap = fieldMapFactory.CreateAddressFieldMap();
            ItemFieldMap = fieldMapFactory.CreateOrderItemFieldMap();
            FieldMaps = new List<OdbcFieldMap> { OrderFieldMap, AddressFieldMap, ItemFieldMap };
            selectedFieldMap = OrderFieldMap;
            SaveMapCommand = new RelayCommand(SaveMap);
            Handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The name the map will be saved as.
        /// </summary>
        public string MapName { get; set; }

        /// <summary>
        /// The external odbc tables.
        /// </summary>
        public IEnumerable<OdbcTable> Tables { get; set; }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        public OdbcTable SelectedTable
        {
            get { return selectedTable; }
            set
            {
                Handler.Set(nameof(SelectedTable), ref selectedTable, value);
                selectedTable.Load(dataSource);
                Columns = new ObservableCollection<OdbcColumn>(selectedTable.Columns);
            }
        }

        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        public ObservableCollection<OdbcColumn> Columns
        {
            get { return columns; }
            set { Handler.Set(nameof(Columns), ref columns, value); }
        }

        /// <summary>
        /// List of field maps to be mapped.
        /// </summary>
        public IEnumerable<OdbcFieldMap> FieldMaps { get; set; }

        /// <summary>
        /// The selected field map.
        /// </summary>
        public OdbcFieldMap SelectedFieldMap
        {
            get { return selectedFieldMap; }
            set { Handler.Set(nameof(SelectedFieldMap), ref selectedFieldMap, value); }
        }

        /// <summary>
        /// The order field map.
        /// </summary>
        public OdbcFieldMap OrderFieldMap { get; set; }

        /// <summary>
        /// The address field map.
        /// </summary>
        public OdbcFieldMap AddressFieldMap { get; set; }

        /// <summary>
        /// The item field map.
        /// </summary>
        public OdbcFieldMap ItemFieldMap { get; set; }

        /// <summary>
        /// The save map command.
        /// </summary>
        public ICommand SaveMapCommand { get; set; }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        public void Load(OdbcStoreEntity store)
        {
            dataSource.Restore(store.ConnectionString);
            schema.Load(dataSource);

            Tables = schema.Tables;
        }

        /// <summary>
        /// Saves the map.
        /// </summary>
        private void SaveMap()
        {
            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(new List<OdbcFieldMap>
            {
                OrderFieldMap,
                AddressFieldMap,
                ItemFieldMap
            });
        }
    }
}