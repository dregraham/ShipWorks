﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.BestRate
{
    [TestClass]
    public class FedExBestRateBrokerTest
    {

        private FedExBestRateBroker testObject;
        private Mock<ICarrierAccountRepository<FedExAccountEntity>> genericRepositoryMock;
        private Mock<FedExShipmentType> genericShipmentTypeMock;

        [TestInitialize]
        public void Initialize()
        {
            genericRepositoryMock = new Mock<ICarrierAccountRepository<FedExAccountEntity>>();
            genericShipmentTypeMock = new Mock<FedExShipmentType>();

            testObject = new FedExBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) => genericShipmentTypeMock.Object.GetRates(shipment)
            };
        }

        [TestMethod]
        public void GetInsuranceProvider_ReturnsShipWorks_FedExSettingSpecfiesShipWorks_Test()
        {
            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { FedExInsuranceProvider = (int)InsuranceProvider.ShipWorks }));
        }

        [TestMethod]
        public void GetInsuranceProvider_ReturnsCarrier_FedExSettingSpecfiesCarrier_Test()
        {
            Assert.AreEqual(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { FedExInsuranceProvider = (int)InsuranceProvider.Carrier }));
        }
    }
}
