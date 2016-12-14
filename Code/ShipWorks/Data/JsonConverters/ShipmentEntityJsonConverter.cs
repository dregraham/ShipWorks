using System;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.JsonConverters
{
    /// <summary>
    /// Convert a shipment entity into a form that makes sense when logging
    /// </summary>
    public class ShipmentEntityJsonConverter : JsonConverter
    {
        /// <summary>
        /// Can the object type be converted
        /// </summary>
        public override bool CanConvert(Type objectType) => objectType == typeof(ShipmentEntity);

        /// <summary>
        /// Can this converter read data back
        /// </summary>
        public override bool CanRead => false;

        /// <summary>
        /// This converted does not support reading
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write the JSON for the shipment entity
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ShipmentEntity shipment = value as ShipmentEntity;
            if (shipment == null)
            {
                return;
            }

            JObject shipmentJson = new JObject();
            shipmentJson.Add("ShipmentID", shipment.ShipmentID);
            shipmentJson.Add("OrderID", shipment.OrderID);
            shipmentJson.Add("ShipmentType", shipment.ShipmentTypeCode.ToString());
            shipmentJson.Add("RowVersion", shipment.RowVersion.ToHexString());
            shipmentJson.WriteTo(writer);
        }
    }
}
