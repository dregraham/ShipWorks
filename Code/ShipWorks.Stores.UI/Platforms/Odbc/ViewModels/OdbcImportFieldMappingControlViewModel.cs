using GalaSoft.MvvmLight.Command;
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// View Model for the <see cref="WizardPages.OdbcImportFieldMappingControl"/>
    /// </summary>
    public class OdbcImportFieldMappingControlViewModel : IOdbcImportFieldMappingControlViewModel, INotifyPropertyChanged
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

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingControlViewModel"/> class.
        /// </summary>
        public OdbcImportFieldMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory,
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
                    foreach (OdbcFieldMapDisplay displayMap in itemMaps)
                    {
                        // Disabling this condition because the simplification it suggests is invalid code.
                        // The two branches of the if statement do very different things (adding or removing attributes)
#pragma warning disable S3240 // The simplest possible condition syntax should be used
                        if (delta > 0)
                        {
                            fieldMapFactory.GetAttributeRangeFieldMap(numberOfAttributesPerItem + 1, delta, displayMap.Index).Entries
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
#pragma warning restore S3240 // The simplest possible condition syntax should be used

                        Debug.Assert(FindEntriesBy(displayMap, OrderItemAttributeFields.Name).Count() == value,
                            "Error setting number of attributes");
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
            LoadColumnSource(store);
            LoadMap(store);
            LoadDownloadStrategy((OdbcImportStrategy)store.ImportStrategy);
        }

        /// <summary>
        /// Loads the import map from the store
        /// </summary>
        /// <param name="store">The store.</param>
        private void LoadMap(OdbcStoreEntity store)
        {
            IOdbcFieldMap storeFieldMap = fieldMapFactory.CreateFieldMapFrom(store.ImportMap);

            EnsureExternalFieldsExistInColumnSource(storeFieldMap);

            Order = new OdbcFieldMapDisplay("Order", fieldMapFactory.CreateOrderFieldMap(storeFieldMap));
            selectedFieldMap = Order;

            Address = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateAddressFieldMap(storeFieldMap));
            Items = new ObservableCollection<OdbcFieldMapDisplay>();

            // If there are no item fields, there are no items.
            if (!storeFieldMap.Entries.Any(e =>
                e.ShipWorksField.ContainingObjectName == "OrderItemEntity" ||
                e.ShipWorksField.ContainingObjectName == "OrderItemAttributeEntity"))
            {
                return;
            }

            numberOfItemsPerOrder = storeFieldMap.Entries.Max(e => e.Index) + 1;
            numberOfAttributesPerItem = 0;
            for (int index = 0; index < numberOfItemsPerOrder; index++)
            {
                int attributeCountForThisItem =
                    storeFieldMap.FindEntriesBy(OrderItemAttributeFields.Name).Count(e => e.Index == index);
                numberOfAttributesPerItem = Math.Max(numberOfAttributesPerItem, attributeCountForThisItem);
            }

            handler.Set(nameof(NumberOfAttributesPerItem), ref numberOfAttributesPerItem, numberOfAttributesPerItem);
            handler.Set(nameof(NumberOfItemsPerOrder), ref numberOfItemsPerOrder, numberOfItemsPerOrder);

            for (int index = 0; index < numberOfItemsPerOrder; index++)
            {
                IOdbcFieldMap map = fieldMapFactory.CreateOrderItemFieldMap(storeFieldMap, index,
                    numberOfAttributesPerItem);

                Items.Add(new OdbcFieldMapDisplay($"Item {index + 1}", map, index));
            }

            RecordIdentifier = new OdbcColumn(storeFieldMap.RecordIdentifierSource);
            IOdbcFieldMapEntry orderNumberEntry = storeFieldMap.FindEntriesBy(OrderFields.OrderNumber, true).Single();

            if (numberOfItemsPerOrder == 1 && RecordIdentifier.Name != orderNumberEntry.ExternalField.Column.Name)
            {
                IsSingleLineOrder = false;
            }
        }

        /// <summary>
        /// Checks to see that any external columns loaded from the store exist in the selected column source.
        /// If they do not exist, set them to none so that the old mappings are not retained.
        /// </summary>
        /// <param name="storeFieldMap">The store field map.</param>
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
                        columnsNotFound.Add(entry.ExternalField.Column.Name);
                        entry.ExternalField.Column = new OdbcColumn(EmptyColumnName);
                    }
                }
                if (columnsNotFound.Any())
                {
                    messageHelper.ShowWarning(
                        $"The column(s) {string.Join(", ", columnsNotFound)} could not be found in the current data source. Any mappings that use these column(s) have been reset. Changes will not be saved until the finish button is clicked.");
                }
            }
        }

        /// <summary>
        /// Loads the column source.
        /// </summary>
        private void LoadColumnSource(OdbcStoreEntity store)
        {
            IOdbcDataSource selectedDataSource = dataSourceService.GetImportDataSource(store);

            string columnSourceName = store.ImportColumnSourceType == (int)OdbcColumnSourceType.Table ?
                store.ImportColumnSource :
                CustomQueryColumnSourceName;

            IOdbcColumnSource columnSource = columnSourceFactory(columnSourceName);

            columnSource.Load(selectedDataSource, store.ImportColumnSource,
                (OdbcColumnSourceType)store.ImportColumnSourceType);

            ColumnSource = columnSource;
            Columns = new ObservableCollection<OdbcColumn>(ColumnSource.Columns);
            Columns.Insert(0, new OdbcColumn("(None)"));
        }

        /// <summary>
        /// Loads the download strategy.
        /// </summary>
        /// <param name="importStrategy">The download strategy.</param>
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
                    store.ImportMap = reader.ReadToEnd();
                }
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
            List<IOdbcFieldMapEntry> mapEntries = GetAllMapEntries();

            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(mapEntries);

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
    }
}