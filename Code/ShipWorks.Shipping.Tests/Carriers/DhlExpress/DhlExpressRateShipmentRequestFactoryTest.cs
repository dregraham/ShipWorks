using Autofac.Extras.Moq;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressRateShipmentRequestFactoryTest : IDisposable
    {
        readonly AutoMock mock;
        Mock<IDhlExpressAccountRepository> accountRepo;
        Mock<IShipmentElementFactory> shipmentElementFactory;


        public DhlExpressRateShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            accountRepo = mock.Mock<IDhlExpressAccountRepository>();

            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory = mock.Mock<IShipmentElementFactory>();
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });
        }

        [Fact]
        public void Create_ThrowsError_WhenShipmentIsNull()
        {
            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public void Create_ThrowsError_WhenDhlExpressShipmentIsNull()
        {
            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.Create(new ShipmentEntity()));
        }

        [Fact]
        public void Create_DelegatesToAccountRepository_ToGetAccount()
        {
            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            testObject.Create(shipment);

            accountRepo.Verify(r => r.GetAccount(shipment), Times.Once());
        }

        [Fact]
        public void Create_SetsCarrierIDFromAccount()
        {
            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                   .Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-1234" });

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.Create(shipment);

            Assert.Equal("se-1234", request.RateOptions.CarrierIds.First());
        }

        [Fact]
        public void Create_RequestFromShipmentElementFactoryIsReturned()
        {
            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity());

            var requestFromShipmentElementFactory = new RateShipmentRequest() { Shipment = new AddressValidatingShipment() };
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(requestFromShipmentElementFactory);

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.Create(shipment);

            Assert.Equal(requestFromShipmentElementFactory, request);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void Create_AdvancedOptionsAreSet(bool deliveryDutyPaid, bool nonMachinable, bool saturdayDelivery)
        {
            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                   .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    DeliveredDutyPaid = deliveryDutyPaid,
                    NonMachinable = nonMachinable,
                    SaturdayDelivery = saturdayDelivery
                }
            };

            var request = testObject.Create(shipment);

            Assert.Equal(3, request.Shipment.AdvancedOptions.Count());
            Assert.Equal(deliveryDutyPaid, request.Shipment.AdvancedOptions["delivered_duty_paid"]);
            Assert.Equal(nonMachinable, request.Shipment.AdvancedOptions["non_machinable"]);
            Assert.Equal(saturdayDelivery, request.Shipment.AdvancedOptions["saturday_delivery"]);
        }

        [Fact]
        public void Create_GetsCustoms_FromShipmentElementFactory()
        {
            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                  .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            var customsItems = new List<CustomsItem>();
            shipmentElementFactory
                .Setup(f => f.CreateCustomsItems(It.IsAny<ShipmentEntity>()))
                .Returns(customsItems);

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.Create(shipment);

            shipmentElementFactory.Verify(f => f.CreateCustomsItems(shipment), Times.Once);
            Assert.Equal(customsItems, request.Shipment.Customs.CustomsItems);
        }

        [Theory]
        [InlineData(ShipEngineContentsType.ReturnedGoods, InternationalOptions.ContentsEnum.Returnedgoods)]
        [InlineData(ShipEngineContentsType.Documents, InternationalOptions.ContentsEnum.Documents)]
        [InlineData(ShipEngineContentsType.Gift, InternationalOptions.ContentsEnum.Gift)]
        [InlineData(ShipEngineContentsType.Sample, InternationalOptions.ContentsEnum.Sample)]
        public void Create_ContentsSet(ShipEngineContentsType contents, InternationalOptions.ContentsEnum apiContents)
        {
            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity
                {
                    Contents = (int) contents
                }
            };

            var request = testObject.Create(shipment);

            Assert.Equal(apiContents, request.Shipment.Customs.Contents);
        }

        [Theory]
        [InlineData(ShipEngineNonDeliveryType.ReturnToSender, InternationalOptions.NonDeliveryEnum.Returntosender)]
        [InlineData(ShipEngineNonDeliveryType.TreatAsAbandoned, InternationalOptions.NonDeliveryEnum.Treatasabandoned)]
        public void Create_NonDeliverySet(ShipEngineNonDeliveryType nonDelivery, InternationalOptions.NonDeliveryEnum apiNonDelivery)
        {
            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity
                {
                    NonDelivery = (int) nonDelivery
                }
            };

            var request = testObject.Create(shipment);

            Assert.Equal(apiNonDelivery, request.Shipment.Customs.NonDelivery);
        }

        [Fact]
        public void Create_CreatesPackages_FromPacakgeAdapterDelegatesToShipmentTypeAndElementFactory()
        {

            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity
            {
                DhlExpress = new DhlExpressShipmentEntity()
            };
            
            var packageAdapters = new List<IPackageAdapter>();

            var dhlShipmentType = mock.Mock<ShipmentType>();
            dhlShipmentType.Setup(t => t.GetPackageAdapters(shipment))
                .Returns(packageAdapters);

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(dhlShipmentType.Object);

            var request = testObject.Create(shipment);

            shipmentTypeManager.Verify(m => m.Get(ShipmentTypeCode.DhlExpress), Times.Once());
            dhlShipmentType.Verify(t => t.GetPackageAdapters(shipment), Times.Once());
            shipmentElementFactory.Verify(f => f.CreatePackages(packageAdapters), Times.Once());
        }

        [Fact]
        public void Create_CreatesPackages_FromPacakgeAdapterDelegatesToShipmentType()
        {

            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity());

            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            List<ShipmentPackage> apiPackages = new List<ShipmentPackage>();
            shipmentElementFactory.Setup(f => f.CreatePackages(It.IsAny<List<IPackageAdapter>>()))
                .Returns(apiPackages);
            

            var testObject = mock.Create<DhlExpressRateShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity
            {
                DhlExpress = new DhlExpressShipmentEntity()
            };

            var packageAdapters = new List<IPackageAdapter>();

            var dhlShipmentType = mock.Mock<ShipmentType>();
            dhlShipmentType.Setup(t => t.GetPackageAdapters(shipment))
                .Returns(packageAdapters);

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(dhlShipmentType.Object);

            var request = testObject.Create(shipment);

            Assert.Equal(apiPackages, request.Shipment.Packages);
        }


        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
