using System;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.JsonConverters
{
    /// <summary>
    /// Convert an order entity into a form that makes sense when logging
    /// </summary>
    public class StoreEntityJsonConverter : JsonConverter
    {
        /// <summary>
        /// Can the object type be converted
        /// </summary>
        public override bool CanConvert(Type objectType) => objectType == typeof(StoreEntity);

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
        /// Write the JSON for the order entity
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            StoreEntity store = value as StoreEntity;
            if (store == null)
            {
                return;
            }

            JObject storeJson = new JObject();
            storeJson.Add("StoreID", store.StoreID);
            storeJson.Add("OrderID", store.StoreName);
            storeJson.Add("StoreType", store.StoreTypeCode.ToString());
            storeJson.Add("RowVersion", store.RowVersion.ToHexString());
            storeJson.WriteTo(writer);
        }
    }
}
