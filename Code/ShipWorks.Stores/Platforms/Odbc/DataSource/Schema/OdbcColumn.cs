using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    /// <summary>
    /// Represents a column from an ODBC table
    /// </summary>
    [Obfuscation(Exclude = true)]
	public class OdbcColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
	    public OdbcColumn(string name, string dataType)
	    {
	        Name = name;
            DataType = dataType;
        }

        /// <summary>
        /// The column name
        /// </summary>
	    public string Name { get; }

        /// <summary>
        /// The column datatype
        /// </summary>
        public string DataType { get; }

        /// <summary>
        /// Determines whether the specified Object is equal to this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            OdbcColumn odbcColumnToCompare = obj as OdbcColumn;

            return odbcColumnToCompare != null && Name == odbcColumnToCompare.Name;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}
