using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Provides a mechanism for interacting with an external ODBC field
    /// </summary>
	public class ExternalOdbcMappableField : IOdbcMappableField
	{
        /// <summary>
        /// Constructor
        /// </summary>
	    public ExternalOdbcMappableField(IOdbcTable table, OdbcColumn column)
	    {
	        Table = table;
	        Column = column;
	    }

        /// <summary>
        /// Constructor - used by NewtonSoft deserialization
        /// </summary>
        [JsonConstructor]
        public ExternalOdbcMappableField(OdbcTable table, OdbcColumn column)
        {
            Table = table;
            Column = column;
        }

        /// <summary>
        /// The External Table
        /// </summary>
        public IOdbcTable Table { get; set; }

        /// <summary>
        /// The External Column
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcColumn Column { get; set; }

        /// <summary>
        /// value from the field
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The fields display name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DisplayName => $"{Table.Name} {Column.Name}";

        /// <summary>
        /// The fields Qualified Name in the format Table.Column
        /// </summary>
	    public string GetQualifiedName()
	    {
	        return $"{Table.Name}.{Column.Name}";
	    }
    }
}
