//using System;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using ShipWorks.Stores.Platforms.Amazon.Warehouse;
//using ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse;
//using ShipWorks.Warehouse.DTO.Orders;

//namespace ShipWorks.Stores.Warehouse
//{
//    /// <summary>
//    /// Json converter for warehouse orders
//    /// </summary>
//    public class WarehouseOrderJsonConverter : JsonConverter
//    {
//        private readonly StoreTypeCode storeType;

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public WarehouseOrderJsonConverter(StoreTypeCode storeType)
//        {
//            this.storeType = storeType;
//        }

//        /// <summary>
//        /// Returns true if object is of type WarehouseOrder
//        /// </summary>
//        public override bool CanConvert(Type objectType)
//        {
//            return objectType == typeof(WarehouseOrder);
//        }

//        /// <summary>
//        /// Reads the json and converts it the correct order object
//        /// </summary>
//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            JObject orderJObject = JObject.Load(reader);
//            var orderString = orderJObject.ToString();
//            return CreateWarehouseOrder(orderString);
//        }

//        /// <summary>
//        /// Cannot Write
//        /// </summary>
//        public override bool CanWrite => false;

//        /// <summary>
//        /// Not supported
//        /// </summary>
//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            throw new NotSupportedException();
//        }

//        /// <summary>
//        /// Create a warehouse order from order json
//        /// </summary>
//        private WarehouseOrder CreateWarehouseOrder(string orderJson)
//        {
//            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
//            {
//                Converters = {new WarehouseOrderItemJsonConverter(storeType)}
//            };

//            switch (storeType)
//            {
//                case StoreTypeCode.Amazon:
//                    return JsonConvert.DeserializeObject<AmazonWarehouseOrder>(orderJson, serializerSettings);
//                case StoreTypeCode.ChannelAdvisor:
//                    return JsonConvert.DeserializeObject<ChannelAdvisorWarehouseOrder>(orderJson, serializerSettings);
//                default:
//                    return JsonConvert.DeserializeObject<WarehouseOrder>(orderJson, serializerSettings);
//            }
//        }
//    }
//}
