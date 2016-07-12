using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    public class OdbcUploadMappingControlViewModel: INotifyPropertyChanged, IOdbcUploadMappingControlViewModel
    {
        private readonly IOdbcFieldMapFactory fieldMapFactory;
        private OdbcFieldMapDisplay selectedFieldMap;
        private readonly PropertyChangedHandler handler;
        private ObservableCollection<OdbcColumn> columns;
        public event PropertyChangedEventHandler PropertyChanged;



        public OdbcUploadMappingControlViewModel(IOdbcFieldMapFactory fieldMapFactory)
        {
            this.fieldMapFactory = fieldMapFactory;

            Shipment = new OdbcFieldMapDisplay("Shipment", fieldMapFactory.CreateShipmentFieldMap());
            ShipmentAddress = new OdbcFieldMapDisplay("Address", fieldMapFactory.CreateShiptoAddressFieldMap());

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

        }

        /// <summary>
        /// The column source.
        /// </summary>
        public IOdbcColumnSource ColumnSource { get; private set; }

        /// <summary>
        /// Gets or sets the shipment map
        /// </summary>
        public OdbcFieldMapDisplay Shipment { get; set; }

        /// <summary>
        /// Gets or sets the shipment address map.
        /// </summary>
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
           throw new NotImplementedException();
        }

    }
}
