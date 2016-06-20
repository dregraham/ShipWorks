using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Contains data for an ODBCRecord
    /// </summary>
    public class OdbcRecord
    {
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the name of the record identifier column.
        /// </summary>
        public string RecordIdentifier { get; set; }

        /// <summary>
        /// Gets a value indicating whether this record has any values.
        /// </summary>
        public bool HasValues => fields.Any();

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