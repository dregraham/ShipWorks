﻿using Autofac;
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
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="WizardPages.OdbcImportFieldMappingControl"/>
    /// </summary>
    public class OdbcImportFieldMappingControlViewModel : IOdbcImportFieldMappingControlViewModel, INotifyPropertyChanged
    {
        private readonly IOdbcFieldMapFactory fieldMapFactory;
        private readonly Func<Type, ILog> logFactory;
        private readonly IMessageHelper messageHelper;
        private ObservableCollection<OdbcColumn> columns;
        private OdbcFieldMapDisplay selectedFieldMap;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ILog log;

        private bool isSingleLineOrder = true;
        private int numberOfAttributesPerItem;
        private int numberOfItemsPerOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingControlViewModel"/> class.
        /// </summary>
        public OdbcImportFieldMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory,
             Func<Type, ILog> logFactory, IMessageHelper messageHelper)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.logFactory = logFactory;
            this.messageHelper = messageHelper;

            log = logFactory(typeof (OdbcImportFieldMappingControlViewModel));

            Order = new OdbcFieldMapDisplay("Order", fieldMapFactory.CreateOrderFieldMap());
            Address = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateAddressFieldMap());
            Items = new ObservableCollection<OdbcFieldMapDisplay>();

            selectedFieldMap = Order;

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
                                $"Attribute {attributeNumber}"), new ExternalOdbcMappableField(null),itemIndex));
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
        /// Loads the column source.
        /// </summary>
        public void LoadColumnSource(IOdbcColumnSource source)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, "ColumnSource");

            ColumnSource = source;
            Columns = new ObservableCollection<OdbcColumn>(ColumnSource.Columns);
            Columns.Insert(0, new OdbcColumn("(None)"));
        }

        /// <summary>
        /// Loads the download strategy.
        /// </summary>
        /// <param name="downloadStrategy">The download strategy.</param>
        public void LoadDownloadStrategy(OdbcDownloadStrategy downloadStrategy)
        {
            IOdbcFieldMapEntry lastModifiedEntry = FindEntriesBy(Order, OrderFields.OnlineLastModified).FirstOrDefault();

            int lastModifiedEntryIndex = Order.Entries.IndexOf(lastModifiedEntry);

            if (lastModifiedEntry != null)
            {
                lastModifiedEntry.ShipWorksField.IsRequired = downloadStrategy == OdbcDownloadStrategy.ByModifiedTime;
            }

            // Have to remove and insert the entry because this is what is bound to the property changed trigger.
            // The entry itself must change, not just a property on it. This avoids having to add a property changed
            // handler to the ShipWorksMappableField
            Order.Entries.RemoveAt(lastModifiedEntryIndex);
            Order.Entries.Insert(lastModifiedEntryIndex, lastModifiedEntry);
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