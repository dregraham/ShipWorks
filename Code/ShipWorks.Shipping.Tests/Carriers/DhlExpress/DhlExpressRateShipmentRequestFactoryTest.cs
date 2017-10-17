using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
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
        readonly Mock<IDhlExpressAccountRepository> accountRepo;
        readonly Mock<IShipmentElementFactory> shipmentElementFactory;
        readonly PurchaseLabelRequest purchaseLabelRequest;

        public DhlExpressRateShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            accountRepo = mock.Mock<IDhlExpressAccountRepository>();

            accountRepo.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-1234" });

            shipmentElementFactory = mock.Mock<IShipmentElementFactory>();
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            purchaseLabelRequest = new PurchaseLabelRequest() { Shipment = new Shipment() };
            shipmentElementFactory
                .Setup(f => f.CreatePurchaseLabelRequest(It.IsAny<ShipmentEntity>(), It.IsAny<List<IPackageAdapter>>(), It.IsAny<string>()))
                .Returns(purchaseLabelRequest);
        }

        [Fact]
        public void CreateRateShipmentRequest_ThrowsError_WhenShipmentIsNull()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.CreateRateShipmentRequest(null));
        }

        [Fact]
        public void CreateRateShipmentRequest_ThrowsError_WhenDhlExpressShipmentIsNull()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.CreateRateShipmentRequest(new ShipmentEntity()));
        }

        [Fact]
        public void CreateRateShipmentRequest_DelegatesToAccountRepository_ToGetAccount()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            testObject.CreateRateShipmentRequest(shipment);

            accountRepo.Verify(r => r.GetAccount(shipment), Times.Once());
        }

        [Fact]
        public void CreateRateShipmentRequest_SetsCarrierIDFromAccount()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal("se-1234", request.RateOptions.CarrierIds.First());
        }

        [Fact]
        public void CreateRateShipmentRequest_RequestFromShipmentElementFactoryIsReturned()
        {
            var requestFromShipmentElementFactory = new RateShipmentRequest() { Shipment = new AddressValidatingShipment() };
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>()))
                .Returns(requestFromShipmentElementFactory);

            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(requestFromShipmentElementFactory, request);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void CreateRateShipmentRequest_AdvancedOptionsAreSet(bool deliveryDutyPaid, bool nonMachinable, bool saturdayDelivery)
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    DeliveredDutyPaid = deliveryDutyPaid,
                    NonMachinable = nonMachinable,
                    SaturdayDelivery = saturdayDelivery
                }
            };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(3, request.Shipment.AdvancedOptions.Count());
            Assert.Equal(deliveryDutyPaid, request.Shipment.AdvancedOptions["delivered_duty_paid"]);
            Assert.Equal(nonMachinable, request.Shipment.AdvancedOptions["non_machinable"]);
            Assert.Equal(saturdayDelivery, request.Shipment.AdvancedOptions["saturday_delivery"]);
        }

        [Fact]
        public void CreateRateShipmentRequest_GetsCustoms_FromShipmentElementFactory()
        {
            var customsItems = new List<CustomsItem>();
            shipmentElementFactory
                .Setup(f => f.CreateCustomsItems(It.IsAny<ShipmentEntity>()))
                .Returns(customsItems);

            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.CreateRateShipmentRequest(shipment);

            shipmentElementFactory.Verify(f => f.CreateCustomsItems(shipment), Times.Once);
            Assert.Equal(customsItems, request.Shipment.Customs.CustomsItems);
        }

        [Theory]
        [InlineData(ShipEngineContentsType.ReturnedGoods, InternationalOptions.ContentsEnum.Returnedgoods)]
        [InlineData(ShipEngineContentsType.Documents, InternationalOptions.ContentsEnum.Documents)]
        [InlineData(ShipEngineContentsType.Gift, InternationalOptions.ContentsEnum.Gift)]
        [InlineData(ShipEngineContentsType.Sample, InternationalOptions.ContentsEnum.Sample)]
        public void CreateRateShipmentRequest_ContentsSet(ShipEngineContentsType contents, InternationalOptions.ContentsEnum apiContents)
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity
                {
                    Contents = (int) contents
                }
            };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(apiContents, request.Shipment.Customs.Contents);
        }

        [Theory]
        [InlineData(ShipEngineNonDeliveryType.ReturnToSender, InternationalOptions.NonDeliveryEnum.Returntosender)]
        [InlineData(ShipEngineNonDeliveryType.TreatAsAbandoned, InternationalOptions.NonDeliveryEnum.Treatasabandoned)]
        public void CreateRateShipmentRequest_NonDeliverySet(ShipEngineNonDeliveryType nonDelivery, InternationalOptions.NonDeliveryEnum apiNonDelivery)
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity
                {
                    NonDelivery = (int) nonDelivery
                }
            };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(apiNonDelivery, request.Shipment.Customs.NonDelivery);
        }

        [Fact]
        public void CreateRateShipmentRequest_CreatesPackages_FromPacakgeAdapterDelegatesToShipmentTypeAndElementFactory()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

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

            testObject.CreateRateShipmentRequest(shipment);

            shipmentTypeManager.Verify(m => m.Get(ShipmentTypeCode.DhlExpress), Times.Once());
            dhlShipmentType.Verify(t => t.GetPackageAdapters(shipment), Times.Once());
            shipmentElementFactory.Verify(f => f.CreatePackages(packageAdapters), Times.Once());
        }

        [Fact]
        public void CreateRateShipmentRequest_CreatesPackages_FromPacakgeAdapterDelegatesToShipmentType()
        {
            List<ShipmentPackage> apiPackages = new List<ShipmentPackage>();
            shipmentElementFactory.Setup(f => f.CreatePackages(It.IsAny<List<IPackageAdapter>>()))
                .Returns(apiPackages);            

            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

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

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(apiPackages, request.Shipment.Packages);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_ThrowsError_WhenShipmentIsNull()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.CreatePurchaseLabelRequest(null));
        }

        [Fact]
        public void CreatePurchaseLabelRequest_ThrowsError_WhenDhlExpressShipmentIsNull()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            Assert.Throws<ArgumentNullException>(() => testObject.CreatePurchaseLabelRequest(new ShipmentEntity()));
        }

        [Fact]
        public void CreatePurchaseLabelRequest_DelegatesToAccountRepository_ToGetAccount()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            testObject.CreatePurchaseLabelRequest(shipment);

            accountRepo.Verify(r => r.GetAccount(shipment), Times.Once());
        }

        [Fact]
        public void CreatePurchaseLabelRequest_SetsCarrierIDFromAccount()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal("se-1234", request.Shipment.CarrierId);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_RequestFromShipmentElementFactoryIsReturned()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(purchaseLabelRequest, request);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void CreatePurchaseLabelRequest_AdvancedOptionsAreSet(bool deliveryDutyPaid, bool nonMachinable, bool saturdayDelivery)
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    DeliveredDutyPaid = deliveryDutyPaid,
                    NonMachinable = nonMachinable,
                    SaturdayDelivery = saturdayDelivery
                }
            };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(3, request.Shipment.AdvancedOptions.Count());
            Assert.Equal(deliveryDutyPaid, request.Shipment.AdvancedOptions["delivered_duty_paid"]);
            Assert.Equal(nonMachinable, request.Shipment.AdvancedOptions["non_machinable"]);
            Assert.Equal(saturdayDelivery, request.Shipment.AdvancedOptions["saturday_delivery"]);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_GetsCustoms_FromShipmentElementFactory()
        {
            var customsItems = new List<CustomsItem>();
            shipmentElementFactory
                .Setup(f => f.CreateCustomsItems(It.IsAny<ShipmentEntity>()))
                .Returns(customsItems);

            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            shipmentElementFactory.Verify(f => f.CreateCustomsItems(shipment), Times.Once);
            Assert.Equal(customsItems, request.Shipment.Customs.CustomsItems);
        }

        [Theory]
        [InlineData(ShipEngineContentsType.ReturnedGoods, InternationalOptions.ContentsEnum.Returnedgoods)]
        [InlineData(ShipEngineContentsType.Documents, InternationalOptions.ContentsEnum.Documents)]
        [InlineData(ShipEngineContentsType.Gift, InternationalOptions.ContentsEnum.Gift)]
        [InlineData(ShipEngineContentsType.Sample, InternationalOptions.ContentsEnum.Sample)]
        public void CreatePurchaseLabelRequest_ContentsSet(ShipEngineContentsType contents, InternationalOptions.ContentsEnum apiContents)
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity
                {
                    Contents = (int) contents
                }
            };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(apiContents, request.Shipment.Customs.Contents);
        }

        [Theory]
        [InlineData(ShipEngineNonDeliveryType.ReturnToSender, InternationalOptions.NonDeliveryEnum.Returntosender)]
        [InlineData(ShipEngineNonDeliveryType.TreatAsAbandoned, InternationalOptions.NonDeliveryEnum.Treatasabandoned)]
        public void CreatePurchaseLabelRequest_NonDeliverySet(ShipEngineNonDeliveryType nonDelivery, InternationalOptions.NonDeliveryEnum apiNonDelivery)
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity
                {
                    NonDelivery = (int) nonDelivery
                }
            };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(apiNonDelivery, request.Shipment.Customs.NonDelivery);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_CreatesPackages_FromShipmentTypeGetPackageAdapters()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

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

            testObject.CreatePurchaseLabelRequest(shipment);

            shipmentTypeManager.Verify(m => m.Get(ShipmentTypeCode.DhlExpress), Times.Once());
            dhlShipmentType.Verify(t => t.GetPackageAdapters(shipment), Times.Once());
        }

        [Fact]
        public void CreatePurchaseLabelRequest_SendsPackageAdapters_FromShipmentTypeGetPackageAdapters()
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

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

            testObject.CreatePurchaseLabelRequest(shipment);

            shipmentElementFactory.Verify(f=>f.CreatePurchaseLabelRequest(shipment, packageAdapters, It.IsAny<string>()), Times.Once());
        }

        [Theory]
        [InlineData(DhlExpressServiceType.ExpressEnvelope)]
        [InlineData(DhlExpressServiceType.ExpressWorldWide)]
        public void CreatePurchaseLabelRequest_SetsServiceCorrectly(DhlExpressServiceType serviceType)
        {
            var testObject = mock.Create<DhlExpressShipmentRequestFactory>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    Service = (int) serviceType
                }
            };

            testObject.CreatePurchaseLabelRequest(shipment);

            shipmentElementFactory.Verify(f=>f.CreatePurchaseLabelRequest(shipment, It.IsAny<List<IPackageAdapter>>(), EnumHelper.GetApiValue(serviceType)), Times.Once());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
