using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiDownloader : StoreDownloader
    {
        public YahooApiDownloader(StoreEntity store) : base(store)
        {
        }

        public YahooApiDownloader(StoreEntity store, StoreType storeType) : base(store, storeType)
        {
        }

        protected override void Download()
        {
            Progress.Detail = "Checking for new orders...";

            try
            {
                CheckForNewOrders();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void CheckForNewOrders()
        {
            YahooApiWebClient client = new YahooApiWebClient(Store as YahooStoreEntity);

            long lastOrderNumber = GetOrderNumberStartingPoint();

            string response = client.GetOrderRange(lastOrderNumber + 1);

            SerializationUtility
        }

        /// <summary>
        /// Deserializes the response XML
        /// </summary>
        private static T DeserializeResponse<T>(string xml)
        {
            try
            {
                return SerializationUtility.DeserializeFromXml<T>(xml);
            }
            catch (InvalidOperationException ex)
            {
                if (xml.Contains("ErrorResponse"))
                {
                    YahooError errorResponse = SerializationUtility.DeserializeFromXml<YahooError>(xml);
                    throw new YahooException(errorResponse., ex);
                }

                throw new YahooException($"Error Deserializing {typeof(T).Name}", ex);
            }
        }
    }
}
