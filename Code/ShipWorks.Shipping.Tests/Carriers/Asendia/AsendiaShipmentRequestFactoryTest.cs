using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.Asendia
{
    public class AsendiaShipmentRequestFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IAsendiaAccountRepository> accountRepo;
        private readonly Mock<IShipEngineRequestFactory> shipmentElementFactory;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly PurchaseLabelRequest purchaseLabelRequest;
        private ShipmentEntity shipment;
        List<IPackageAdapter> packageAdapters;
        Mock<ShipmentType> shipmentType;
        AsendiaShipmentRequestFactory testObject;

        public AsendiaShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            accountRepo = mock.Mock<IAsendiaAccountRepository>();

            accountRepo.Setup(r => r.GetAccount(AnyShipment))
                .Returns(new AsendiaAccountEntity() { ShipEngineCarrierId = "se-1234" });

            shipmentElementFactory = mock.Mock<IShipEngineRequestFactory>();
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(AnyShipment))
                .Returns(new RateShipmentRequest() { Shipment = new AddressValidatingShipment() });

            purchaseLabelRequest = new PurchaseLabelRequest() { Shipment = new Shipment() };
            shipmentElementFactory
                .Setup(f => f.CreatePurchaseLabelRequest(AnyShipment, It.IsAny<List<IPackageAdapter>>(), AnyString))
                .Returns(purchaseLabelRequest);

            packageAdapters = new List<IPackageAdapter>();

            shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(t => t.GetPackageAdapters(shipment))
                .Returns(packageAdapters);
            shipmentType.Setup(t => t.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(true);
            shipmentType.Setup(t => t.SupportsGetRates)
                .Returns(true);

            shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(m => m.Get(ShipmentTypeCode.Asendia))
                .Returns(shipmentType.Object);

            shipment = new ShipmentEntity()
            {
                Asendia = new AsendiaShipmentEntity(),
                ShipmentTypeCode = ShipmentTypeCode.Asendia
            };

            testObject = mock.Create<AsendiaShipmentRequestFactory>();
        }

        [Fact]
        public void CreateRateShipmentRequest_ThrowsError_WhenAsendiaShipmentIsNull()
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateRateShipmentRequest_AdvancedOptionsAreSet(bool nonMachinable)
        {
            shipment.Asendia = new AsendiaShipmentEntity()
            {
                NonMachinable = nonMachinable,
            };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(1, request.Shipment.AdvancedOptions.Count());
            Assert.Equal(nonMachinable, request.Shipment.AdvancedOptions["non_machinable"]);
        }

        [Theory]
        [InlineData(ShipEngineContentsType.ReturnedGoods, InternationalOptions.ContentsEnum.Returnedgoods)]
        [InlineData(ShipEngineContentsType.Documents, InternationalOptions.ContentsEnum.Documents)]
        [InlineData(ShipEngineContentsType.Gift, InternationalOptions.ContentsEnum.Gift)]
        [InlineData(ShipEngineContentsType.Sample, InternationalOptions.ContentsEnum.Sample)]
        public void CreateRateShipmentRequest_ContentsSet(ShipEngineContentsType contents, InternationalOptions.ContentsEnum apiContents)
        {
            shipment.Asendia = new AsendiaShipmentEntity
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
            shipment.Asendia = new AsendiaShipmentEntity
            {
                NonDelivery = (int) nonDelivery
            };

            var request = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(apiNonDelivery, request.Shipment.Customs.NonDelivery);
        }

        [Fact]
        public void CreatePurchaseLabelRequest_ThrowsError_WhenAsendiaShipmentIsNull()
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreatePurchaseLabelRequest_AdvancedOptionsAreSet(bool nonMachinable)
        {
            shipment.Asendia = new AsendiaShipmentEntity()
            {
                NonMachinable = nonMachinable,
            };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(1, request.Shipment.AdvancedOptions.Count());
            Assert.Equal(nonMachinable, request.Shipment.AdvancedOptions["non_machinable"]);
        }

        [Theory]
        [InlineData(ShipEngineContentsType.ReturnedGoods, InternationalOptions.ContentsEnum.Returnedgoods)]
        [InlineData(ShipEngineContentsType.Documents, InternationalOptions.ContentsEnum.Documents)]
        [InlineData(ShipEngineContentsType.Gift, InternationalOptions.ContentsEnum.Gift)]
        [InlineData(ShipEngineContentsType.Sample, InternationalOptions.ContentsEnum.Sample)]
        public void CreatePurchaseLabelRequest_ContentsSet(ShipEngineContentsType contents, InternationalOptions.ContentsEnum apiContents)
        {
            shipment.Asendia = new AsendiaShipmentEntity
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
            shipment.Asendia = new AsendiaShipmentEntity
            {
                NonDelivery = (int) nonDelivery
            };

            var request = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(apiNonDelivery, request.Shipment.Customs.NonDelivery);
        }

        [Theory]
        [InlineData(AsendiaServiceType.AsendiaEPacket)]
        [InlineData(AsendiaServiceType.AsendiaInternationalExpress)]
        [InlineData(AsendiaServiceType.AsendiaIPA)]
        [InlineData(AsendiaServiceType.AsendiaISAL)]
        [InlineData(AsendiaServiceType.AsendiaOther)]
        [InlineData(AsendiaServiceType.AsendiaPMEI)]
        [InlineData(AsendiaServiceType.AsendiaPMI)]
        [InlineData(AsendiaServiceType.AsendiaPriorityTracked)]
        public void CreatePurchaseLabelRequest_SetsServiceCorrectly(AsendiaServiceType serviceType)
        {
            shipment.Asendia = new AsendiaShipmentEntity()
            {
                Service = serviceType
            };

            testObject.CreatePurchaseLabelRequest(shipment);

            shipmentElementFactory.Verify(f => f.CreatePurchaseLabelRequest(shipment, It.IsAny<List<IPackageAdapter>>(), EnumHelper.GetApiValue(serviceType)), Times.Once());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
