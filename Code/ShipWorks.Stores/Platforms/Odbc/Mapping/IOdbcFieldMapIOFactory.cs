namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public interface IOdbcFieldMapIOFactory
	{
		IOdbcFieldMapReader CreateReader(OdbcFieldMap map);

		IOdbcFieldMapWriter CreateWriter(OdbcFieldMap map);
	}
}
