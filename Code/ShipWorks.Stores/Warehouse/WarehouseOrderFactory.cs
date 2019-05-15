using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Amazon.Warehouse;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Factory for creating store specific warehouse orders from json
    /// </summary>
    public static class WarehouseOrderFactory
    {
        /// <summary>
        /// Create an order from orderData
        /// </summary>
        public static WarehouseOrder CreateOrder(string orderData)
        {
            StoreTypeCode? storeType = GetStoreType(orderData);

            switch (storeType)
            {
                case StoreTypeCode.Amazon:
                    return JsonConvert.DeserializeObject<AmazonWarehouseOrder>(orderData);
                case StoreTypeCode.ChannelAdvisor:
                    return JsonConvert.DeserializeObject<ChannelAdvisorWarehouseOrder>(orderData);
                default:
                    return JsonConvert.DeserializeObject<WarehouseOrder>(orderData);
            }
        }

        /// <summary>
        /// Get the datas storetype
        /// </summary>
        /// <returns>null if we cant determine the storetype</returns>
        private static StoreTypeCode? GetStoreType(string orderData)
        {
            JObject orderObject = JObject.Parse(orderData);

            string storeTypeString = orderObject.GetValue("StoreType")?.ToString();

            if (int.TryParse(storeTypeString, out int storeType))
            {
                return (StoreTypeCode)storeType;
            }

            return null;
        }
    }
}