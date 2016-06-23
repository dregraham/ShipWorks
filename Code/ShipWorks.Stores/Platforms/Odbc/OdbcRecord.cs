using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Contains data for an ODBCRecord
    /// </summary>
    public class OdbcRecord
    {
        private readonly string recordIdentifierSource;
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcRecord(string recordIdentifierSource)
        {
            this.recordIdentifierSource = recordIdentifierSource;
        }

        /// <summary>
        /// Gets the Record Identifier.
        /// </summary>
        /// <value>
        /// Null if cannot get value.
        /// </value>
        public string RecordIdentifier => GetValue(recordIdentifierSource)?.ToString();

        /// <summary>
		/// Gets a value indicating whether this record has values.
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
        /// <remarks>
        /// If value is DBNull or whitespace, column value is set to null.
        /// </remarks>
        public void AddField(string columnName, object value)
        {
            fields[columnName] = value is DBNull || (value is string && string.IsNullOrWhiteSpace((string) value)) ?
                null :
                value;
        }
    }
}