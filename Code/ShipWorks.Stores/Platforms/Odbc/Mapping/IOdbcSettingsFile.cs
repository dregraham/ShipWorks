using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Interface to Save and Load Odbc Settings
    /// </summary>
    public interface IOdbcSettingsFile
    {
        /// <summary>
        /// The action to perform on this file (Import/Upload)
        /// </summary>
        string Action { get; }

        /// <summary>
        /// The column source.
        /// </summary>
        string ColumnSource { get; set; }

        /// <summary>
        /// The type of the column source.
        /// </summary>
        OdbcColumnSourceType ColumnSourceType { get; set; }

        /// <summary>
        /// The file extension.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Gets the filter value.
        /// </summary>
        string Filter { get; }

        /// <summary>
        /// The ODBC field map.
        /// </summary>
        IOdbcFieldMap OdbcFieldMap { get; set; }

        /// <summary>
        /// Opens the load file dialog to load the map
        /// </summary>
        void Open(TextReader reader);

        /// <summary>
        /// Opens the save file dialog to save the map
        /// </summary>
        void Save(TextWriter textWriter);
    }
}