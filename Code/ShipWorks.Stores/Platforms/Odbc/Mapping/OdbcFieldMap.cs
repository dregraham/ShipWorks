using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains Field Mapping information for ODBC
    /// </summary>
	public class OdbcFieldMap
	{
		private readonly IOdbcFieldMapIOFactory ioFactory;

        /// <summary>
        /// Constructor
        /// </summary>
		public OdbcFieldMap(IOdbcFieldMapIOFactory ioFactory)
		{
		    MethodConditions.EnsureArgumentIsNotNull(ioFactory);

		    this.ioFactory = ioFactory;
		    Entries = new List<OdbcFieldMapEntry>();
		}

        /// <summary>
        /// The ODBC Field Map Entries
        /// </summary>
	    public List<OdbcFieldMapEntry> Entries { get; }

        /// <summary>
        /// The Display Name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// The External Table Name
        /// </summary>
        public string ExternalTableName { get; set; }

        /// <summary>
        /// Add the given ODBC Field Map Entry to the ODBC Field Map
        /// </summary>
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
