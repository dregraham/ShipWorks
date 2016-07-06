using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public interface IOdbcMapSettingsControlViewModel
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
        IEnumerable<IOdbcColumnSource> ColumnSources { get; set; }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        IOdbcColumnSource SelectedTable { get; set; }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        void Load(IOdbcDataSource dataSource);
        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        bool IsTableSelected { get; set; }

        /// <summary>
        /// Whether the download strategy is last modified.
        /// </summary>
        bool IsDownloadStrategyLastModified { get; set; }

        ICommand ExecuteQueryCommand { get; set; }
        DataTable QueryResults { get; set; }
        string ResultMessage { get; set; }



        bool ValidateRequiredMapSettings();
    }
}
