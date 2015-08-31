using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    [TestClass]
    public class AmazonRateTests
    {
        [TestMethod]
        public void GetRates_SendsShipmentWeight_ToAmazonApi()
        {
            VerifyApiRequest((s, o) => s.TotalWeight = 1.23, x => x.Weight == 1.23);
        }

        [TestMethod]
        public void GetRates_SendsShipmentDimensions_ToAmazonApi()
        {
            VerifyApiRequest((s, o) =>
            {
                s.Amazon.DimsHeight = 3;
                s.Amazon.DimsLength = 4;
                s.Amazon.DimsWidth = 2;
            }, 
            x => x.PackageDimensions.Height == 3 &&
                x.PackageDimensions.Length == 4 &&
                x.PackageDimensions.Width == 2);
        }

        [TestMethod]
        public void AmazonRate_GetRates_ProcessesReturnFromWebClient_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShipmentEntity shipment = new ShipmentEntity()
                {
                    Order = new AmazonOrderEntity()
                    {
                        AmazonOrderID = "10",
                    },
                    Amazon = new AmazonShipmentEntity()
                    {
                        DeclaredValue = 12,
                        DimsHeight = 1,
                        DimsLength = 1,
                        DimsWidth = 1,
                        DimsWeight = 1
                    }
                };

                AmazonOrderItemEntity item = new AmazonOrderItemEntity()
                {
                    SKU = "abc",
                    OrderItemID = 123,
                    Quantity = 2
                };

                shipment.Order.OrderItems.Add(item);

                AmazonRates testObject = mock.Create<AmazonRates>();

                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(ValidResponse);

                RateGroup resultRateGroup = testObject.GetRates(shipment);
                Assert.AreEqual(20.56m, resultRateGroup.Rates.First().Amount);
            }
        }


        //[TestMethod]
        //public void AmazonRate_GetRates_CorrectShipmentInfoSentToClient_Test()
        //{
        //    using (var mock = AutoMock.GetLoose())
        //    {
        //        // Rather than mocking up a valid GetEligibleShippingServicesResponse
        //        // Deserializing from a good one
        //        // First rate is 20.56
        //        GetEligibleShippingServicesResponse response = SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(GetEmbeddedResourceXml("ShipWorks.Tests.Shipping.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml"));

        //        ShipmentEntity shipment = new ShipmentEntity()
        //        {
        //            Order = new AmazonOrderEntity()
        //            {
        //                AmazonOrderID = "10",
        //            },
        //            Amazon = new AmazonShipmentEntity()
        //            {
        //                AmazonAccountID = 123,
        //                DeclaredValue = 12,
        //                DimsHeight = 1,
        //                DimsLength = 1,
        //                DimsWidth = 1,
        //                DimsWeight = 1
        //            }
        //        };

        //        AmazonOrderItemEntity item = new AmazonOrderItemEntity()
        //        {
        //            SKU = "abc",
        //            OrderItemID = 123,
        //            Quantity = 2
        //        };

        //        shipment.Order.OrderItems.Add(item);

        //        AmazonAccountEntity amazonAccount = new AmazonAccountEntity()
        //        {
        //            AmazonAccountID = 123,
        //            MerchantID = "testMerchantID",
        //            AuthToken = "abc123"
        //        };


        //        mock.Mock<IAmazonAccountManager>()
        //            .Setup(a => a.GetAccount(123))
        //            .Returns(amazonAccount);

        //        AmazonMwsWebClientSettingsFactory settingsFactory = mock.Create<AmazonMwsWebClientSettingsFactory>();

        //        AmazonMwsWebClientSettings mwsSettings = settingsFactory.Create(shipment.Amazon);

        //        Mock<IAmazonShippingWebClient> webClient = new Mock<IAmazonShippingWebClient>();

        //        AmazonRates testObject = new AmazonRates(webClient.Object, settingsFactory);

        //        mock.Mock<IAmazonShippingWebClient>()
        //            .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
        //            .Returns(response);

        //        testObject.GetRates(shipment);

        //        webClient.Verify(
        //            a => a.GetRates(
        //                It.Is<ShipmentRequestDetails>(p => p.AmazonOrderId == "10"), It.IsAny<AmazonMwsWebClientSettings>()
        //            ));
        //    }
        //}

        private void VerifyApiRequest(Action<ShipmentEntity, OrderEntity> configureInput, Expression<Func<ShipmentRequestDetails, bool>> verifyCall)
        {
            ShipmentEntity shipment = SampleShipment;
            configureInput(shipment, shipment.Order as AmazonOrderEntity);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(ValidResponse);

                AmazonRates testObject = mock.Create<AmazonRates>();

                testObject.GetRates(shipment);

                mock.Mock<IAmazonShippingWebClient>()
                    .Verify(x => x.GetRates(It.Is(verifyCall), It.IsAny<AmazonMwsWebClientSettings>()));
            }
        }

        public GetEligibleShippingServicesResponse ValidResponse
        {
            get
            {
                return SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(GetEmbeddedResourceXml("ShipWorks.Tests.Shipping.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml"));
            }
        }

        public ShipmentEntity SampleShipment
        {
            get
            {
                return new ShipmentEntity()
                {
                    Order = new AmazonOrderEntity()
                    {
                        AmazonOrderID = "10",
                    },
                    Amazon = new AmazonShipmentEntity()
                    {
                        DeclaredValue = 12,
                        DimsHeight = 1,
                        DimsLength = 1,
                        DimsWidth = 1,
                        DimsWeight = 1
                    }
                };
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
