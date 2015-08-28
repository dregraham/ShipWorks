using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    [TestClass]
    public class GetEligibleShippingServicesResponseTest
    {
        [TestMethod]
        public void Deserialize_GetEligibleShippingServicesResponse()
        {
            string successfulResponseXml = GetEmbeddedResourceXml("ShipWorks.Tests.Shipping.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml");
            SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(successfulResponseXml);
        }

        [TestMethod]
        public void Deserialize_ShippingService()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string successfulResponseXml = GetEmbeddedResourceXml("ShipWorks.Tests.Shipping.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml");

                GetEligibleShippingServicesResponse response = SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(successfulResponseXml);

                ShippingServiceList serviceList = response.GetEligibleShippingServicesResult.ShippingServiceList;

                Assert.AreEqual(3, serviceList.ShippingService.Count);
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
