using System.Collections.Generic;
using System.Windows.Input;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="OdbcImportFieldMappingDlg"/>
    /// </summary>
    public interface IOdbcImportFieldMappingDlgViewModel
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
        IEnumerable<OdbcColumn> Columns { get; set; }

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
        /// The save map command.
        /// </summary>
        ICommand SaveMapCommand { get; set; }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        void Load(IEnumerable<OdbcTable> tables);

    }
}