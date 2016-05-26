using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="OdbcImportFieldMappingControl"/>
    /// </summary>
    public class OdbcImportFieldMappingControlViewModel : IOdbcImportFieldMappingControlViewModel, INotifyPropertyChanged
    {
        private readonly IOdbcFieldMapFactory fieldMapFactory;
        private readonly IOdbcSchema schema;
        private readonly Func<Type, ILog> logFactory;
        private readonly IMessageHelper messageHelper;
        private IOdbcTable selectedTable;
        private ObservableCollection<OdbcColumn> columns;
        private OdbcFieldMapDisplay selectedFieldMap;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        private IOdbcTable previousSelectedTable = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingControlViewModel"/> class.
        /// </summary>
        public OdbcImportFieldMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory, IOdbcDataSource dataSource,
            IOdbcSchema schema, Func<Type, ILog> logFactory, IMessageHelper messageHelper)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.DataSource = dataSource;
            this.schema = schema;
            this.logFactory = logFactory;
            this.messageHelper = messageHelper;

            SaveMapCommand = new RelayCommand(SaveMapToDisk,() => selectedTable != null);
            TableChangedCommand = new RelayCommand(TableChanged);

            OrderFieldMap = new OdbcFieldMapDisplay("Order", fieldMapFactory.CreateOrderFieldMap());
            AddressFieldMap = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateAddressFieldMap());
            ItemFieldMap = new OdbcFieldMapDisplay("Item", fieldMapFactory.CreateOrderItemFieldMap());

            FieldMaps = new List<OdbcFieldMapDisplay> { OrderFieldMap, AddressFieldMap, ItemFieldMap };

            selectedFieldMap = OrderFieldMap;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        public IOdbcDataSource DataSource { get; }

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
        public IOdbcTable SelectedTable
        {
            get { return selectedTable; }
            set { handler.Set(nameof(SelectedTable), ref selectedTable, value); }
        }

        /// <summary>
        /// Gets or sets the table changed command.
        /// </summary>
        public RelayCommand TableChangedCommand { get; private set; }
       
        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        public ObservableCollection<OdbcColumn> Columns
        {
            get { return columns; }
            set { handler.Set(nameof(Columns), ref columns, value); }
        }

        /// <summary>
        /// List of field maps to be mapped.
        /// </summary>
        public IEnumerable<OdbcFieldMapDisplay> FieldMaps { get; set; }

        /// <summary>
        /// The selected field map.
        /// </summary>
        public OdbcFieldMapDisplay SelectedFieldMap
        {
            get { return selectedFieldMap; }
            set { handler.Set(nameof(SelectedFieldMap), ref selectedFieldMap, value); }
        }

        /// <summary>
        /// The order field map.
        /// </summary>
        public OdbcFieldMapDisplay OrderFieldMap { get; set; }

        /// <summary>
        /// The address field map.
        /// </summary>
        public OdbcFieldMapDisplay AddressFieldMap { get; set; }

        /// <summary>
        /// The item field map.
        /// </summary>
        public OdbcFieldMapDisplay ItemFieldMap { get; set; }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        public void Load(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store);
            
            try
            {
                DataSource.Restore(store.ConnectionString);
                schema.Load(DataSource);

                Tables = schema.Tables;
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Saves the map.
        /// </summary>
        public void Save(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store);

            OdbcFieldMap map = GetSingleMap();
            Stream memoryStream = new MemoryStream();

            try
            {
                map.Save(memoryStream);
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }

            memoryStream.Position = 0;
            StreamReader reader = new StreamReader(memoryStream);

            store.Map = reader.ReadToEnd();
        }

        /// <summary>
        /// Build a single ODBC Field Map from the Order Address and Item Field Maps
        /// </summary>
        private OdbcFieldMap GetSingleMap()
        {
            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(new List<OdbcFieldMap>
            {
                OrderFieldMap.Map,
                AddressFieldMap.Map,
                ItemFieldMap.Map
            });

            map.ExternalTableName = selectedTable.Name;
            map.Entries.ForEach(e => e.ExternalField.Table = selectedTable);
            map.Entries.ForEach(e => e.ExternalField.Table.ResetColumns());

            return map;
        }

        /// <summary>
        /// Fires when user changes table.
        /// </summary>
        private void TableChanged()
        {
            if (previousSelectedTable == selectedTable)
            {
                // We set the table back.
                return;
            }

            // If the value has changed and there was a table previously selected, 
            // see if any column has been mapped and allow user to cancel if it has been
            if (previousSelectedTable != null && GetSingleMap().Entries.Any(e => e.ExternalField?.Column != null))
            {
                DialogResult questionResult = messageHelper.ShowQuestion(MessageBoxIcon.Warning,
                    MessageBoxButtons.YesNo,
                    "Changing the selected table will clear your current mapping selections. Do you want to continue?");

                if (questionResult != DialogResult.Yes)
                {
                    SelectedTable = previousSelectedTable;
                    return;
                }
            }

            selectedTable.Load(DataSource, logFactory(typeof(OdbcTable)));
            Columns = new ObservableCollection<OdbcColumn>(selectedTable.Columns);
            Columns.Insert(0, new OdbcColumn(string.Empty));

            previousSelectedTable = SelectedTable;
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

            bool? result = dlg.ShowDialog();

            if (result != null && result.Value)
            {
                FileStream fs = (FileStream)dlg.OpenFile();
                OdbcFieldMap map = GetSingleMap();

                try
                {
                    map.Save(fs);
                }
                catch (ShipWorksOdbcException ex)
                {
                    messageHelper.ShowError(ex.Message);
                }
            }
        }
    }
}