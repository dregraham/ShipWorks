using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Ups.OneBalance;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.OneBalance
{
    public class UpsShipmentRequestFactoryTest
    {
        private readonly AutoMock mock;
        private readonly UpsShipmentRequestFactory testObject;
        private ShipmentEntity shipment;
        private readonly Mock<IShipEngineRequestFactory> shipmentElementFactory;

        public UpsShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>()
            {
                new TestPackageAdapter()
            };
            

            Mock<ShipmentType> shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(t => t.GetPackageAdapters(It.IsAny<ShipmentEntity>()))
                .Returns(packageAdapters);
            shipmentType.Setup(t => t.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(true);
            shipmentType.Setup(t => t.SupportsGetRates)
                .Returns(true);

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(m => m.Get(ShipmentTypeCode.UpsOnLineTools))
                .Returns(shipmentType.Object);

            var rateShipmentRequest = new RateShipmentRequest() { Shipment = new AddressValidatingShipment() };
            rateShipmentRequest.Shipment.Packages = new List<ShipmentPackage>() { new ShipmentPackage() { LabelMessages = new LabelMessages() } };

            shipmentElementFactory = mock.Mock<IShipEngineRequestFactory>();
            shipmentElementFactory
                .Setup(f => f.CreateRateRequest(AnyShipment))
                .Returns(rateShipmentRequest);

            var purchaseLabelRequest = new PurchaseLabelRequest() { Shipment = new Shipment() };
            purchaseLabelRequest.Shipment.Packages = new List<ShipmentPackage>() { new ShipmentPackage() { LabelMessages = new LabelMessages() } };
            
            shipmentElementFactory
                .Setup(f => f.CreatePurchaseLabelRequest(AnyShipment, It.IsAny<List<IPackageAdapter>>(), AnyString, It.IsAny<Func<IPackageAdapter, string>>(), It.IsAny<Action<ShipmentPackage, IPackageAdapter>>()))
                .Returns(purchaseLabelRequest);

            var accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccountReadOnly(It.IsAny<ShipmentEntity>())).Returns(new UpsAccountEntity() { ShipEngineCarrierId = "abcd" });

            testObject = mock.Create<UpsShipmentRequestFactory>();

            shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
            };
        }

        [Fact]
        public void CreatePurchaseLabelRequest_SetsCarrierInsurance_WhenShipmentHasUpsInsurance()
        {
            shipment.Insurance = true;
            shipment.InsuranceProvider = (int) InsuranceProvider.Carrier;

            var result = testObject.CreatePurchaseLabelRequest(shipment);

            Assert.Equal(Shipment.InsuranceProviderEnum.Carrier, result.Shipment.InsuranceProvider);
        }

        [Fact]
        public void CreateRateShipmentRequest_SetsCarrierInsurance_WhenShipmentHasUpsInsurance()
        {
            shipment.Insurance = true;
            shipment.InsuranceProvider = (int) InsuranceProvider.Carrier;

            var result = testObject.CreateRateShipmentRequest(shipment);

            Assert.Equal(AddressValidatingShipment.InsuranceProviderEnum.Carrier, result.Shipment.InsuranceProvider);
        }
    }
}
