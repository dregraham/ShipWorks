using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;

namespace ShipWorks.Stores.UI.Platforms.Odbc
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
        void Load(IOdbcDataSource dataSource);

        /// <summary>
        /// Validates the required map settings.
        /// </summary>
        bool ValidateRequiredMapSettings();

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        void SaveMapSettings(OdbcStoreEntity store);
    }
}
