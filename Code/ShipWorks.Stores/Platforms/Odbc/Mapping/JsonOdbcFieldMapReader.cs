using System;
using System.IO;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Facilitates reading the ODBC Field Map from JSON
    /// </summary>
    public class JsonOdbcFieldMapReader : IOdbcFieldMapReader
    {
        private readonly ILog log;
        private readonly JObject json;
        private readonly JToken mapEntries;
        private int position = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonOdbcFieldMapReader(Stream stream, ILog log)
        {
            MethodConditions.EnsureArgumentIsNotNull(stream);
            this.log = log;

            stream.Position = 0;
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string data = streamReader.ReadToEnd();
                try
                {
                    json = JObject.Parse(data);
                    mapEntries = json["Entries"];
                }
                catch (JsonReaderException ex)
                {
                    throw new ShipWorksOdbcException("Stream does not contain a valid Odbc Map.", ex);
                }
            }
        }

        /// <summary>
        /// Reads the Display Name from the stream
        /// </summary>
        public string ReadDisplayName()
        {
            try
            {
                return json["DisplayName"].ToString();
            }
            catch (Exception ex)
            {
                log.Error("Error parsing Display Name from map.", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Reads the External Table Name from the stream
        /// </summary>
        public string ReadExternalTableName()
        {
            try
            {
                return json["ExternalTableName"].ToString();
            }
            catch (Exception ex)
            {
                log.Error("Error parsing External Table Name from map.", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Reads the ODBC Field Map Entry from the stream
        /// </summary>
        public OdbcFieldMapEntry ReadEntry()
        {
            position++;
            try
            {
                JToken current = mapEntries[position];

                if (current != null)
                {
                    return JsonConvert.DeserializeObject<OdbcFieldMapEntry>(current.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                log.Error("Error Deserializing Field Map Entry.", ex);
                return null;
            }
        }
    }
}
