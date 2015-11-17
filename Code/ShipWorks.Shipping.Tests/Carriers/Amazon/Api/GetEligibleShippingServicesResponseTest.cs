using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using System.IO;
using System.Reflection;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    public class GetEligibleShippingServicesResponseTest
    {
        [Fact]
        public void Deserialize_GetEligibleShippingServicesResponse()
        {
            string successfulResponseXml = GetEmbeddedResourceXml("ShipWorks.Shipping.Tests.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml");
            SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(successfulResponseXml);
        }

        [Fact]
        public void Deserialize_ShippingService()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string successfulResponseXml = GetEmbeddedResourceXml("ShipWorks.Shipping.Tests.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml");

                GetEligibleShippingServicesResponse response = SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(successfulResponseXml);

                ShippingServiceList serviceList = response.GetEligibleShippingServicesResult.ShippingServiceList;

                Assert.Equal(3, serviceList.ShippingService.Count);
            }
        }

        private string GetEmbeddedResourceXml(string embeddedResourceName)
        {
            string xml = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }

            return xml;
        }

    }
}
