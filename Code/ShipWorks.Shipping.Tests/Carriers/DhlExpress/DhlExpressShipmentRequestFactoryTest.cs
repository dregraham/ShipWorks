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
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressShipmentRequestFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IDhlExpressAccountRepository> accountRepo;
        private readonly Mock<IShipEngineRequestFactory> shipmentElementFactory;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly PurchaseLabelRequest purchaseLabelRequest;
        private ShipmentEntity shipment;
        List<IPackageAdapter> packageAdapters;
        Mock<ShipmentType> dhlShipmentType;
        DhlExpressShipmentRequestFactory testObject;

        public DhlExpressShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            accountRepo = mock.Mock<IDhlExpressAccountRepository>();

            accountRepo.Setup(r => r.GetAccount(AnyShipment))
                .Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-1234" });

            shipmentElementFactory = mock.Mock<IShipEngineRequestFactory>();
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(AnyShipment))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            purchaseLabelRequest = new PurchaseLabelRequest() { Shipment = new Shipment() };
            shipmentElementFactory
                .Setup(f => f.CreatePurchaseLabelRequest(AnyShipment, It.IsAny<List<IPackageAdapter>>(), AnyString))
                .Returns(purchaseLabelRequest);

            packageAdapters = new List<IPackageAdapter>();

            dhlShipmentType = mock.Mock<ShipmentType>();
            dhlShipmentType.Setup(t => t.GetPackageAdapters(shipment))
                .Returns(packageAdapters);
            dhlShipmentType.Setup(t => t.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(true);
            dhlShipmentType.Setup(t => t.SupportsGetRates)
                .Returns(true);

            shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(dhlShipmentType.Object);

            shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity(),
                ShipmentTypeCode = ShipmentTypeCode.DhlExpress
            };

            testObject = mock.Create<DhlExpressShipmentRequestFactory>();
        }

        [Fact]
        public void CreateRateShipmentRequest_ThrowsError_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.CreateRateShipmentRequest(null));
        }

        [Fact]
        public void CreateRateShipmentRequest_ThrowsError_WhenDhlExpressShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.CreateRateShipmentRequest(new ShipmentEntity()));
        }

        [Fact]
        public void CreateRateShipmentRequest_DelegatesToAccountRepository_ToGetAccount()
        {
            testObject.CreateRateShipmentRequest(shipment);

            accountRepo.Verify(r => r.GetAccount(shipment), Times.Once());
        }

        [Fact]
        public void CreateRateShipmentRequest_SetsCarrierIDFromAccount()
        {
            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal("se-1234", request.RateOptions.CarrierIds.First());
        }

        [Fact]
        public void CreateRateShipmentRequest_RequestFromShipmentElementFactoryIsReturned()
        {
            var requestFromShipmentElementFactory = new RateShipmentRequest() { Shipment = new AddressValidatingShipment() };
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(AnyShipment))
                .Returns(requestFromShipmentElementFactory);
            
            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(requestFromShipmentElementFactory, request);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void CreateRateShipmentRequest_AdvancedOptionsAreSet(bool deliveryDutyPaid, bool nonMachinable, bool saturdayDelivery)
        {
            shipment.DhlExpress = new DhlExpressShipmentEntity()
            {
                DeliveredDutyPaid = deliveryDutyPaid,
                NonMachinable = nonMachinable,
                SaturdayDelivery = saturdayDelivery
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
                .Setup(f => f.CreateCustomsItems(AnyShipment))
                .Returns(customsItems);

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
            shipment.DhlExpress = new DhlExpressShipmentEntity
            {
                Contents = (int) contents
            };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(apiContents, request.Shipment.Customs.Contents);
        }

        [Theory]
        [InlineData(ShipEngineNonDeliveryType.ReturnToSender, InternationalOptions.NonDeliveryEnum.Returntosender)]
        [InlineData(ShipEngineNonDeliveryType.TreatAsAbandoned, InternationalOptions.NonDeliveryEnum.Treatasabandoned)]
        public void CreateRateShipmentRequest_NonDeliverySet(ShipEngineNonDeliveryType nonDelivery, InternationalOptions.NonDeliveryEnum apiNonDelivery)
        {
            shipment.DhlExpress = new DhlExpressShipmentEntity
            {
                NonDelivery = (int) nonDelivery
            };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(apiNonDelivery, request.Shipment.Customs.NonDelivery);
        }

        [Fact]
        public void CreateRateShipmentRequest_CreatesPackages_FromPackageAdapterDelegatesToShipmentTypeAndElementFactory()
        {
            testObject.CreateRateShipmentRequest(shipment);

            dhlShipmentType.Verify(t => t.GetPackageAdapters(shipment), Times.Once());
            shipmentElementFactory.Verify(f => f.CreatePackages(packageAdapters), Times.Once());
        }

        [Fact]
        public void CreateRateShipmentRequest_CreatesPackages_FromPacakgeAdapterDelegatesToShipmentType()
        {
            List<ShipmentPackage> apiPackages = new List<ShipmentPackage>();
            shipmentElementFactory.Setup(f => f.CreatePackages(It.IsAny<List<IPackageAdapter>>()))
                .Returns(apiPackages);            
            
            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(apiPackages, request.Shipment.Packages);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_ThrowsError_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.CreatePurchaseLabelRequest(null));
        }

        [Fact]
        public void CreatePurchaseLabelRequest_ThrowsError_WhenDhlExpressShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.CreatePurchaseLabelRequest(new ShipmentEntity()));
        }

        [Fact]
        public void CreatePurchaseLabelRequest_DelegatesToAccountRepository_ToGetAccount()
        {
            testObject.CreatePurchaseLabelRequest(shipment);

            accountRepo.Verify(r => r.GetAccount(shipment), Times.Once());
        }

        [Fact]
        public void CreatePurchaseLabelRequest_SetsCarrierIDFromAccount()
        {
            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal("se-1234", request.Shipment.CarrierId);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_RequestFromShipmentElementFactoryIsReturned()
        {
            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(purchaseLabelRequest, request);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void CreatePurchaseLabelRequest_AdvancedOptionsAreSet(bool deliveryDutyPaid, bool nonMachinable, bool saturdayDelivery)
        {
            shipment.DhlExpress = new DhlExpressShipmentEntity()
            {
                DeliveredDutyPaid = deliveryDutyPaid,
                NonMachinable = nonMachinable,
                SaturdayDelivery = saturdayDelivery
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
                .Setup(f => f.CreateCustomsItems(AnyShipment))
                .Returns(customsItems);

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
            shipment.DhlExpress = new DhlExpressShipmentEntity
            {
                Contents = (int) contents
            };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(apiContents, request.Shipment.Customs.Contents);
        }

        [Theory]
        [InlineData(ShipEngineNonDeliveryType.ReturnToSender, InternationalOptions.NonDeliveryEnum.Returntosender)]
        [InlineData(ShipEngineNonDeliveryType.TreatAsAbandoned, InternationalOptions.NonDeliveryEnum.Treatasabandoned)]
        public void CreatePurchaseLabelRequest_NonDeliverySet(ShipEngineNonDeliveryType nonDelivery, InternationalOptions.NonDeliveryEnum apiNonDelivery)
        {
            shipment.DhlExpress = new DhlExpressShipmentEntity
            {
                NonDelivery = (int) nonDelivery
            };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(apiNonDelivery, request.Shipment.Customs.NonDelivery);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_CreatesPackages_FromShipmentTypeGetPackageAdapters()
        {
            testObject.CreatePurchaseLabelRequest(shipment);

            dhlShipmentType.Verify(t => t.GetPackageAdapters(shipment), Times.Once());
        }

        [Fact]
        public void CreatePurchaseLabelRequest_SendsPackageAdapters_FromShipmentTypeGetPackageAdapters()
        {            
            testObject.CreatePurchaseLabelRequest(shipment);

            shipmentElementFactory.Verify(f=>f.CreatePurchaseLabelRequest(shipment, packageAdapters, AnyString), Times.Once());
        }

        [Theory]
        [InlineData(DhlExpressServiceType.ExpressEnvelope)]
        [InlineData(DhlExpressServiceType.ExpressWorldWide)]
        public void CreatePurchaseLabelRequest_SetsServiceCorrectly(DhlExpressServiceType serviceType)
        {
            shipment.DhlExpress = new DhlExpressShipmentEntity()
            {
                Service = (int) serviceType
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
