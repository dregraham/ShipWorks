using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class DeserializerTest
    {
        private string xml;

        public DeserializerTest ()
        {
            xml = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.Yahoo.Artifacts.YahooOrderResponse.xml");
        }

        [Fact]
        public void Test()
        {
            XmlDeserializer deserializer = new XmlDeserializer();

            YahooResponseResourceList responseList = (YahooResponseResourceList) deserializer.Deserialize(typeof (YahooResponseResourceList), xml);

            Assert.Equal(responseList.OrderListQuery.YahooOrders.FirstOrDefault().Currency.Replace(" ", ""), "USD");
        }



        private string GetEmbeddedResourceJson(string embeddedResourceName)
        {
            string txt = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        txt = reader.ReadToEnd();
                    }
            }

            return txt;
        }
    }
}
