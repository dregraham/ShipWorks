using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private string mapName;
        private bool isSingleLineOrder = true;
        private int numberOfAttributesPerItem;
        private int numberOfItemsPerOrder;
        private ObservableCollection<OdbcFieldMapDisplay> displayFieldMaps;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingControlViewModel"/> class.
        /// </summary>
        public OdbcImportFieldMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory,
            IOdbcSchema schema, Func<Type, ILog> logFactory, IMessageHelper messageHelper)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.schema = schema;
            this.logFactory = logFactory;
            this.messageHelper = messageHelper;

            SaveMapCommand = new RelayCommand(SaveMapToDisk,() => selectedTable != null);
            TableChangedCommand = new RelayCommand(TableChanged);

            OdbcFieldMapDisplay orderMap = new OdbcFieldMapDisplay("Order", fieldMapFactory.CreateOrderFieldMap());
            OdbcFieldMapDisplay addressMap = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateAddressFieldMap());

            displayFieldMaps = new ObservableCollection<OdbcFieldMapDisplay>(){ orderMap, addressMap };

            selectedFieldMap = displayFieldMaps[0];

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            NumbersUpTo25 = Enumerable.Range(0, 26).ToList();
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcDataSource DataSource { get; private set; }

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
                    mapName = SelectedTable == null ? DataSource.Name : $"{DataSource.Name} - {SelectedTable.Name}";
                }
                return mapName;
            }
            set { handler.Set(nameof(MapName), ref mapName, value); }
        }

        /// <summary>
        /// The external odbc tables.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<OdbcTable> Tables { get; set; }

        /// <summary>
        /// Save Map Command
        /// </summary>
        /// <remarks>
        /// selected table must not be null for it to be enabled
        /// </remarks>
        [Obfuscation(Exclude = true)]
        public ICommand SaveMapCommand { get; set; }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcTable SelectedTable
        {
            get { return selectedTable; }
            set
            {
                // Set map name for the user, if they have not altered it.
                // Starts by setting map name to selected data source name.
                // When a table is selected, if map name is untouched by user,
                // the map name is changed to "DataSourceName - SelectedColumnName"
                if (MapName != null && DataSource.Name != null &&
                    (MapName.Equals(DataSource.Name, StringComparison.InvariantCulture) ||
                    MapName.Equals($"{DataSource.Name} - {SelectedTable.Name}", StringComparison.InvariantCulture)))
                {
                    MapName = $"{DataSource.Name} - {value.Name}";
                }

                handler.Set(nameof(SelectedTable), ref selectedTable, value);
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
        /// List of field maps to be mapped.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<OdbcFieldMapDisplay> DisplayFieldMaps
        {
            get { return displayFieldMaps; }
            set { handler.Set(nameof(DisplayFieldMaps), ref displayFieldMaps, value); }
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
                DisplayFieldMaps[2].DisplayName = value ? "Item 1" : "Item";

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
                    "Should never set multi line order numberOfItemsPerORder to a value other than 1.");

                int delta = value - numberOfItemsPerOrder;

                if (delta > 0)
                {
                    for (int i = numberOfItemsPerOrder + 1; i <= value; i++)
                    {
                        OdbcFieldMap map = fieldMapFactory.CreateOrderItemFieldMap(0);
                        DisplayFieldMaps.Add(new OdbcFieldMapDisplay($"Item {i}", map));

                        // Give the new item the correct number of attributes
                        GetRangeOfAttributes(1, numberOfAttributesPerItem).ToList().ForEach(m => map.AddEntry(m));
                    }
                }
                else if (delta < 0)
                {
                    DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"))
                        .Skip(value)
                        .ToList()
                        .ForEach(map => DisplayFieldMaps.Remove(map));
                }

                Debug.Assert(DisplayFieldMaps.Count(m => m.DisplayName.Contains("Item")) == value,
                    $"Number of items not equal to value. Value = {value}, Number of items = {DisplayFieldMaps.Count(m => m.DisplayName.Contains("Item"))}");

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
                IEnumerable<OdbcFieldMapDisplay> itemMaps = DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));

                if (delta != 0)
                {
                    foreach (OdbcFieldMapDisplay displayMap in itemMaps)
                    {
                        if (delta > 0)
                        {
                            GetRangeOfAttributes(numberOfAttributesPerItem + 1, delta)
                                .ToList()
                                .ForEach(displayMap.Map.AddEntry);
                        }
                        else // delta < 0
                        {
                            displayMap.Map.FindEntriesBy(OrderItemAttributeFields.Name)
                                .Skip(value)
                                .ToList()
                                .ForEach(m => displayMap.Map.RemoveEntry(m));
                        }

                        Debug.Assert(displayMap.Map.FindEntriesBy(OrderItemAttributeFields.Name).Count() == value,
                            "Error setting number of attributes");
                    }
                }

                handler.Set(nameof(NumberOfAttributesPerItem), ref numberOfAttributesPerItem, value);
            }
        }

        private IEnumerable<OdbcFieldMapEntry> GetRangeOfAttributes(int startAttributeNumber, int numberOfAttributes)
        {
            // Generate attribute numbers for new attributes to add and add them.
            return Enumerable.Range(startAttributeNumber, numberOfAttributes)
                .Select(
                    attributeNumber =>
                        new OdbcFieldMapEntry(
                            new ShipWorksOdbcMappableField(OrderItemAttributeFields.Name,
                                $"Attribute {attributeNumber}"), new ExternalOdbcMappableField(null, null)));
        } 

        /// <summary>
        /// List of numbers 1-25 for binding to number of items and number of attributes lists
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<int> NumbersUpTo25 { get; }

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

        /// <summary>
        /// Checks the required fields.
        /// </summary>
        public bool EnsureRequiredFieldsHaveValue()
        {
            // Finds an entry that is required and the external column has not been set
            IOdbcFieldMapEntry unsetRequiredFieldMap =
                DisplayFieldMaps[0].Map.Entries.FirstOrDefault(
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
        private OdbcFieldMap GetSingleMap()
        {
            IEnumerable<OdbcFieldMap> maps = DisplayFieldMaps.Select(m => m.Map);

            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(maps.SelectMany(m=>m.Entries));

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
            Columns.Insert(0, new OdbcColumn("(None)"));

            previousSelectedTable = SelectedTable;
        }

        /// <summary>
        /// Prompt the user and save the map to disk
        /// </summary>
        private void SaveMapToDisk()
        {
            if (!EnsureRequiredFieldsHaveValue())
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
                        OdbcFieldMap map = GetSingleMap();
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