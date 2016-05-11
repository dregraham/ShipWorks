using System;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Provides a mechanism for interacting with an external ODBC field
    /// </summary>
	public class ExternalOdbcMappableField : IOdbcMappableField
	{
	    private readonly IOdbcTable table;
	    private readonly OdbcColumn column;

        /// <summary>
        /// Constructor
        /// </summary>
	    public ExternalOdbcMappableField(IOdbcTable table, OdbcColumn column)
	    {
	        this.table = table;
	        this.column = column;
	    }

        /// <summary>
        /// The fields Qualified Name in the format Table.Column
        /// </summary>
	    public string QualifiedName => $"{table.Name}.{column.Name}";

        /// <summary>
        /// value from the field
        /// </summary>
	    public string Value
		{
			get
			{
				throw new NotImplementedException();
			}
		}

        /// <summary>
        /// The fields display name
        /// </summary>
	    public string DisplayName => $"{table.Name} {column.Name}";
    }
}
