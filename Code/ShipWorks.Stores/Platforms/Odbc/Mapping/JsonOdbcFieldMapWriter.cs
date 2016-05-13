using System.IO;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Facilitates writing the ODBC Field Map in JSON
    /// </summary>
	public class JsonOdbcFieldMapWriter : IOdbcFieldMapWriter
	{
	    private readonly OdbcFieldMap map;

        /// <summary>
        /// Constructor
        /// </summary>
	    public JsonOdbcFieldMapWriter(OdbcFieldMap map)
	    {
	        MethodConditions.EnsureArgumentIsNotNull(map);
	        this.map = map;
	    }

        /// <summary>
        /// Serializes and writes the ODBC Field Map to the given stream.
        /// </summary>
	    public void Write(Stream stream)
		{
            StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.Write(JsonConvert.SerializeObject(map));
            streamWriter.Flush();
		}
	}
}
