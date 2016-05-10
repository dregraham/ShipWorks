using System;
using System.Collections.Generic;
using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public class OdbcFieldMap
	{
		private IOdbcFieldMapIOFactory ioFactory;
		List<OdbcFieldMapEntry> entries;

		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{
			throw new NotImplementedException();
		}

		public void AddEntry(OdbcFieldMapEntry entry)
		{
			throw new NotImplementedException();
		}

		public void Load(Stream stream)
		{
			throw new NotImplementedException();
		}

		public void Save(Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
