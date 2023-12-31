using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using System.Reflection;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Provides a mechanism for interacting with an external ODBC field
    /// </summary>
    [Obfuscation(Exclude = true)]
	public class ExternalOdbcMappableField : IExternalOdbcMappableField
    {
        /// <summary>
        /// Constructor
        /// </summary>
	    public ExternalOdbcMappableField(OdbcColumn column)
	    {
	        Column = column;
	    }

        /// <summary>
        /// The External Column
        /// </summary>
        public OdbcColumn Column { get; set; }

        /// <summary>
        /// value from the field
        /// </summary>
        [JsonIgnore]
        public object Value { get; private set; }

        /// <summary>
        /// The fields display name
        /// </summary>
        [JsonIgnore]
        public string DisplayName => $"{Column.Name}";

        /// <summary>
        /// The fields Qualified Name in the format Table.Column
        /// </summary>
        [JsonIgnore]
	    public string QualifiedName => $"{Column.Name}";

        /// <summary>
        /// Loads the given record
        /// </summary>
        /// <remarks>
        /// sets Value by getting it from the record using the external column name
        /// </remarks>
        public void LoadValue(OdbcRecord record)
        {
            MethodConditions.EnsureArgumentIsNotNull(record, nameof(record));
            Value = record.GetValue(Column.Name);
        }

        /// <summary>
        /// Resets the value
        /// </summary>
        /// <remarks>
        /// resetting via a method lets us keep the setter of Value private
        /// this ensures that when we deserializes it does not get set
        /// </remarks>
        public void ResetValue()
        {
            Value = null;
        }
    }
}
