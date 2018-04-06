using System;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShippingProfileManagerWrapperTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ShippingProfileManagerWrapper testObject;

        public ShippingProfileManagerWrapperTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            testObject = context.Mock.Create<ShippingProfileManagerWrapper>();
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_ReturnsExistingProfile_WhenProfileExists()
        {
            var profile = Create.Profile()
                .AsPrimary()
                .AsOther()
                .Save();

            var loadedProfile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OtherShipmentType>());

            Assert.Equal(profile.ShippingProfileID, loadedProfile.ShippingProfileID);
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SetsDefaultValues_WhenProfileIsCreated()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OtherShipmentType>());

            Assert.Equal("Defaults - Other", profile.Name);
            Assert.Equal(ShipmentTypeCode.Other, profile.ShipmentType);
            Assert.True(profile.ShipmentTypePrimary);
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_DelegatesToShipmentTypeForConfiguration_WhenProfileIsCreated()
        {
            Mock<OtherShipmentType> shipmentType = context.Mock.CreateMock<OtherShipmentType>();
            shipmentType.CallBase = true;

            var profile = testObject.GetOrCreatePrimaryProfile(shipmentType.Object);

            shipmentType.Verify(x => x.ConfigurePrimaryProfile(profile));
        }

        public void Dispose() => context.Dispose();
    }
}
