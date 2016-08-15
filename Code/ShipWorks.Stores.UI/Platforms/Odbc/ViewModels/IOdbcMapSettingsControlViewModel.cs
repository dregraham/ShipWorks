using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    public interface IOdbcMapSettingsControlViewModel
    {
        /// <summary>
        /// Gets the data source.
        /// </summary>
        IOdbcDataSource DataSource { get; }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        void Load(IOdbcDataSource dataSource, IOdbcSchema odbcSchema, string columnSourceFromStore, OdbcStoreEntity store);

        /// <summary>
        /// Validates the required map settings.
        /// </summary>
        bool ValidateRequiredMapSettings();

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        void SaveMapSettings(OdbcStoreEntity store);

        /// <summary>
        /// Loads the map settings.
        /// </summary>
        void LoadMapSettings(OdbcStoreEntity store);
    }
}
