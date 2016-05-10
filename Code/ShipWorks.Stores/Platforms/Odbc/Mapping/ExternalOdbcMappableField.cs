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

	    public string GetQualifiedName()
	    {
	        throw new NotImplementedException();
	    }

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
