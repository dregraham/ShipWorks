﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon.Api
{
    [Trait("Carrier", "Amazon")]
    public class AmazonRateTests
    {
        GetEligibleShippingServices fakedRateResponse;
        AmazonRates testObject;
        ShipmentEntity shipment;
        AmazonMwsWebClientSettings mwsSettings;
        Mock<IAmazonShippingWebClient> webClient;
        Mock<IAmazonMwsWebClientSettingsFactory> mwsSettingsFactory;

        public AmazonRateTests()
        {
            fakedRateResponse = new GetEligibleShippingServices()
            {
                GetEligibleShippingServicesResponse = new GetEligibleShippingServicesResponse()
                {
                    ShippingServiceList = new List<ShippingService>()
                    {
                        new ShippingService()
                        {
                            CarrierName = "TestCarrier",
                            Rate = new Rate()
                            {
                                Amount = 5
                            }
                        }
                    }
                }
            };

            shipment = new ShipmentEntity()
            {
                Order = new AmazonOrderEntity()
                {
                    AmazonOrderID = "10"
                },
                Amazon = new AmazonShipmentEntity()
                {
                    DeclaredValue = 12,
                    DimsHeight = 23,
                    DimsLength = 17,
                    DimsWidth = 4,
                    DimsWeight = 3,
                    DimsAddWeight = true,
                    DateMustArriveBy = DateTime.Today.AddDays(5)
                }
            };

            shipment.Order.OrderItems.Add(new AmazonOrderItemEntity()
            {
                AmazonOrderItemCode = "42",
                Quantity = 5
            });

            mwsSettings = new AmazonMwsWebClientSettings(new AmazonMwsConnection("", "", ""));

            mwsSettingsFactory = new Mock<IAmazonMwsWebClientSettingsFactory>();
            mwsSettingsFactory.Setup(s => s.Create(It.IsAny<AmazonShipmentEntity>())).Returns(mwsSettings);
            
            webClient = new Mock<IAmazonShippingWebClient>();
            webClient.Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                .Returns(fakedRateResponse);

            testObject = new AmazonRates(webClient.Object, mwsSettingsFactory.Object);
        }

        [Fact]
        public void AmazonRate_GetRates_ProcessesReturnFromWebClient_Test()
        {
            RateGroup resultRateGroup = testObject.GetRates(shipment);
            Assert.Equal(5, resultRateGroup.Rates.First().Amount);
        }

        [Fact]
        public void AmazonRate_GetRates_CorrectShipmentInfoSentToClient_Test()
        {
            testObject.GetRates(shipment);

            webClient.Verify(
                a => a.GetRates(
                    It.Is<ShipmentRequestDetails>(p => p.AmazonOrderId == "10"),mwsSettings
                ));
        }
    }
}
