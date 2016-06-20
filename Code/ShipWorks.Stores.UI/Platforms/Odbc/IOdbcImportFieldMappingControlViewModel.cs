using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="OdbcImportFieldMappingControl"/>
    /// </summary>
    public interface IOdbcImportFieldMappingControlViewModel
    {
        /// <summary>
        /// Gets the data source.
        /// </summary>
        IOdbcDataSource DataSource { get; }

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
        IOdbcTable SelectedTable { get; set; }

        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        ObservableCollection<OdbcColumn> Columns { get; set; }

        /// <summary>
        /// List of field maps to be mapped.
        /// </summary>
        IEnumerable<OdbcFieldMapDisplay> FieldMaps { get; set; }

        /// <summary>
        /// The selected field map.
        /// </summary>
        OdbcFieldMapDisplay SelectedFieldMap { get; set; }

        /// <summary>
        /// The order field map.
        /// </summary>
        OdbcFieldMapDisplay OrderFieldMap { get; set; }

        /// <summary>
        /// The address field map.
        /// </summary>
        OdbcFieldMapDisplay AddressFieldMap { get; set; }

        /// <summary>
        /// The item field map.
        /// </summary>
        OdbcFieldMapDisplay ItemFieldMap { get; set; }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        void Load(IOdbcDataSource dataSource);

        /// <summary>
        /// Save the Map to the given store.
        /// </summary>
        void Save(OdbcStoreEntity store);

        /// <summary>
        /// Checks the required fields have value.
        /// </summary>
        bool EnsureRequiredFieldsHaveValue();
    }
}