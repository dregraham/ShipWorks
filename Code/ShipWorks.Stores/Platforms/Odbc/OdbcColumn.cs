using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc
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
	    public string Name { get; set; }
	}
}
