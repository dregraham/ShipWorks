using System;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Tests.Shipping.Profiles
{
    public class ShippingProfileTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IShippingProfileManager> shippingProfileManagerMock;
        private readonly Mock<IShortcutManager> shortcutManagerMock;
        private Mock<ISecurityContext> securityContext;

        public ShippingProfileTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingProfileManagerMock = mock.Mock<IShippingProfileManager>();
            shortcutManagerMock = mock.Mock<IShortcutManager>();

            securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, It.IsAny<long>())).Returns(true);
        }

        [Fact]
        public void Apply_DelegatesToShippingManagerChangeShipmentType_WhenShipmentAndProfilesShipmentTypesDontMatch()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Usps };
            ShipmentEntity shipment = new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.FedEx };
            var shippingManager = mock.Mock<IShippingManager>();
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.Apply(shipment);

            shippingManager.Verify(m => m.ChangeShipmentType(ShipmentTypeCode.Usps, shipment), Times.Once);
        }

        [Fact]
        public void Apply_DoesNotCreateProfileApplicationStrategyUsingStrategyFactory_WhenUserDoesNotHavePermission()
        {
            securityContext.Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, It.IsAny<long>())).Returns(false);

            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShipmentEntity shipment = new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.Amazon };
            var shippingProfileApplicationStrategyFactory = mock.Mock<IShippingProfileApplicationStrategyFactory>();
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.Apply(shipment);

            shippingProfileApplicationStrategyFactory.Verify(f => f.Create(ShipmentTypeCode.Amazon), Times.Never);
        }

        [Fact]
        public void Apply_DoesNotDelegatesToShippingManagerChangeShipmentType_WhenShipmentAndProfilesShipmentTypesMatch()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShipmentEntity shipment = new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.Amazon };
            var shippingManager = mock.Mock<IShippingManager>();
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.Apply(shipment);

            shippingManager.Verify(m => m.ChangeShipmentType(It.IsAny<ShipmentTypeCode>(), shipment), Times.Never);
        }

        [Fact]
        public void Apply_CreatesProfileApplicationStrategyUsingStrategyFactory()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShipmentEntity shipment = new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.Amazon };
            var shippingProfileApplicationStrategyFactory = mock.Mock<IShippingProfileApplicationStrategyFactory>();
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.Apply(shipment);

            shippingProfileApplicationStrategyFactory.Verify(f => f.Create(ShipmentTypeCode.Amazon), Times.Once);
        }

        [Fact]
        public void Apply_UsesStrategyToApplyProfile()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShipmentEntity shipment = new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.Amazon };
            var strategy = mock.Mock<IShippingProfileApplicationStrategy>();
            mock.Mock<IShippingProfileApplicationStrategyFactory>().Setup(f => f.Create(ShipmentTypeCode.Amazon)).Returns(strategy);
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.Apply(shipment);

            strategy.Verify(s => s.ApplyProfile(profile, shipment), Times.Once);
        }

        [Fact]
        public void Apply_SendsProfileAppliedMessage()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.FedEx };
            ShipmentEntity shipment = new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.FedEx };
            var messenger = mock.Mock<IMessenger>();
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.Apply(shipment);

            messenger.Verify(m => m.Send(It.IsAny<ProfileAppliedMessage>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenShipmentTypeCodeIsAmazonAndProfileIsAmazon()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.True(testObject.IsApplicable(ShipmentTypeCode.Amazon));
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenShipmentTypeCodeIsNotAmazonAndProfileIsNotAmazon()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Usps };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.True(testObject.IsApplicable(ShipmentTypeCode.FedEx));
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenShipmentTypeCodeIsAmazonAndProfileIsGlobal()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = null };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.True(testObject.IsApplicable(ShipmentTypeCode.Amazon));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenShipmentTypeCodeIsAmazonAndProfileUsps()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Usps };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.False(testObject.IsApplicable(ShipmentTypeCode.Amazon));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenShipmentTypeCodeIsUspsAndProfileIsAmazon()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.False(testObject.IsApplicable(ShipmentTypeCode.Usps));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenShipmentTypeCodeIsNoneAndProfileIsGlobal()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = null };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.False(testObject.IsApplicable(ShipmentTypeCode.None));
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenShipmentTypeCodeIsNoneAndProfileIsNotGlobal()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Usps };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.True(testObject.IsApplicable(ShipmentTypeCode.None));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenShipmentTypeCodeIsNoneAndProfileIsAmazon()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            Assert.True(testObject.IsApplicable(ShipmentTypeCode.None));
        }


        private ShippingProfile CreateShippingProfile(IShippingProfileEntity profile, IShortcutEntity shortcut) =>
            mock.Create<ShippingProfile>(TypedParameter.From(profile), TypedParameter.From(shortcut));

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
