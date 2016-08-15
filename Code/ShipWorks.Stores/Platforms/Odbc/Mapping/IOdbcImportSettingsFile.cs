using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Interface to Save and Load Import Settings
    /// </summary>
    public interface IOdbcImportSettingsFile : IOdbcSettingsFile
    {
        /// <summary>
        /// Gets or sets the ODBC import strategy.
        /// </summary>
        OdbcImportStrategy OdbcImportStrategy { get; set; }
    }
}