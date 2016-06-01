using System;
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
            MethodConditions.EnsureArgumentIsNotNull(map);
            this.map = map;
        }

        /// <summary>
        /// Serializes and writes the ODBC Field Map to the given stream.
        /// </summary>
	    public void Write(Stream stream)
		{
            MethodConditions.EnsureArgumentIsNotNull(stream);

            StreamWriter streamWriter = new StreamWriter(stream);
            try
            {
                string data = JsonConvert.SerializeObject(map, GetSerializerSettings());

                streamWriter.Write(data);
                streamWriter.Flush();
            }
            catch (Exception)
            {
                throw new ShipWorksOdbcException("Failed to save the Odbc import field map");
            }
		}

        /// <summary>
        /// Gets the serializer settings.
        /// </summary>
        private JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
        }
	}
}
