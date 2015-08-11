using System;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.BestRate;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.BestRate
{
    public class iParcelBestRateBrokerTest
    {
        private iParcelBestRateBroker testObject;
        private Mock<ICarrierAccountRepository<IParcelAccountEntity>> genericRepositoryMock;
        private Mock<iParcelShipmentType> genericShipmentTypeMock;

        public iParcelBestRateBrokerTest()
        {
            genericRepositoryMock = new Mock<ICarrierAccountRepository<IParcelAccountEntity>>();
            genericShipmentTypeMock = new Mock<iParcelShipmentType>();

            testObject = new iParcelBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) => genericShipmentTypeMock.Object.GetRates(shipment)
            };
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_iParcelSettingSpecfiesShipWorks_Test()
        {
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { IParcelInsuranceProvider = (int)InsuranceProvider.ShipWorks }));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_iParcelSettingSpecfiesCarrier_Test()
        {
            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { IParcelInsuranceProvider = (int)InsuranceProvider.Carrier }));
        }
    }
}
