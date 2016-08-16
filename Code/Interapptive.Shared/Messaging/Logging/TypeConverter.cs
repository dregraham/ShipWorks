using System;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Convert an object to just a type name
    /// </summary>
    public class TypeConverter : JsonConverter
    {
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
            writer.WriteValue(value.GetType().FullName);
        }
    }
}
