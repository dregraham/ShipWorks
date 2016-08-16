using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Insurance;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.BestRate
{
    public class FedExBestRateBrokerTest
    {

        private FedExBestRateBroker testObject;
        private Mock<ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity>> genericRepositoryMock;
        private Mock<FedExShipmentType> genericShipmentTypeMock;

        public FedExBestRateBrokerTest()
        {
            genericRepositoryMock = new Mock<ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity>>();
            genericShipmentTypeMock = new Mock<FedExShipmentType>();

            testObject = new FedExBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object);
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_FedExSettingSpecfiesShipWorks()
        {
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { FedExInsuranceProvider = (int) InsuranceProvider.ShipWorks }));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_FedExSettingSpecfiesCarrier()
        {
            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { FedExInsuranceProvider = (int) InsuranceProvider.Carrier }));
        }
    }
}
