using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.ShopSite.Dto;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite.Responses
{
    /// <summary>
    /// Helper to get static responses
    /// </summary>
    public static class ShopSiteResponseHelper
    {
        public static string GetOrdersXml()
        {
            string ordersXml = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Stores.Tests.Platforms.ShopSite.Responses.GetOrders.xml"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ordersXml = reader.ReadToEnd();
                }
            }
            return ordersXml;
        }

        public static string GetTestConnectionXml()
        {
            string ordersXml = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Stores.Tests.Platforms.ShopSite.Responses.TestConnection.xml"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ordersXml = reader.ReadToEnd();
                }
            }
            return ordersXml;
        }

        public static string GetAccessTokenResponse()
        {
            string accessTokenJson = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Stores.Tests.Platforms.ShopSite.Responses.FetchAccessToken.json"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    accessTokenJson = reader.ReadToEnd();
                }
            }
            return accessTokenJson;
        }

        public static AccessResponse GetAccessTokenJson()
        {
            return JsonConvert.DeserializeObject<AccessResponse>(GetAccessTokenResponse());
        }

        public static string GetEmptyAccessTokenResponse()
        {
            string accessTokenJson = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Stores.Tests.Platforms.ShopSite.Responses.FetchEmptyAccessToken.json"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    accessTokenJson = reader.ReadToEnd();
                }
            }
            return accessTokenJson;
        }

        public static AccessResponse GetEmptyAccessTokenJson()
        {
            return JsonConvert.DeserializeObject<AccessResponse>(GetEmptyAccessTokenResponse());
        }
    }
}
