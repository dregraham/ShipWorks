using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// View Model for UploadMAppingControl
    /// </summary>
    public class OdbcUploadMappingControlViewModel : INotifyPropertyChanged, IOdbcUploadMappingControlViewModel
    {
        private readonly IOdbcFieldMapFactory fieldMapFactory;
        private readonly IMessageHelper messageHelper;
        private OdbcFieldMapDisplay selectedFieldMap;
        private readonly PropertyChangedHandler handler;
        private ObservableCollection<OdbcColumn> columns;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadMappingControlViewModel"/> class.
        /// </summary>
        public OdbcUploadMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory, IMessageHelper messageHelper)
        {
            this.fieldMapFactory = fieldMapFactory;
            this.messageHelper = messageHelper;

            Shipment = new OdbcFieldMapDisplay("Shipment", fieldMapFactory.CreateShipmentFieldMap());
            ShipmentAddress = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateShiptoAddressFieldMap());
            selectedFieldMap = Shipment;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The column source.
        /// </summary>
        public IOdbcColumnSource ColumnSource { get; private set; }

        /// <summary>
        /// Gets or sets the shipment map
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcFieldMapDisplay Shipment { get; set; }

        /// <summary>
        /// Gets or sets the shipment address map.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcFieldMapDisplay ShipmentAddress { get; set; }

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
        /// The columns from the selected external odbc table.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<OdbcColumn> Columns
        {
            get { return columns; }
            set { handler.Set(nameof(Columns), ref columns, value); }
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
                    store.UploadMap = reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Build a single ODBC Field Map from the Order Address and Item Field Maps
        /// </summary>
        private OdbcFieldMap CreateMap()
        {
            List<IOdbcFieldMapEntry> mapEntries = Shipment.Entries.ToList();
            mapEntries.AddRange(ShipmentAddress.Entries);

            OdbcFieldMap map = fieldMapFactory.CreateFieldMapFrom(mapEntries);

            IEnumerable<IOdbcFieldMapEntry> entries = map.FindEntriesBy(OrderFields.OrderNumber);
            map.RecordIdentifierSource = entries.FirstOrDefault()?.ExternalField.Column.Name;

            return map;
        }

        /// <summary>
        /// </summary>
        public bool ValidateRequiredMappingFields()
        {
            // get a list of all of the mapped columns
            List<IOdbcFieldMapEntry> mappedExternalColumns =
                Shipment.Entries.Where(
                    e => !e.ExternalField.Column?.Name.Equals("(None)", StringComparison.InvariantCulture) ?? false)
                    .ToList();

            mappedExternalColumns.AddRange(ShipmentAddress.Entries.Where(
                e => !e.ExternalField.Column?.Name.Equals("(None)", StringComparison.InvariantCulture) ?? false));

            // see if any of the external columns are duplicated
            string [] duplicateColumns = mappedExternalColumns.GroupBy(e => e.ExternalField.Column.Name)
                .SelectMany(g => g.Skip(1))
                .Select(e => e.ExternalField.Column.Name)
                .Distinct()
                .Select(d => "    - " + d)
                .ToArray();

            if (duplicateColumns.Any())
            {
                string displayName = string.Join(Environment.NewLine, duplicateColumns);

                messageHelper.ShowError($"Each column can only be mapped once. The following columns are mapped more than once:{Environment.NewLine}{displayName}");
                return false;
            }

            // Finds an entry that is required and the external column has not been set
            IEnumerable<IOdbcFieldMapEntry> unsetRequiredFieldMapEntries =
               Shipment.Entries.Where(
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

            return true;
        }
    }
}