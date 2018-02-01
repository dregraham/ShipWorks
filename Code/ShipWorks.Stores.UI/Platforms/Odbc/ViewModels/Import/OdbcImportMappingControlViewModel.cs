using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    /// <summary>
    /// View Model for the OdbcImportMappingControl
    /// </summary>
    public class OdbcImportMappingControlViewModel : IOdbcImportMappingControlViewModel, INotifyPropertyChanged
    {
        private readonly IOdbcFieldMapFactory fieldMapFactory;
        private readonly IMessageHelper messageHelper;
        private ObservableCollection<OdbcColumn> columns;
        private OdbcFieldMapDisplay selectedFieldMap;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Func<string, IOdbcColumnSource> columnSourceFactory;
        private readonly IOdbcDataSourceService dataSourceService;

        private const string CustomQueryColumnSourceName = "Custom Import";
        private const string EmptyColumnName = "(None)";
        private bool isSingleLineOrder = true;
        private int numberOfAttributesPerItem;
        private int numberOfItemsPerOrder;
        private string loadedMapName;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMappingControlViewModel"/> class.
        /// </summary>
        public OdbcImportMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory,
            IMessageHelper messageHelper,
            Func<string, IOdbcColumnSource> columnSourceFactory,
            IOdbcDataSourceService dataSourceService)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.messageHelper = messageHelper;
            this.columnSourceFactory = columnSourceFactory;
            this.dataSourceService = dataSourceService;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            NumbersUpTo25 = Enumerable.Range(0, 26).ToList();
        }

        /// <summary>
        /// The column source.
        /// </summary>
        public IOdbcColumnSource ColumnSource { get; private set; }

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
        /// Save Map Command
        /// </summary>
        /// <remarks>
        /// selected table must not be null for it to be enabled
        /// </remarks>
        [Obfuscation(Exclude = true)]
        public ICommand SaveMapCommand { get; private set; }

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
                if (value)
                {
                    if (Items.Count > 0)
                    {
                        Items[0].DisplayName = "Item 1";
                    }
                }
                else
                {
                    NumberOfItemsPerOrder = 1;
                    Items[0].DisplayName = "Item";
                }

                handler.Set(nameof(IsSingleLineOrder), ref isSingleLineOrder, value);
                handler.RaisePropertyChanged(nameof(Items));
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
                        IOdbcFieldMap map = fieldMapFactory.CreateOrderItemFieldMap(null, i, numberOfAttributesPerItem);

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
                    foreach (OdbcFieldMapDisplay itemFieldMap in itemMaps)
                    {
                        // Disabling this condition because the simplification it suggests is invalid code.
                        // The two branches of the if statement do very different things (adding or removing attributes)
#pragma warning disable S3240 // The simplest possible condition syntax should be used
                        if (delta > 0)
                        {
                            fieldMapFactory.GetAttributeRangeFieldMap(numberOfAttributesPerItem + 1, delta, itemFieldMap.Index).Entries
                                .ToList()
                                .ForEach(itemFieldMap.Entries.Add);
                        }
                        else // delta < 0
                        {
                            IEnumerable<IOdbcFieldMapEntry> attributeEntries = FindEntriesBy(itemFieldMap, OrderItemAttributeFields.Name);
                            attributeEntries
                                .Skip(value)
                                .ToList()
                                .ForEach(m => itemFieldMap.Entries.Remove(m));
                        }
#pragma warning restore S3240 // The simplest possible condition syntax should be used
                    }
                }

                handler.Set(nameof(NumberOfAttributesPerItem), ref numberOfAttributesPerItem, value);
            }
        }

        /// <summary>
        /// List of numbers 0-25 for binding to number of items and number of attributes lists
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<int> NumbersUpTo25 { get; }

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
        /// Loads the specified store.
        /// </summary>
        public void Load(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            LoadColumnSource(store);
            LoadMap(store);
            LoadDownloadStrategy((OdbcImportStrategy) store.ImportStrategy);
        }

        /// <summary>
        /// Loads the import map from the store
        /// </summary>
        private void LoadMap(OdbcStoreEntity store)
        {
            IOdbcFieldMap storeFieldMap = fieldMapFactory.CreateFieldMapFrom(store.ImportMap);
            loadedMapName = storeFieldMap.Name;

            EnsureExternalFieldsExistInColumnSource(storeFieldMap);

            Order = new OdbcFieldMapDisplay("Order", fieldMapFactory.CreateOrderFieldMap(storeFieldMap));
            selectedFieldMap = Order;

            Address = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateAddressFieldMap(storeFieldMap));
            Items = new ObservableCollection<OdbcFieldMapDisplay>();

            // Only attempt to load item mappings when the map contains item entries
            if (storeFieldMap.Entries.Any(e =>
                e.ShipWorksField.ContainingObjectName == "OrderItemEntity" ||
                e.ShipWorksField.ContainingObjectName == "OrderItemAttributeEntity"))
            {
                LoadItemMappings(storeFieldMap);
            }
            else
            {
                // Default to 1 item per order
                NumberOfItemsPerOrder = 1;
            }

            RecordIdentifier =
                columns.Any(c => c.Name.Equals(storeFieldMap.RecordIdentifierSource, StringComparison.InvariantCulture)) ?
                    new OdbcColumn(storeFieldMap.RecordIdentifierSource) :
                    columns[0];

            IOdbcFieldMapEntry orderNumberEntry =
                storeFieldMap.FindEntriesBy(OrderFields.OrderNumberComplete, true).SingleOrDefault();

            // First set IsSingleLineOrder based on store value
            IsSingleLineOrder = store.ImportOrderItemStrategy == (int) OdbcImportOrderItemStrategy.SingleLine;

            // The store value may not be correct because this was introduced in v5.5, so just in case, correct it
            // if, based on the map, it is not possibly a single line order (only 1 item per line and has a reccord identifier
            // that is not the order number).
            int calculatedNumberOfEntries = storeFieldMap.Entries.Select(e => e.Index).DefaultIfEmpty(0).Max() + 1;

            if (orderNumberEntry != null &&
                calculatedNumberOfEntries == 1 &&
                !string.IsNullOrEmpty(RecordIdentifier.Name) &&
                RecordIdentifier.Name != orderNumberEntry.ExternalField.Column.Name)
            {
                IsSingleLineOrder = false;
            }
        }

        /// <summary>
        /// Loads the item mappings.
        /// </summary>
        /// <param name="storeFieldMap">The store field map.</param>
        private void LoadItemMappings(IOdbcFieldMap storeFieldMap)
        {
            numberOfItemsPerOrder = storeFieldMap.Entries.Max(e => e.Index) + 1;
            handler.Set(nameof(NumberOfItemsPerOrder), ref numberOfItemsPerOrder, numberOfItemsPerOrder);

            LoadNumberOfItemAttributes(storeFieldMap);

            for (int index = 0; index < numberOfItemsPerOrder; index++)
            {
                IOdbcFieldMap map = fieldMapFactory.CreateOrderItemFieldMap(storeFieldMap, index,
                    numberOfAttributesPerItem);

                Items.Add(new OdbcFieldMapDisplay($"Item {index + 1}", map, index));
            }
        }

        /// <summary>
        /// Loads the number of item attributes.
        /// </summary>
        private void LoadNumberOfItemAttributes(IOdbcFieldMap storeFieldMap)
        {
            numberOfAttributesPerItem = storeFieldMap.FindEntriesBy(OrderItemAttributeFields.Name)
                .Select(a => int.Parse(a.ShipWorksField.DisplayName.Substring("Attribute ".Length)))
                .DefaultIfEmpty(0)
                .Max();

            handler.Set(nameof(NumberOfAttributesPerItem), ref numberOfAttributesPerItem, numberOfAttributesPerItem);
        }

        /// <summary>
        /// Checks to see that any external columns loaded from the store exist in the selected column source.
        /// If they do not exist, set them to none so that the old mappings are not retained.
        /// </summary>
        private void EnsureExternalFieldsExistInColumnSource(IOdbcFieldMap storeFieldMap)
        {
            if (storeFieldMap.Entries.Any())
            {
                List<string> columnsNotFound = new List<string>();

                foreach (IOdbcFieldMapEntry entry in storeFieldMap.Entries)
                {
                    if (!Columns.Any(
                        c => c.Name.Equals(entry.ExternalField.Column.Name, StringComparison.InvariantCulture)))
                    {
                        columnsNotFound.Add(entry.ExternalField.Column.Name.Trim());
                        entry.ExternalField.Column = new OdbcColumn(EmptyColumnName);
                    }
                }
                if (columnsNotFound.Any())
                {
                    messageHelper.ShowWarning(
                        $"The column(s) {string.Join(", ", columnsNotFound.Distinct())} could not be found in the current data source. Any mappings that use these column(s) have been reset. Changes will not be saved until the finish button is clicked.");
                }
            }
        }

        /// <summary>
        /// Loads the column source.
        /// </summary>
        private void LoadColumnSource(OdbcStoreEntity store)
        {
            IOdbcDataSource selectedDataSource = dataSourceService.GetImportDataSource(store);

            string columnSourceName = store.ImportColumnSourceType == (int) OdbcColumnSourceType.Table
                ? store.ImportColumnSource
                : CustomQueryColumnSourceName;

            IOdbcColumnSource columnSource = columnSourceFactory(columnSourceName);

            columnSource.Load(selectedDataSource, store.ImportColumnSource,
                (OdbcColumnSourceType) store.ImportColumnSourceType);

            ColumnSource = columnSource;
            Columns = new ObservableCollection<OdbcColumn>(ColumnSource.Columns);
            Columns.Insert(0, new OdbcColumn(EmptyColumnName));
        }

        /// <summary>
        /// Loads the download strategy.
        /// </summary>
        /// <param name="importStrategy">The download strategy.</param>
        [SuppressMessage("SonarLint", "S2259: value is null on at least one execution path",
            Justification = "The variable is only used for its name")]
        private void LoadDownloadStrategy(OdbcImportStrategy importStrategy)
        {
            IOdbcFieldMapEntry lastModifiedEntry = FindEntriesBy(Order, OrderFields.OnlineLastModified).FirstOrDefault();

            if (lastModifiedEntry != null)
            {
                lastModifiedEntry.ShipWorksField.IsRequired = importStrategy == OdbcImportStrategy.ByModifiedTime;
            }

            handler.RaisePropertyChanged(nameof(lastModifiedEntry.ShipWorksField.IsRequired));
        }


        /// <summary>
        /// Saves the map.
        /// </summary>
        public void Save(OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            IOdbcFieldMap map = CreateMap();
            try
            {
                store.ImportMap = map.Serialize();
                store.ImportOrderItemStrategy = isSingleLineOrder
                    ? (int) OdbcImportOrderItemStrategy.SingleLine
                    : (int) OdbcImportOrderItemStrategy.MultiLine;
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Checks the required fields.
        /// </summary>
        public bool ValidateRequiredMappingFields()
        {
            // Finds an entry that is required and the external column has not been set
            IEnumerable<IOdbcFieldMapEntry> unsetRequiredFieldMapEntries =
               Order.Entries.Where(
                    entry =>
                        entry.ShipWorksField.IsRequired &&
                        (entry.ExternalField.Column?.Name.Equals("(None)", StringComparison.InvariantCulture) ?? true)).ToList();

            if (unsetRequiredFieldMapEntries.Count() == 1)
            {
                string displayName = unsetRequiredFieldMapEntries.FirstOrDefault()?.ShipWorksField.Name;
                messageHelper.ShowError($"{displayName} is a required field. Please select a column to map to {displayName}.");
                return false;
            }

            if (unsetRequiredFieldMapEntries.Count() > 1)
            {
                string displayNames = string.Join(", ", unsetRequiredFieldMapEntries.Select(x => x.ShipWorksField.Name));
                messageHelper.ShowError($"{displayNames} are required fields. Please select columns to map to {displayNames}.");
                return false;
            }

            if (!IsSingleLineOrder && (string.IsNullOrWhiteSpace(RecordIdentifier?.Name) || RecordIdentifier.Name == EmptyColumnName))
            {
                messageHelper.ShowError("When orders contain items on multiple lines, an order identifier is required to be mapped.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Build a single ODBC Field Map from the Order Address and Item Field Maps
        /// </summary>
        private IOdbcFieldMap CreateMap()
        {
            List<IOdbcFieldMapEntry> mapEntries = GetAllMapEntries();

            IOdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(mapEntries);
            map.Name = loadedMapName;

            if (!IsSingleLineOrder)
            {
                map.RecordIdentifierSource = RecordIdentifier?.Name;
            }
            else
            {
                IEnumerable<IOdbcFieldMapEntry> entries = map.FindEntriesBy(OrderFields.OrderNumberComplete);
                map.RecordIdentifierSource = entries.FirstOrDefault()?.ExternalField.Column.Name;
            }

            return map;
        }

        /// <summary>
        /// Gets all map entries.
        /// </summary>
        private List<IOdbcFieldMapEntry> GetAllMapEntries()
        {
            List<IOdbcFieldMapEntry> mapEntries = Order.Entries.ToList();
            mapEntries.AddRange(Address.Entries);
            mapEntries.AddRange(Items.SelectMany(item => item.Entries));
            return mapEntries;
        }
    }
}