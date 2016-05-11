using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public class ShipWorksOdbcMappableField : IOdbcMappableField
	{
	    private string displayName;

	    public ShipWorksOdbcMappableField(EntityField2 field)
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
