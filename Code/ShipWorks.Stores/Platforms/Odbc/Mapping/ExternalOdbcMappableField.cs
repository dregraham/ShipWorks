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
	    public ExternalOdbcMappableField()
	    {
	    }
        public OdbcTable Table { get; set; }
        public OdbcColumn Column { get; set; }

	    public ExternalOdbcMappableField(IOdbcTable table, OdbcColumn column)
	    {
	        this.table = table;
	        this.column = column;
	    }

        /// <summary>
        /// The fields Qualified Name in the format Table.Column
        /// </summary>
	    public string GetQualifiedName()
	    {
	        return $"{Table.Name}.{Column.Name}";
	    }

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
