using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using System;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Facilitates writing the ODBC Field Map in JSON
    /// </summary>
	public class JsonOdbcFieldMapSerializer : IOdbcFieldMapSerializer
	{
        private readonly OdbcFieldMap map;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map"></param>
        public JsonOdbcFieldMapSerializer(OdbcFieldMap map)
        {
            MethodConditions.EnsureArgumentIsNotNull(map);
            this.map = map;
        }

        /// <summary>
        /// Serializes the ODBC Field Map
        /// </summary>
        public string Serialize()
        {
            try
            {
                return JsonConvert.SerializeObject(map, GetSerializerSettings());
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
