using System;
using System.Collections.Generic;
using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public class OdbcFieldMap
	{
		private IOdbcFieldMapIOFactory ioFactory;

		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{

		}

        public List<OdbcFieldMapEntry> Entries { get; }

        public string DisplayName { get; set; }

        public void AddEntry(OdbcFieldMapEntry entry)
		{
			Entries.Add(entry);
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
