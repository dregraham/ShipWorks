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
        /// <param name="map"></param>
        public JsonOdbcFieldMapWriter(OdbcFieldMap map)
        {
            this.map = map;
        }

        /// <summary>
        /// Serializes and writes the ODBC Field Map to the given stream.
        /// </summary>
	    public void Write(Stream stream)
		{
            MethodConditions.EnsureArgumentIsNotNull(map);

            StreamWriter streamWriter = new StreamWriter(stream);
            string data = JsonConvert.SerializeObject(map, GetSerializerSettings());

            streamWriter.Write(data);
            streamWriter.Flush();
		}

        /// <summary>
        /// Json serializer settings to be used when serializing
        /// </summary>
        private JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
        }
	}
}
