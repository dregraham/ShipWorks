﻿using System;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.BestRate;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.BestRate
{
    public class OnTracBestRateBrokerTest
    {
        private OnTracBestRateBroker testObject;
        private Mock<ICarrierAccountRepository<OnTracAccountEntity>> genericRepositoryMock;
        private Mock<OnTracShipmentType> genericShipmentTypeMock;

        public OnTracBestRateBrokerTest()
        {
            genericRepositoryMock = new Mock<ICarrierAccountRepository<OnTracAccountEntity>>();
            genericShipmentTypeMock = new Mock<OnTracShipmentType>();

            testObject = new OnTracBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object);
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_OnTracSettingSpecfiesShipWorks_Test()
        {
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks}));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_OnTracSettingSpecfiesCarrier_Test()
        {
            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { OnTracInsuranceProvider = (int)InsuranceProvider.Carrier }));
        }
    }
}
