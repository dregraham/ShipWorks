namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public interface IOdbcMappableField
	{
		string GetQualifiedName();

		string Value
		{
			get;
		}

		string DisplayName
		{
			get;
		}
	}
}
