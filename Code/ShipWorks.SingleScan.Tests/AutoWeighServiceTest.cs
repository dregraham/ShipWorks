using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class AutoWeighServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AutoWeighService testObject;
        private readonly ITrackedDurationEvent trackedDurationEvent;

        public AutoWeighServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            trackedDurationEvent = mock.Mock<ITrackedDurationEvent>().Object;

            testObject = mock.Create<AutoWeighService>();

            SetAutoWeighSetting(true);
        }

        [Fact]
        public void AutoWeighService_DoesNotReadScale_WhenAutoWeighIsOff()
        {
            SetAutoWeighSetting(false);

            var uspsShipmentEntity = Create.Shipment().AsPostal(x => x.AsUsps()).Build();

            var shipments = new List<ShipmentEntity>() { uspsShipmentEntity };

            testObject.ApplyWeight(shipments, trackedDurationEvent);

            mock.Mock<IScaleReader>()
                .Verify(s => s.ReadScale(), Times.Never);
        }

        [Fact]
        public void AutoWeighService_SetsWeightOnAllPackages_WhenMultiplePackages_AndMultipleShipments()
        {
            var shipment = Create.Shipment().Build();
            var shipments = new List<ShipmentEntity>() { shipment, shipment };

            var packageAdapter = mock.Mock<IPackageAdapter>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdapters())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);

            mock.Mock<IScaleReader>()
                .Setup(s => s.ReadScale())
                .ReturnsAsync(ScaleReadResult.Success(42, ScaleType.Usb));

            testObject.ApplyWeight(shipments, trackedDurationEvent);

            packageAdapter.VerifySet(package => package.Weight = 42, Times.Exactly(4));
        }

        [Fact]
        public void AutoWeighService_DoesNotSetWeightOnAllPackages_WhenWeightIsWithinTenthOfAnOunceOfExistingWeight()
        {
            var shipment = Create.Shipment().Build();
            var shipments = new List<ShipmentEntity>() { shipment, shipment };

            var packageAdapter = mock.Mock<IPackageAdapter>();
            packageAdapter.SetupGet(p => p.Weight).Returns(42);

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdapters())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);

            mock.Mock<IScaleReader>()
                .Setup(s => s.ReadScale())
                .ReturnsAsync(ScaleReadResult.Success(42.00625, ScaleType.Usb));

            testObject.ApplyWeight(shipments, trackedDurationEvent);

            packageAdapter.VerifySet(package => package.Weight = It.IsAny<double>(), Times.Never);
        }

        [Fact]
        public void AutoWeighService_ReturnsTrue_WhenWeightIsApplied()
        {
            var shipment = Create.Shipment().Build();
            var shipments = new List<ShipmentEntity>() { shipment, shipment };

            var packageAdapter = mock.Mock<IPackageAdapter>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdapters())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);

            mock.Mock<IScaleReader>()
                .Setup(s => s.ReadScale())
                .ReturnsAsync(ScaleReadResult.Success(42, ScaleType.Usb));

            bool returnValue = testObject.ApplyWeight(shipments, trackedDurationEvent);

            Assert.True(returnValue);
        }

        [Fact]
        public void AutoWeighService_DoesNotSetWeightOnAllPackages_WhenWeightIsLessThanFiveHundredthsOfAnOunce()
        {
            var shipment = Create.Shipment().Build();
            var shipments = new List<ShipmentEntity>() { shipment, shipment };

            var packageAdapter = mock.Mock<IPackageAdapter>();
            packageAdapter.SetupGet(x => x.Weight).Returns(100);

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdapters())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);

            mock.Mock<IScaleReader>()
                .Setup(s => s.ReadScale())
                .ReturnsAsync(ScaleReadResult.Success(.002, ScaleType.Usb));

            testObject.ApplyWeight(shipments, trackedDurationEvent);

            packageAdapter.VerifySet(package => package.Weight = It.IsAny<double>(), Times.Never);
        }

        [Fact]
        public void AutoWeighService_ReturnsFalse_WhenStatusFromScaleIsNotSuccess()
        {
            var shipment = Create.Shipment().Build();
            var shipments = new List<ShipmentEntity>() { shipment, shipment };

            var packageAdapter = mock.Mock<IPackageAdapter>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdapters())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);

            mock.Mock<IScaleReader>()
                .Setup(s => s.ReadScale())
                .ReturnsAsync(ScaleReadResult.ReadError("blah", ScaleType.Usb));

            bool returnValue = testObject.ApplyWeight(shipments, trackedDurationEvent);

            Assert.False(returnValue);
        }

        [Fact]
        public void AutoWeighService_SendsYesTelemetry_WhenWeightIsApplied()
        {
            var shipment = Create.Shipment().Build();
            var shipments = new List<ShipmentEntity>() { shipment, shipment };

            var packageAdapter = mock.Mock<IPackageAdapter>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdapters())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);

            mock.Mock<IScaleReader>()
                .Setup(s => s.ReadScale())
                .ReturnsAsync(ScaleReadResult.Success(42, ScaleType.Usb));

            testObject.ApplyWeight(shipments, trackedDurationEvent);

            mock.Mock<ITrackedDurationEvent>()
                .Verify(t => t.AddProperty(AutoWeighService.TelemetryPropertyName, "Yes"), Times.Once);
        }

        private void SetAutoWeighSetting(bool allowAutoWeigh)
        {
            mock.Mock<ISingleScanAutomationSettings>()
                .Setup(p => p.IsAutoWeighEnabled())
                .Returns(allowAutoWeigh);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}