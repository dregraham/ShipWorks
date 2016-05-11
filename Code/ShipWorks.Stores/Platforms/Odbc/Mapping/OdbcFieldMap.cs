using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public class OdbcFieldMap
	{
		private IOdbcFieldMapIOFactory ioFactory;

		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{
            Entries = new List<OdbcFieldMapEntry>();
		}

        public List<OdbcFieldMapEntry> Entries { get; }

        [Obfuscation(Exclude = true)]
        public string DisplayName { get; set; }

        public string ExternalTableName { get; set; }

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
