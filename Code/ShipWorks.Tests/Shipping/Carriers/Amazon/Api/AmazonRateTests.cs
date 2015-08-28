﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.ResolveAnything;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Shipping.Carriers.Amazon;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using System.IO;
using System.Reflection;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    [TestClass]
    public class AmazonRateTests
    {

        [TestMethod]
        public void AmazonRate_GetRates_ProcessesReturnFromWebClient_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Rather than mocking up a valid GetEligibleShippingServicesResponse
                // Deserializing from a good one
                // First rate is 20.56
                GetEligibleShippingServicesResponse response = SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(GetEmbeddedResourceXml("ShipWorks.Tests.Shipping.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml"));

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
                    .Returns(response);
                    
                RateGroup resultRateGroup = testObject.GetRates(shipment);
                Assert.AreEqual((Decimal)20.56, resultRateGroup.Rates.First().Amount);
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
