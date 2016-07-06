using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="WizardPages.OdbcImportFieldMappingControl" />
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
        IEnumerable<IOdbcColumnSource> ColumnSources { get; set; }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        IOdbcColumnSource SelectedTable { get; set; }

        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        ObservableCollection<OdbcColumn> Columns { get; set; }

        /// <summary>
        /// The selected field map.
        /// </summary>
        OdbcFieldMapDisplay SelectedFieldMap { get; set; }

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
        bool ValidateRequiredMappingFields();

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

        bool IsQueryValid { get; set; }

        void LoadColumns();

        bool ValidateRequiredMapSettings();
    }
}