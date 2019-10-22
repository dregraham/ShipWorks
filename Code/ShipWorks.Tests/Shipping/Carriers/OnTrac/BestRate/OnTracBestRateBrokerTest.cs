using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.BestRate;
using ShipWorks.Shipping.Insurance;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.BestRate
{
    public class OnTracBestRateBrokerTest
    {
        private OnTracBestRateBroker testObject;
        private Mock<ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity>> genericRepositoryMock;
        private Mock<OnTracShipmentType> genericShipmentTypeMock;
        private Mock<IBestRateExcludedAccountRepository> bestRateExludedAccountRepositoryMock;

        public OnTracBestRateBrokerTest()
        {
            genericRepositoryMock = new Mock<ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity>>();
            genericShipmentTypeMock = new Mock<OnTracShipmentType>();
            bestRateExludedAccountRepositoryMock = new Mock<IBestRateExcludedAccountRepository>();
            bestRateExludedAccountRepositoryMock.Setup(r => r.GetAll()).Returns(new List<long>());

            testObject = new OnTracBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object, bestRateExludedAccountRepositoryMock.Object);
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_OnTracSettingSpecfiesShipWorks()
        {
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks }));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_OnTracSettingSpecfiesCarrier()
        {
            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { OnTracInsuranceProvider = (int) InsuranceProvider.Carrier }));
        }
    }
}
