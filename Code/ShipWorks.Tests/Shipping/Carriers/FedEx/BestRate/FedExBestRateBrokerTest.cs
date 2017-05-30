using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.BestRate
{
    public class FedExBestRateBrokerTest
    {

        private readonly FedExBestRateBroker testObject;
        private readonly Mock<ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity>> genericRepositoryMock;
        private readonly Mock<FedExShipmentType> genericShipmentTypeMock;

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

        [Fact]
        public void GetBestRates_AddsBrokerException_WhenExceptionsRateFootnoteFactoryIsReturnedFromBase()
        {
            FedExAccountEntity account = new FedExAccountEntity() {CountryCode = "US"};

            genericRepositoryMock.Setup(x => x.Accounts)
                                 .Returns(new List<FedExAccountEntity> { account });
            genericRepositoryMock.Setup(x => x.DefaultProfileAccount)
                                 .Returns(account);

            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.FedEx);

            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                .Callback<ShipmentEntity>(x =>
                {
                    x.FedEx = new FedExShipmentEntity();
                    x.FedEx.Packages.Add(new FedExPackageEntity());
                });

            var testShipment = new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.BestRate, ContentWeight = 12.1, BestRate = new BestRateShipmentEntity(), OriginCountryCode = "US" };
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            RateGroup rateGroup = new RateGroup(new List<RateResult>());
            rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(ShipmentTypeCode.FedEx, new Exception("blah")));

            testObject.GetRatesAction = (shipment, type) => rateGroup;

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(1, brokerExceptions.Count);
            Assert.Equal("blah", brokerExceptions.Single().Message);
        }
    }
}
