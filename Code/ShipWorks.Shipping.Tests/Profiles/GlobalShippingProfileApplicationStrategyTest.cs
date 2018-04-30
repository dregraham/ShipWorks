using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Shipping.Tests.Profiles
{
    public class GlobalShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShippingProfileEntity profile;
        private readonly ShipmentEntity shipment;

        public GlobalShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            profile = new ShippingProfileEntity() { Packages = { new PackageProfileEntity() } };
            shipment = new ShipmentEntity();
        }

        [Fact]
        public void ApplyProfile_AppliesProfileWeightToEachPackage()
        {
            var shipmentType = mock.Mock<ShipmentType>();
            var package1 = mock.CreateMock<IPackageAdapter>();
            var package2 = mock.CreateMock<IPackageAdapter>();

            profile.Packages[0].Weight = 42;

            shipmentType.Setup(s => s.GetPackageAdapters(shipment)).Returns(new[] { package1.Object, package2.Object });
            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentType.Object);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            package1.VerifySet(s => s.Weight = 42, Times.Once);
            package2.VerifySet(s => s.Weight = 42, Times.Once);
        }

        [Fact]
        public void ApplyProfile_AppliesProfileIDToEachPackage()
        {
            var shipmentType = mock.Mock<ShipmentType>();
            var package1 = mock.CreateMock<IPackageAdapter>();
            var package2 = mock.CreateMock<IPackageAdapter>();

            profile.Packages[0].DimsProfileID = 42;

            shipmentType.Setup(s => s.GetPackageAdapters(shipment)).Returns(new[] { package1.Object, package2.Object });
            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentType.Object);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            package1.VerifySet(s => s.DimsProfileID = 42, Times.Once);
            package2.VerifySet(s => s.DimsProfileID = 42, Times.Once);
        }

        [Fact]
        public void ApplyProfile_AppliesProfileLengthToEachPackage()
        {
            var shipmentType = mock.Mock<ShipmentType>();
            var package1 = mock.CreateMock<IPackageAdapter>();
            var package2 = mock.CreateMock<IPackageAdapter>();

            profile.Packages[0].DimsLength = 42;

            shipmentType.Setup(s => s.GetPackageAdapters(shipment)).Returns(new[] { package1.Object, package2.Object });
            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentType.Object);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            package1.VerifySet(s => s.DimsLength = 42, Times.Once);
            package2.VerifySet(s => s.DimsLength = 42, Times.Once);
        }

        [Fact]
        public void ApplyProfile_AppliesProfileWidthToEachPackage()
        {
            var shipmentType = mock.Mock<ShipmentType>();

            var package1 = mock.CreateMock<IPackageAdapter>();
            var package2 = mock.CreateMock<IPackageAdapter>();

            profile.Packages[0].DimsWidth = 42;

            shipmentType.Setup(s => s.GetPackageAdapters(shipment)).Returns(new[]
            {
                package1.Object, package2.Object
            });

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentType.Object);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            package1.VerifySet(s => s.DimsWidth = 42, Times.Once);
            package2.VerifySet(s => s.DimsWidth = 42, Times.Once);
        }

        [Fact]
        public void ApplyProfile_AppliesProfileHeightToEachPackage()
        {
            var shipmentType = mock.Mock<ShipmentType>();

            var package1 = mock.CreateMock<IPackageAdapter>();
            var package2 = mock.CreateMock<IPackageAdapter>();

            profile.Packages[0].DimsHeight = 42;

            shipmentType.Setup(s => s.GetPackageAdapters(shipment)).Returns(new[]
            {
                package1.Object, package2.Object
            });

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentType.Object);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            package1.VerifySet(s => s.DimsHeight = 42, Times.Once);
            package2.VerifySet(s => s.DimsHeight = 42, Times.Once);
        }

        [Fact]
        public void ApplyProfile_AppliesProfileAdditionalWeightToEachPackage()
        {
            var shipmentType = mock.Mock<ShipmentType>();

            var package1 = mock.CreateMock<IPackageAdapter>();
            var package2 = mock.CreateMock<IPackageAdapter>();

            profile.Packages[0].DimsWeight = 42;

            shipmentType.Setup(s => s.GetPackageAdapters(shipment)).Returns(new[]
            {
                package1.Object, package2.Object
            });

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentType.Object);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            package1.VerifySet(s => s.AdditionalWeight = 42, Times.Once);
            package2.VerifySet(s => s.AdditionalWeight = 42, Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ApplyProfile_AppliesProfileAddWeightToEachPackage(bool addWeight)
        {
            var shipmentType = mock.Mock<ShipmentType>();

            var package1 = mock.CreateMock<IPackageAdapter>();
            var package2 = mock.CreateMock<IPackageAdapter>();

            profile.Packages[0].DimsAddWeight = addWeight;

            shipmentType.Setup(s => s.GetPackageAdapters(shipment)).Returns(new[]
            {
                package1.Object, package2.Object
            });

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentType.Object);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            package1.VerifySet(s => s.ApplyAdditionalWeight = addWeight, Times.Once);
            package2.VerifySet(s => s.ApplyAdditionalWeight = addWeight, Times.Once);
        }
        
        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            shipmentTypeManager.Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            profile.OriginID = 123;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            profile.ReturnShipment = true;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            profile.RequestedLabelFormat = (int)ThermalLanguage.EPL;

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL ;

            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));
        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            var insuranceChoice = mock.Mock<IInsuranceChoice>();

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new Editing.DimensionsAdapter());

            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<GlobalShippingProfileApplicationStrategy>();
            profile.Insurance = true;

            testObject.ApplyProfile(profile, shipment);

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}