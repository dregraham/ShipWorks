using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public interface IOdbcFieldMapWriter
	{
		void Write(Stream stream);
	}
}
