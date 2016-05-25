using System;
using System.IO;
using System.Linq;
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
        private int entryPosition;

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonOdbcFieldMapReader(Stream stream, ILog log)
        {
            MethodConditions.EnsureArgumentIsNotNull(stream);
            this.log = log;

            stream.Position = 0;
            try
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    string data = streamReader.ReadToEnd();

                    json = JObject.Parse(data);
                    mapEntries = json["Entries"];

                }
            }
            catch (Exception ex)
            {
                Type exType = ex.GetType();
                log.Error("Error reading map", ex);

                // If its an error throw while reading the stream or parsing the json
                // rethrow as a ShipWorksOdbcException.
                if (exType == typeof(JsonReaderException) ||
                    exType == typeof(ArgumentException) ||
                    exType == typeof(IOException))
                {
                    throw new ShipWorksOdbcException("ShipWorks was unable to read the ODBC Map.", ex);
                }

                // this exception is unexpected so we throw.
                throw;
            }
            // Sent the entry position used when reading the Odbc field map entries
            entryPosition = 0;
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
            OdbcFieldMapEntry result = null;
            try
            {
                if (mapEntries.ElementAtOrDefault(entryPosition) != null)
                {
                    result = JsonConvert.DeserializeObject<OdbcFieldMapEntry>(mapEntries[entryPosition].ToString());
                }

                entryPosition++;
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Error Deserializing Field Map Entry.", ex);
                return null;
            }
        }
    }
}
