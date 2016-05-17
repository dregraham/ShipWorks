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
        /// <summary>
        /// Serializes and writes the ODBC Field Map to the given stream.
        /// </summary>
	    public void Write(OdbcFieldMap map, Stream stream)
		{
            MethodConditions.EnsureArgumentIsNotNull(map);

            StreamWriter streamWriter = new StreamWriter(stream);
            string data = JsonConvert.SerializeObject(map, GetSerializerSettings());

            streamWriter.Write(data);
            streamWriter.Flush();
		}

        private JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
        }
	}
}
