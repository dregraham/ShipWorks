using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS
{
    public class UpsShippingProfileApplicationStrategyTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ShipmentType> shipmentType;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<IShippingProfileApplicationStrategy> baseShippingProfileApplicationStrategy;
        private readonly UpsShippingProfileApplicationStrategy testObject;

        public UpsShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(shipmentType);
            baseShippingProfileApplicationStrategy = mock.Mock<IShippingProfileApplicationStrategy>();

            testObject = mock.Create<UpsShippingProfileApplicationStrategy>();
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerForShipmentType()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            testObject.ApplyProfile(profile, shipment);

            shipmentTypeManager.Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesDims()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var package = new UpsProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 1,
                DimsWidth = 2,
                DimsHeight = 3
            };

            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.Ups.Packages[0].DimsLength);
            Assert.Equal(2, shipment.Ups.Packages[0].DimsWidth);
            Assert.Equal(3, shipment.Ups.Packages[0].DimsHeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesMultiPackagesDims()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var packageOne = new UpsProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 1,
                DimsWidth = 2,
                DimsHeight = 3
            };

            var packageTwo = new UpsProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 21,
                DimsWidth = 22,
                DimsHeight = 23
            };

            profile.Packages.Add(packageOne);
            profile.Packages.Add(packageTwo);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.Ups.Packages[0].DimsLength);
            Assert.Equal(2, shipment.Ups.Packages[0].DimsWidth);
            Assert.Equal(3, shipment.Ups.Packages[0].DimsHeight);

            Assert.Equal(21, shipment.Ups.Packages[1].DimsLength);
            Assert.Equal(22, shipment.Ups.Packages[1].DimsWidth);
            Assert.Equal(23, shipment.Ups.Packages[1].DimsHeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesDryIce()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var package = new UpsProfilePackageEntity()
            {
                DryIceEnabled = true,
                DryIceIsForMedicalUse = true,
                DryIceRegulationSet = 1,
                DryIceWeight = 2
            };

            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(true, shipment.Ups.Packages[0].DryIceEnabled);
            Assert.Equal(true, shipment.Ups.Packages[0].DryIceIsForMedicalUse);
            Assert.Equal(1, shipment.Ups.Packages[0].DryIceRegulationSet);
            Assert.Equal(2, shipment.Ups.Packages[0].DryIceWeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesVerbalConfirmation()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var package = new UpsProfilePackageEntity()
            {
                VerbalConfirmationEnabled = true,
                VerbalConfirmationName = "Satan",
                VerbalConfirmationPhone = "666-666-6666",
                VerbalConfirmationPhoneExtension = "666"
            };

            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(true, shipment.Ups.Packages[0].VerbalConfirmationEnabled);
            Assert.Equal("Satan", shipment.Ups.Packages[0].VerbalConfirmationName);
            Assert.Equal("666-666-6666", shipment.Ups.Packages[0].VerbalConfirmationPhone);
            Assert.Equal("666", shipment.Ups.Packages[0].VerbalConfirmationPhoneExtension);
        }

        [Fact]
        public void ApplyProfile_RemovesExtraPackagesFromShipment()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Single(shipment.Ups.Packages);
        }

        [Fact]
        public void ApplyProfile_DelegatesToBaseShippingProfileApplicationStrategy()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            baseShippingProfileApplicationStrategy.Verify(b => b.ApplyProfile(profile, shipment));
        }
        
        [Fact]
        public void ApplyProfile_SetsResidentialDetermination()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() { ResidentialDetermination = 2 } };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.ResidentialDetermination);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToUpdateDynamicShipmentData()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() { ResidentialDetermination = 2 } };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.UpdateDynamicShipmentData(shipment));
        }
    }
}
