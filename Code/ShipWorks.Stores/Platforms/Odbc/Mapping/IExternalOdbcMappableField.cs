using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Provides a mechanism for interacting with an external ODBC field
    /// </summary>
    public interface IExternalOdbcMappableField : IOdbcMappableField
    {
        /// <summary>
        /// The External Column
        /// </summary>
        OdbcColumn Column { get; set; }

        /// <summary>
        /// Loads the given record
        /// </summary>
        void LoadValue(OdbcRecord record);

        /// <summary>
        /// Resets the value
        /// </summary>
        void ResetValue();
    }
}