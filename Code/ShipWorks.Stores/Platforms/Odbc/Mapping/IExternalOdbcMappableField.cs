using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    public interface IExternalOdbcMappableField : IOdbcMappableField
    {
        /// <summary>
        /// The External Column
        /// </summary>
        [Obfuscation(Exclude = true)]
        OdbcColumn Column { get; set; }

        /// <summary>
        /// The External Table
        /// </summary>
        IOdbcTable Table { get; set; }

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