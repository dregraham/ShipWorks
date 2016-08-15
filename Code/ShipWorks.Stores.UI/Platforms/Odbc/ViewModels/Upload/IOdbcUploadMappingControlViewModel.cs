using System.Collections.ObjectModel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Upload
{
    /// <summary>
    /// ViewModel for the OdbcUploadMappingControl
    /// </summary>
    public interface IOdbcUploadMappingControlViewModel
    {
        /// <summary>
        /// The column source.
        /// </summary>
        IOdbcColumnSource ColumnSource { get; }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        OdbcFieldMapDisplay Shipment { get; set; }

        /// <summary>
        /// Gets or sets the shipment address.
        /// </summary>
        OdbcFieldMapDisplay ShipmentAddress { get; set; }

        /// <summary>
        /// The selected field map.
        /// </summary>
        OdbcFieldMapDisplay SelectedFieldMap { get; set; }

        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        ObservableCollection<OdbcColumn> Columns { get; set; }
        
        /// <summary>
        /// Saves the map.
        /// </summary>
        void Save(OdbcStoreEntity store);

        /// <summary>
        /// Loads the map.
        /// </summary>
        void Load(OdbcStoreEntity store);

        /// <summary>
        /// Validates the required mapping fields.
        /// </summary>
        bool ValidateRequiredMappingFields();
    }
}