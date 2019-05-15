using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Json converter for warehouse orders
    /// </summary>
    public class WarehouseOrderJsonConverter : JsonConverter
    {
        /// <summary>
        /// Returns true if object is of type WarehouseOrder
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(WarehouseOrder);
        }

        /// <summary>
        /// Reads the json and converts it the correct order object
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject orderJObject = JObject.Load(reader);
            var orderString = orderJObject.ToString();
            return WarehouseOrderFactory.CreateOrder(orderString);
        }

        /// <summary>
        /// Cannot Write
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// Not supported
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}