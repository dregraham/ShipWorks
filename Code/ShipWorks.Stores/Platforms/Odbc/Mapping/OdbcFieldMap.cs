using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public class OdbcFieldMap
	{
		private readonly IOdbcFieldMapIOFactory ioFactory;

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

        /// <summary>
        /// Loads the ODBC Field Map from the given stream
        /// </summary>
        /// <param name="stream"></param>
		public void Load(Stream stream)
		{
		    IOdbcFieldMapReader reader = ioFactory.CreateReader(stream);

            OdbcFieldMapEntry entry;
            while ((entry = reader.ReadEntry()) != null)
            {
                AddEntry(entry);
            }

            DisplayName = reader.ReadDisplayName();
            ExternalTableName = reader.ReadExternalTableName();
		}

        /// <summary>
        /// Writes the ODBC Field Map to the given stream
        /// </summary>
        /// <param name="stream"></param>
		public void Save(Stream stream)
		{
		    IOdbcFieldMapWriter writer = ioFactory.CreateWriter();
            writer.Write(this, stream);
		}
	}
}
