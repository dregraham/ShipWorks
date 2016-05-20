using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using log4net;
using Microsoft.Win32;
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
        private readonly Func<Type, ILog> logFactory;
        private OdbcTable selectedTable;
        private ObservableCollection<OdbcColumn> columns;
        private OdbcFieldMap selectedFieldMap;
        protected readonly PropertyChangedHandler Handler;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingDlgViewModel"/> class.
        /// </summary>
        public OdbcImportFieldMappingDlgViewModel(IOdbcFieldMapFactory fieldMapFactory, OdbcDataSource dataSource, IOdbcSchema schema, Func<Type, ILog> logFactory)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.dataSource = dataSource;
            this.schema = schema;
            this.logFactory = logFactory;
            SaveMapCommand = new RelayCommand(SaveMapToDisk,() => selectedTable != null);
            OrderFieldMap = fieldMapFactory.CreateOrderFieldMap();
            AddressFieldMap = fieldMapFactory.CreateAddressFieldMap();
            ItemFieldMap = fieldMapFactory.CreateOrderItemFieldMap();
            FieldMaps = new List<OdbcFieldMap> { OrderFieldMap, AddressFieldMap, ItemFieldMap };
            selectedFieldMap = OrderFieldMap;
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
        /// Save Map Command
        /// </summary>
        /// <remarks>
        /// selected table must not be null for it to be enabled
        /// </remarks>
        public ICommand SaveMapCommand { get; set; }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        public OdbcTable SelectedTable
        {
            get { return selectedTable; }
            set
            {
                Handler.Set(nameof(SelectedTable), ref selectedTable, value);
                selectedTable.Load(dataSource, logFactory(typeof(OdbcTable)));
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
        public void Save(OdbcStoreEntity store)
        {
            OdbcFieldMap map = GetSingleMap();
            Stream memoryStream = new MemoryStream();

            map.Save(memoryStream);
            memoryStream.Position = 0;
            StreamReader reader = new StreamReader(memoryStream);

            string data = reader.ReadToEnd();
        }

        /// <summary>
        /// Build a single ODBC Field Map from the Order Address and Item Field Maps
        /// </summary>
        /// <returns></returns>
        private OdbcFieldMap GetSingleMap()
        {
            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(new List<OdbcFieldMap>
            {
                OrderFieldMap,
                AddressFieldMap,
                ItemFieldMap
            });

            map.ExternalTableName = selectedTable.Name;
            map.Entries.ForEach(e => e.ExternalField.Table = selectedTable);
            map.Entries.ForEach(e => e.ExternalField.Table.Columns = null);

            return map;
        }

        /// <summary>
        /// Prompt the user and save the map to disk
        /// </summary>
        private void SaveMapToDisk()
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                DefaultExt = "swm",
                Filter = "ShipWorks Map Files|*.swm"
            };

            dlg.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dlg.FileName))
            {
                FileStream fs = (FileStream)dlg.OpenFile();
                OdbcFieldMap map = GetSingleMap();
                map.Save(fs);
            }
        }
    }
}