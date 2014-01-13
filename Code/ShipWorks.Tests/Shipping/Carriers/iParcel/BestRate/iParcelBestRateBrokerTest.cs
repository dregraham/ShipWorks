using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.BestRate;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.BestRate
{
    [TestClass]
    public class iParcelBestRateBrokerTest
    {
        private iParcelBestRateBroker testObject;
        private Mock<ICarrierAccountRepository<IParcelAccountEntity>> genericRepositoryMock;
        private Mock<iParcelShipmentType> genericShipmentTypeMock;

        [TestInitialize]
        public void Initialize()
        {
            genericRepositoryMock = new Mock<ICarrierAccountRepository<IParcelAccountEntity>>();
            genericShipmentTypeMock = new Mock<iParcelShipmentType>();

            testObject = new iParcelBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = shipment => genericShipmentTypeMock.Object.GetRates(shipment)
            };
        }

        [TestMethod]
        public void GetInsuranceProvider_ReturnsShipWorks_iParcelSettingSpecfiesShipWorks_Test()
        {
            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { IParcelInsuranceProvider = (int)InsuranceProvider.ShipWorks }));
        }

        [TestMethod]
        public void GetInsuranceProvider_ReturnsCarrier_iParcelSettingSpecfiesCarrier_Test()
        {
            Assert.AreEqual(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { IParcelInsuranceProvider = (int)InsuranceProvider.Carrier }));
        }
    }
}
