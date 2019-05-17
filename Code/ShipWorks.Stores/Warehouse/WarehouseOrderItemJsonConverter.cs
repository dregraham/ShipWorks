using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Amazon.Warehouse;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    public class WarehouseOrderItemJsonConverter : JsonConverter
    {
        private readonly StoreTypeCode storeType;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseOrderItemJsonConverter(StoreTypeCode storeType)
        {
            this.storeType = storeType;
        }

        /// <summary>
        /// Returns true if object is of type WarehouseOrder
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(WarehouseOrderItem);
        }

        /// <summary>
        /// Reads the json and converts it the correct order object
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject itemJObject = JObject.Load(reader);
            var itemString = itemJObject.ToString();
            return CreateWarehouseItem(itemString, storeType);
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

        /// <summary>
        /// Create a warehouse order from order json
        /// </summary>
        private static WarehouseOrderItem CreateWarehouseItem(string itemJson, StoreTypeCode storeType)
        {
            switch (storeType)
            {
                case StoreTypeCode.Amazon:
                    return JsonConvert.DeserializeObject<AmazonWarehouseItem>(itemJson);
                case StoreTypeCode.ChannelAdvisor:
                    return JsonConvert.DeserializeObject<ChannelAdvisorWarehouseItem>(itemJson);
                default:
                    return JsonConvert.DeserializeObject<WarehouseOrderItem>(itemJson);
            }
        }
    }
}
