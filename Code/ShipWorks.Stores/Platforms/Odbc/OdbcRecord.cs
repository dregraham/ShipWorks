using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Contains data for an ODBCRecord
    /// </summary>
    public class OdbcRecord
    {
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the record identifier
        /// </summary>
        public string RecordIdentifier { get; set; }

        /// <summary>
        /// Gets the value of the field - null if not set
        /// </summary>
        public object GetValue(string fieldName)
        {
            object value;
            fields.TryGetValue(fieldName, out value);

            return value;
        }

        /// <summary>
        /// Adds or updates the value of the field.
        /// </summary>
        public void AddField(string columnName, object value)
        {
            fields[columnName] = value;
        }
    }
}