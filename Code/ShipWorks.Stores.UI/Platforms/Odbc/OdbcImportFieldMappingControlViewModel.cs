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
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using ShipWorks.Data.Model.HelperClasses;
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
        private bool orderHasSingleLineItem = true;
        private int numberOfAttributesPerItem;
        private int numberOfItemsPerOrder;
        private ObservableCollection<OdbcFieldMapDisplay> displayFieldMaps;

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

            OdbcFieldMapDisplay orderMap = new OdbcFieldMapDisplay("Order", fieldMapFactory.CreateOrderFieldMap());
            OdbcFieldMapDisplay addressMap = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateAddressFieldMap());

            displayFieldMaps = new ObservableCollection<OdbcFieldMapDisplay>(){ orderMap, addressMap };

            selectedFieldMap = displayFieldMaps[0];

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            NumbersUpTo25 = new List<int>();
            for (int i = 0; i <= 25; i++)
            {
                NumbersUpTo25.Add(i);
            }
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcDataSource DataSource { get; }

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
        public bool OrderHasSingleLineItem
        {
            get { return orderHasSingleLineItem; }
            set
            {
                if (value)
                {
                    SwitchToSingleLineItems();
                }
                else
                {
                    SwitchToMultiLineItems();
                }

                handler.Set(nameof(OrderHasSingleLineItem), ref orderHasSingleLineItem, value);
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
                if(OrderHasSingleLineItem)
                {
                    int delta = value - numberOfItemsPerOrder;

                    if (delta > 0)
                    {
                        for (int i = numberOfItemsPerOrder; i < value; i++)
                        {
                            OdbcFieldMap map = fieldMapFactory.CreateOrderItemFieldMap();

                            // Give the new item the correct number of attributes
                            for (int j = 0; j < numberOfAttributesPerItem; j++)
                            {
                                ShipWorksOdbcMappableField shipWorksField =
                                    new ShipWorksOdbcMappableField(OrderItemAttributeFields.Name, $"Attribute Name {j + 1}");
                                ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(null, null);

                                map.AddEntry(new OdbcFieldMapEntry(shipWorksField, externalField));
                            }

                            DisplayFieldMaps.Add(new OdbcFieldMapDisplay($"Item {i + 1}", map));
                        }
                    }
                    else if (delta < 0)
                    {
                        for (int i = numberOfItemsPerOrder; i > value; i--)
                        {
                            DisplayFieldMaps.RemoveAt(DisplayFieldMaps.Count-1);
                        }
                    }
                }

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

                if (delta > 0)
                {
                    foreach (OdbcFieldMapDisplay displayMap in DisplayFieldMaps.
                        Where(displayMap => displayMap.DisplayName.Contains("Item")))
                    {
                        for (int i = numberOfAttributesPerItem; i < value; i++)
                        {
                            ShipWorksOdbcMappableField shipWorksField =
                                new ShipWorksOdbcMappableField(OrderItemAttributeFields.Name,
                                    $"Attribute Name {i + 1}");
                            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(null, null);

                            displayMap.Map.AddEntry(new OdbcFieldMapEntry(shipWorksField, externalField));
                        }
                    }
                }
                else if (delta < 0)
                {
                    foreach (OdbcFieldMapDisplay displayMap in DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item")))
                    {
                        for (int i = numberOfAttributesPerItem; i > value; i--)
                        {
                            displayMap.Map.RemoveEntryAt(displayMap.Map.Entries.Count() - 1);
                        }
                    }
                }

                handler.Set(nameof(NumberOfAttributesPerItem), ref numberOfAttributesPerItem, value);
            }
        }

        /// <summary>
        /// List of numbers 1-25 for binding to number of items and number of attributes lists
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<int> NumbersUpTo25 { get; }

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
            IEnumerable<IOdbcFieldMapEntry> entries = DisplayFieldMaps[0].Map.Entries.Where(e => e.ShipWorksField.IsRequired);

            foreach (IOdbcFieldMapEntry entry in entries.Where(entry => entry.ExternalField.Column == null ||
            entry.ExternalField.Column.Name.Equals("(None)", StringComparison.InvariantCulture)))
            {
                messageHelper.ShowError($"{entry.ShipWorksField.DisplayName} is a required field. Please select a column to map to {entry.ShipWorksField.DisplayName}.");
                return false;
            }

            if (!OrderHasSingleLineItem && string.IsNullOrWhiteSpace(RecordIdentifier?.Name))
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

            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(maps);

            map.ExternalTableName = selectedTable.Name;

            map.Entries.ToList().ForEach(e =>
            {
                e.ExternalField.Table = selectedTable;
            });

            if (!OrderHasSingleLineItem)
            {
                map.RecordIdentifierSource = RecordIdentifier?.Name;
            }
            else
            {
                IEnumerable<IOdbcFieldMapEntry> entries = map.FindEntriesBy(OrderFields.OrderNumber);
                map.RecordIdentifierSource = entries.FirstOrDefault()?.ExternalField.Column.Name;
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
            SaveFileDialog dlg = new SaveFileDialog
            {
                DefaultExt = "swdbm",
                Filter = "ShipWorks Database Map|*.swdbm"
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

        /// <summary>
        /// Switches to single line items.
        /// </summary>
        private void SwitchToSingleLineItems()
        {
            List<OdbcFieldMapDisplay> maps = new List<OdbcFieldMapDisplay>()
            {
                DisplayFieldMaps[0], DisplayFieldMaps[1]
            };

            // If no items, dont show items in map list
            if (NumberOfItemsPerOrder == 0)
            {
                DisplayFieldMaps = new ObservableCollection<OdbcFieldMapDisplay>(maps);
                return;
            }

            // If we only have order and address maps, add an Item 1, otherwise, change Item to Item 1
            if (DisplayFieldMaps.Count == 2)
            {
                maps.Add(new OdbcFieldMapDisplay("Item 1", fieldMapFactory.CreateOrderItemFieldMap()));
            }
            else
            {
                maps.Add(DisplayFieldMaps[2]);
                maps[2].DisplayName = "Item 1";
            }

            // add new item entries
            for (int i = 1; i < NumberOfItemsPerOrder; i++)
            {
                maps.Add(new OdbcFieldMapDisplay($"Item {i + 1}", fieldMapFactory.CreateOrderItemFieldMap()));
            }

            DisplayFieldMaps = new ObservableCollection<OdbcFieldMapDisplay>(maps);
        }

        /// <summary>
        /// Switches to multi line items.
        /// </summary>
        private void SwitchToMultiLineItems()
        {
            if (DisplayFieldMaps.Count == 2)
            {
                DisplayFieldMaps.Add(new OdbcFieldMapDisplay("Item", fieldMapFactory.CreateOrderItemFieldMap()));
                return;
            }

            List<OdbcFieldMapDisplay> maps = new List<OdbcFieldMapDisplay>()
            {
                DisplayFieldMaps[0], DisplayFieldMaps[1], DisplayFieldMaps[2]
            };

            maps[2].DisplayName = "Item";

            DisplayFieldMaps = new ObservableCollection<OdbcFieldMapDisplay>(maps);
        }
    }
}