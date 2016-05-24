using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="OdbcImportFieldMappingControl"/>
    /// </summary>
    public interface IOdbcImportFieldMappingControlViewModel
    {
        /// <summary>
        /// The name the map will be saved as.
        /// </summary>
        string MapName { get; set; }

        /// <summary>
        /// The external odbc tables.
        /// </summary>
        IEnumerable<OdbcTable> Tables { get; set; }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        OdbcTable SelectedTable { get; set; }

        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        ObservableCollection<OdbcColumn> Columns { get; set; }

        /// <summary>
        /// List of field maps to be mapped.
        /// </summary>
        IEnumerable<OdbcFieldMap> FieldMaps { get; set; }

        /// <summary>
        /// The selected field map.
        /// </summary>
        OdbcFieldMap SelectedFieldMap { get; set; }

        /// <summary>
        /// The order field map.
        /// </summary>
        OdbcFieldMap OrderFieldMap { get; set; }

        /// <summary>
        /// The address field map.
        /// </summary>
        OdbcFieldMap AddressFieldMap { get; set; }

        /// <summary>
        /// The item field map.
        /// </summary>
        OdbcFieldMap ItemFieldMap { get; set; }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        void Load(OdbcStoreEntity store);

        /// <summary>
        /// Save the Map to the given store.
        /// </summary>
        void Save(OdbcStoreEntity store);
    }
}