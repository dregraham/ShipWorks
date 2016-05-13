using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public class OdbcFieldMap
	{
		private IOdbcFieldMapIOFactory ioFactory;

		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{
		    MethodConditions.EnsureArgumentIsNotNull(ioFactory);

		    this.ioFactory = ioFactory;
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
		    IOdbcFieldMapReader reader = ioFactory.CreateReader(this);
		    reader.ReadEntry();
		}

		public void Save(Stream stream)
		{
		    IOdbcFieldMapWriter writer = ioFactory.CreateWriter(this);
            writer.Write(stream);
		}
	}
}
