using Autofac;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="WizardPages.OdbcImportFieldMappingControl"/>
    /// </summary>
    public class OdbcImportFieldMappingControlViewModel : IOdbcImportFieldMappingControlViewModel, INotifyPropertyChanged
    {
        private const string CustomQueryColumnSourceName = "Custom Import...";
        private readonly IOdbcFieldMapFactory fieldMapFactory;
        private readonly IOdbcSchema schema;
        private readonly Func<Type, ILog> logFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private IOdbcColumnSource selectedTable;
        private ObservableCollection<OdbcColumn> columns;
        private OdbcFieldMapDisplay selectedFieldMap;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ILog log;

        private IOdbcColumnSource previousSelectedColumnSource;
        private string mapName = string.Empty;
        private bool isSingleLineOrder = true;
        private int numberOfAttributesPerItem;
        private int numberOfItemsPerOrder;
        private IEnumerable<IOdbcColumnSource> columnSources;
        private bool isTableSelected = true;
        private bool isDownloadStrategyLastModified = true;

        private DataTable queryResults;
        private string customQuery;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;

        private IOdbcColumnSource columnSource;


        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingControlViewModel"/> class.
        /// </summary>
        public OdbcImportFieldMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory,
            IOdbcSchema schema, Func<Type, ILog> logFactory, IMessageHelper messageHelper,
            IOdbcSampleDataCommand sampleDataCommand)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.schema = schema;
            this.logFactory = logFactory;
            this.messageHelper = messageHelper;
            this.sampleDataCommand = sampleDataCommand;

            log = logFactory(typeof (OdbcImportFieldMappingControlViewModel));

            SaveMapCommand = new RelayCommand(SaveMapToDisk,() => selectedTable != null);
            TableChangedCommand = new RelayCommand(TableChanged);
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery);

            Order = new OdbcFieldMapDisplay("Order", fieldMapFactory.CreateOrderFieldMap());
            Address = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateAddressFieldMap());
            Items = new ObservableCollection<OdbcFieldMapDisplay>();

            selectedFieldMap = Order;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            NumbersUpTo25 = Enumerable.Range(0, 26).ToList();
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        public IOdbcDataSource DataSource { get; private set; }

        /// <summary>
        /// Gets or sets the order entries.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcFieldMapDisplay Order { get; set; }

        /// <summary>
        /// Gets or sets the address entries.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcFieldMapDisplay Address { get; set; }

        /// <summary>
        /// Gets or sets the item entries.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<OdbcFieldMapDisplay> Items { get; set; }

        /// <summary>
        /// The name the map will be saved as.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string MapName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(mapName))
                {
                    mapName = ColumnSource == null ? DataSource.Name : $"{DataSource.Name} - {ColumnSource.Name}";
                }
                return mapName;
            }
            set { handler.Set(nameof(MapName), ref mapName, value); }
        }

        /// <summary>
        /// The external odbc tables.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOdbcColumnSource> ColumnSources
        {
            get { return columnSources; }
            set { handler.Set(nameof(ColumnSources), ref columnSources, value); }
        }

        /// <summary>
        /// Save Map Command
        /// </summary>
        /// <remarks>
        /// selected table must not be null for it to be enabled
        /// </remarks>
        [Obfuscation(Exclude = true)]
        public ICommand SaveMapCommand { get; private set; }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource SelectedTable
        {
            get { return selectedTable; }
            set { handler.Set(nameof(SelectedTable), ref selectedTable, value); }
        }

        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource ColumnSource
        {
            get { return columnSource; }
            set
            {
                // Set map name for the user, if they have not altered it.
                // Starts by setting map name to selected data source name.
                // When a table is selected, if map name is untouched by user,
                // the map name is changed to "DataSourceName - SelectedColumnName"
                if (MapName != null && DataSource.Name != null &&
                    (MapName.Equals(DataSource.Name, StringComparison.InvariantCulture) ||
                    MapName.Equals($"{DataSource.Name} - {ColumnSource.Name}", StringComparison.InvariantCulture)))
                {
                    MapName = value == null ? $"{DataSource.Name}" : $"{DataSource.Name} - {value.Name}";
                }

                handler.Set(nameof(ColumnSource), ref columnSource, value);
            }
        }

        /// <summary>
        /// Gets or sets the table changed command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand TableChangedCommand { get; private set; }

        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<OdbcColumn> Columns
        {
            get { return columns; }
            set { handler.Set(nameof(Columns), ref columns, value); }
        }

        /// <summary>
        /// The selected field map.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcFieldMapDisplay SelectedFieldMap
        {
            get { return selectedFieldMap; }
            set { handler.Set(nameof(SelectedFieldMap), ref selectedFieldMap, value); }
        }

        /// <summary>
        /// Gets or sets the record identifier for multiline order items
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcColumn RecordIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [order has a single line item].
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsSingleLineOrder
        {
            get { return isSingleLineOrder; }
            set
            {
                NumberOfItemsPerOrder = 1;
                Items[0].DisplayName = value ? "Item 1" : "Item";

                handler.Set(nameof(IsSingleLineOrder), ref isSingleLineOrder, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of items per order.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int NumberOfItemsPerOrder
        {
            get { return numberOfItemsPerOrder; }
            set
            {
                Debug.Assert(IsSingleLineOrder || value == 1,
                    "Should never set multi line order numberOfItemsPerOrder to a value other than 1.");

                int delta = value - numberOfItemsPerOrder;

                if (delta > 0)
                {
                    for (int i = numberOfItemsPerOrder; i < value; i++)
                    {
                        OdbcFieldMap map = fieldMapFactory.CreateOrderItemFieldMap(i);

                        // Give the new item the correct number of attributes
                        GetRangeOfAttributes(1, numberOfAttributesPerItem, i).ToList().ForEach(m => map.AddEntry(m));

                        Items.Add(new OdbcFieldMapDisplay($"Item {i + 1}", map, i));
                    }
                }
                else if (delta < 0)
                {
                    Items.Skip(value).ToList().ForEach(map => Items.Remove(map));
                }

                Debug.Assert(Items.Count(m => m.DisplayName.Contains("Item")) == value,
                    $"Number of items not equal to value. Value = {value}, Number of items = {Items.Count}");

                handler.Set(nameof(NumberOfItemsPerOrder), ref numberOfItemsPerOrder, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of attributes per item.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int NumberOfAttributesPerItem
        {
            get { return numberOfAttributesPerItem; }
            set
            {
                int delta = value - numberOfAttributesPerItem;
                IEnumerable<OdbcFieldMapDisplay> itemMaps = Items;

                if (delta != 0)
                {
                    foreach (OdbcFieldMapDisplay displayMap in itemMaps)
                    {
                        if (delta > 0)
                        {
                            GetRangeOfAttributes(numberOfAttributesPerItem + 1, delta, displayMap.Index)
                                .ToList()
                                .ForEach(displayMap.Entries.Add);
                        }
                        else // delta < 0
                        {
                            FindEntriesBy(displayMap, OrderItemAttributeFields.Name)
                                .Skip(value)
                                .ToList()
                                .ForEach(m => displayMap.Entries.Remove(m));
                        }

                        Debug.Assert(FindEntriesBy(displayMap, OrderItemAttributeFields.Name).Count() == value,
                            "Error setting number of attributes");
                    }
                }

                handler.Set(nameof(NumberOfAttributesPerItem), ref numberOfAttributesPerItem, value);
            }
        }

        /// <summary>
        /// Finds the OdbcFieldMapEntries corresponding to the given field
        /// </summary>
        private IEnumerable<IOdbcFieldMapEntry> FindEntriesBy(OdbcFieldMapDisplay map, EntityField2 field)
        {
            return map.Entries.Where(entry =>
                entry.ShipWorksField.Name == field.Name &&
                entry.ShipWorksField.ContainingObjectName == field.ContainingObjectName);
        }

        /// <summary>
        /// Gets the specified number of attributes with item numbers started at the specified start number.
        /// </summary>
        private IEnumerable<OdbcFieldMapEntry> GetRangeOfAttributes(int startAttributeNumber, int numberOfAttributes, int itemIndex)
        {
            // Generate attribute numbers for new attributes to add and add them.
            return Enumerable.Range(startAttributeNumber, numberOfAttributes)
                .Select(
                    attributeNumber =>
                        new OdbcFieldMapEntry(
                            new ShipWorksOdbcMappableField(OrderItemAttributeFields.Name,
                                $"Attribute {attributeNumber}"), new ExternalOdbcMappableField(null, null),itemIndex));
        }

        /// <summary>
        /// List of numbers 0-25 for binding to number of items and number of attributes lists
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<int> NumbersUpTo25 { get; }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsTableSelected
        {
            get { return isTableSelected; }
            set
            {
                ColumnSource = value ? SelectedTable : new OdbcColumnSource(CustomQueryColumnSourceName);

                handler.Set(nameof(IsTableSelected), ref isTableSelected, value);
            }
        }

        /// <summary>
        /// Whether the download strategy is last modified.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsDownloadStrategyLastModified
        {
            get { return isDownloadStrategyLastModified; }
            set { handler.Set(nameof(IsDownloadStrategyLastModified), ref isDownloadStrategyLastModified, value); }
        }

        [Obfuscation(Exclude = true)]
        public ICommand ExecuteQueryCommand { get; set; }

        [Obfuscation(Exclude = true)]
        public DataTable QueryResults
        {
            get { return queryResults; }
            set
            {
                handler.Set(nameof(QueryResults), ref queryResults, value);
            }
        }

        [Obfuscation(Exclude = true)]
        public string CustomQuery
        {
            get { return customQuery; }
            set
            {
                handler.Set(nameof(CustomQuery), ref customQuery, value);
            }
        }

        [Obfuscation(Exclude = true)]
        public string ResultMessage
        {
            get { return resultMessage; }
            set
            {
                handler.Set(nameof(ResultMessage), ref resultMessage, value);
            }
        }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        public void Load(IOdbcDataSource dataSource)
        {
            MethodConditions.EnsureArgumentIsNotNull(dataSource);

            try
            {
                DataSource = dataSource;

                schema.Load(DataSource);
                ColumnSources = schema.Tables;

                ResetViewModel();
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
        }

        private void ResetViewModel()
        {
            MapName = string.Empty;
            IsDownloadStrategyLastModified = true;
            IsTableSelected = true;
            CustomQuery = string.Empty;
            ColumnSource = null;
            QueryResults = null;
            ResultMessage = string.Empty;
            RecordIdentifier = null;
            selectedTable = null;
            SelectedFieldMap = Order;
            NumberOfItemsPerOrder = 0;
            NumberOfAttributesPerItem = 0;
        }

        /// <summary>
        /// Saves the map.
        /// </summary>
        public void Save(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store);

            OdbcFieldMap map = CreateMap();

            using (Stream memoryStream = new MemoryStream())
            {
                try
                {
                    map.Save(memoryStream);
                }
                catch (ShipWorksOdbcException ex)
                {
                    messageHelper.ShowError(ex.Message);
                }

                memoryStream.Position = 0;
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    store.Map = reader.ReadToEnd();
                }
            }
        }

        public bool ValidateRequiredMapSettings()
        {
            if (IsTableSelected && SelectedTable == null)
            {
                messageHelper.ShowError("Please select a table before continuing to the next page.");
                return false;
            }

            if (!IsTableSelected && string.IsNullOrWhiteSpace(CustomQuery))
            {
                messageHelper.ShowError("Please enter a valid query before continuing to the next page.");
                return false;
            }

            if (!IsTableSelected)
            {
                ExecuteQuery();

                if (!IsQueryValid)
                {
                    messageHelper.ShowError("Please enter a valid query before continuing to the next page.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks the required fields.
        /// </summary>
        public bool ValidateRequiredMappingFields()
        {
            // Finds an entry that is required and the external column has not been set
            IOdbcFieldMapEntry unsetRequiredFieldMap =
               Order.Entries.FirstOrDefault(
                    entry =>
                        entry.ShipWorksField.IsRequired &&
                        (entry.ExternalField.Column?.Name.Equals("(None)", StringComparison.InvariantCulture) ?? true));

            if (unsetRequiredFieldMap != null)
            {
                string displayName = unsetRequiredFieldMap.ShipWorksField.DisplayName;
                messageHelper.ShowError($"{displayName} is a required field. Please select a column to map to {displayName}.");
                return false;
            }

            if (!IsSingleLineOrder && string.IsNullOrWhiteSpace(RecordIdentifier?.Name))
            {
                messageHelper.ShowError("When orders contain items on multiple lines, an order identifier is required to be mapped.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Build a single ODBC Field Map from the Order Address and Item Field Maps
        /// </summary>
        private OdbcFieldMap CreateMap()
        {
            List<IOdbcFieldMapEntry> mapEntries = Order.Entries.ToList();
            mapEntries.AddRange(Address.Entries);
            mapEntries.AddRange(Items.SelectMany(item => item.Entries));

            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(mapEntries);

            map.Entries.ToList().ForEach(e =>
            {
                e.ExternalField.Table = selectedTable;
            });

            if (!IsSingleLineOrder)
            {
                map.RecordIdentifierSource = RecordIdentifier?.Name;
            }
            else
            {
                IEnumerable<IOdbcFieldMapEntry> entries = map.FindEntriesBy(OrderFields.OrderNumber);
                map.RecordIdentifierSource = entries.FirstOrDefault()?.ExternalField.Column.Name;
            }

            if (string.IsNullOrEmpty(map.RecordIdentifierSource))
            {
                // This should never happen. We check for validated fields before calling this...
                throw new ShipWorksOdbcException("Cannot save a map without a record identifier.");
            }

            map.CustomQuery = CustomQuery;

            return map;
        }

        /// <summary>
        /// Fires when user changes table.
        /// </summary>
        private void TableChanged()
        {
            if (previousSelectedColumnSource == selectedTable)
            {
                // We set the table back.
                return;
            }

            // If the value has changed and there was a table previously selected,
            // see if any column has been mapped and allow user to cancel if it has been
            if (previousSelectedColumnSource != null &&
                (Order.Entries.Any(e => e.ExternalField?.Column != null) ||
                Address.Entries.Any(e => e.ExternalField?.Column != null) ||
                Items.SelectMany(item => item.Entries).Any(e => e.ExternalField?.Column != null)))
            {
                DialogResult questionResult = messageHelper.ShowQuestion(MessageBoxIcon.Warning,
                    MessageBoxButtons.YesNo,
                    "Changing the selected table will clear your current mapping selections. Do you want to continue?");

                if (questionResult != DialogResult.Yes)
                {
                    SelectedTable = previousSelectedColumnSource;
                    ColumnSource = SelectedTable;
                    return;
                }
            }

            previousSelectedColumnSource = SelectedTable;

            ColumnSource = SelectedTable;
        }

        public void LoadColumns()
        {
            if (IsTableSelected)
            {
                ColumnSource.Load(DataSource, logFactory(typeof(OdbcColumnSource)));
            }
            else
            {
                ColumnSource.Load(DataSource, logFactory(typeof(OdbcColumnSource)), CustomQuery,
                    new OdbcShipWorksDbProviderFactory());
            }

            Columns = new ObservableCollection<OdbcColumn>(ColumnSource.Columns);
            Columns.Insert(0, new OdbcColumn("(None)"));
        }

        /// <summary>
        /// Prompt the user and save the map to disk
        /// </summary>
        private void SaveMapToDisk()
        {
            if (!ValidateRequiredMappingFields())
            {
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog
            {
                DefaultExt = "swdbm",
                Filter = "ShipWorks Database Map|*.swdbm"
            };

            bool? result = dlg.ShowDialog();

            if (result != null && result.Value)
            {
                using (FileStream fs = (FileStream) dlg.OpenFile())
                {
                    try
                    {
                        OdbcFieldMap map = CreateMap();
                        map.Save(fs);
                    }
                    catch (ShipWorksOdbcException ex)
                    {
                        messageHelper.ShowError(ex.Message);
                    }
                }
            }
        }

        #region Custom Query

        /// <summary>
        /// Executes the query.
        /// </summary>
        private void ExecuteQuery()
        {
            QueryResults = null;
            ResultMessage = string.Empty;

            try
            {
                QueryResults = sampleDataCommand.Execute(DataSource, CustomQuery, NumberOfSampleResults);
                if (QueryResults.Rows.Count == 0)
                {
                    ResultMessage = "Query returned no results";
                }
                IsQueryValid = true;
            }
            catch (ShipWorksOdbcException ex)
            {
                log.Error(ex.Message);
                messageHelper.ShowError(ex.Message);
                IsQueryValid = false;
            }
        }

        public bool IsQueryValid { get; set; }

        #endregion
    }
}