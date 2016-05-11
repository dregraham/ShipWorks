using System;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public class ExternalOdbcMappableField : IOdbcMappableField
	{
	    private string displayName;

	    public ExternalOdbcMappableField(OdbcTable table, OdbcColumn column)
		{
			throw new NotImplementedException();
		}

	    public string QualifiedName { get; }

	    public string Value
		{
			get
			{
				throw new NotImplementedException();
			}
		}

	    public string DisplayName
	    {
	        get { return displayName; }
	    }
	}
}
