﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.GlobalShipAddress.Response;
using ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.GlobalShipAddress.IntegrationTest
{
    [TestClass]
    public class GlobalShipAddressIntegrationTest
    {

        [TestMethod]
        [Ignore]
        public void SearchLocation_ActuallyCallsFedEx_IntegrationTest()
        {
            // MARKED WITH THE IGNORE ATTRIBUTE SINCE THIS IS AN INTEGRATION TEST - WE DON'T WANT THIS TEST
            // TO BE EXECUTED IN THE NORMAL BUILD PROCESS

            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "602344126", MeterNumber = "118575561" };

            ShipmentEntity shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            Mock<ICarrierSettingsRepository> MockSettingsRepository = new Mock<ICarrierSettingsRepository>();
            MockSettingsRepository.Setup(x => x.GetShippingSettings())
                                  .Returns(new ShippingSettingsEntity()
                                  {
                                      FedExUsername = "Gaz3GPxQNv8E2ljM",
                                      FedExPassword = "h/Rat4y3/B6Pp2HlIiCPOHPUotyx+ihaH2wiHI42kds=",
                                      
                                  });
            MockSettingsRepository.Setup(x => x.UseTestServer)
                                  .Returns(true);

            MockSettingsRepository.Setup(x => x.GetAccount(It.IsAny<ShipmentEntity>()))
                                  .Returns(account);

            FedExRequestFactory fedExRequestFactory = new FedExRequestFactory(new FedExServiceGateway(MockSettingsRepository.Object), MockSettingsRepository.Object, new FedExShipmentTokenProcessor(), new FedExResponseFactory());
            CarrierRequest searchLocationsRequest = fedExRequestFactory.CreateSearchLocationsRequest(shipment, account);

            ICarrierResponse carrierResponse = searchLocationsRequest.Submit();

            FedExGlobalShipAddressResponse fedExGlobalShipAddressResponse = carrierResponse as FedExGlobalShipAddressResponse;

            Assert.IsNotNull(carrierResponse, "Response is null");
            carrierResponse.Process();

            Assert.IsNotNull(fedExGlobalShipAddressResponse,"Invalid Response...");

            Assert.AreEqual(1, fedExGlobalShipAddressResponse.DistanceAndLocationDetails.Count(), "Unexpected result from FedEx. This could mean FedEx added a new location near the destination address.");
        }
    }
}
