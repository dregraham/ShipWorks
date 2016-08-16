using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Serialize an object as a type name and a reference id
    /// </summary>
    public class ReferenceConverter : JsonConverter
    {
        public static ObjectIDGenerator ObjectIDGen = new ObjectIDGenerator();

        /// <summary>
        /// Can the specified object be converted
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read JSON
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write the object as JSON
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            bool firstTime;
            var objectId = ObjectIDGen.GetId(value, out firstTime);

            writer.WriteStartObject();
            writer.WritePropertyName("Type");
            writer.WriteValue(value.GetType().FullName);

            writer.WritePropertyName("Reference");
            writer.WriteValue(objectId);

            writer.WriteEndObject();
        }
    }
}