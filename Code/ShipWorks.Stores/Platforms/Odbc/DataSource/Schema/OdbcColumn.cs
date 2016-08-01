using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    /// <summary>
    /// Represents a column from an ODBC table
    /// </summary>
	public class OdbcColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
	    public OdbcColumn(string name)
	    {
	        Name = name;
	    }

        /// <summary>
        /// The column name
        /// </summary>
        [Obfuscation(Exclude = true)]
	    public string Name { get; }

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
