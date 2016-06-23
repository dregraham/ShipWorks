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
        public void AddField(string columnName, object value)
        {
            fields[columnName] = value;
        }

        /// <summary>
        /// Cleanses this instance.
        /// </summary>
        public void Cleanse()
        {
            fields
                .Where(field => IsFieldValueNullOrEmpty(field.Value))
                .Select(field => field.Key)
                .ToList()
                .ForEach(key => fields.Remove(key));
        }

        /// <summary>
        /// Determines whether the field value is DBNull, null, or empty string.
        /// </summary>
        private static bool IsFieldValueNullOrEmpty(object value)
        {
            return value == null || value is DBNull || (value is string && string.IsNullOrWhiteSpace((string) value));
        }
    }
}