using System;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Profiles
{
    public class ShippingProfileTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IShippingProfileManager> shippingProfileManagerMock;
        private readonly Mock<IShortcutManager> shortcutManagerMock;

        public ShippingProfileTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingProfileManagerMock = mock.Mock<IShippingProfileManager>();
            shortcutManagerMock = mock.Mock<IShortcutManager>();
        }

        [Fact]
        public void ShippingProfile_ShipmentTypeDescriptionIsShipmentTypeDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, EnumHelper.GetDescription(profile.ShipmentType));
        }

        [Fact]
        public void ShippingProfile_ShortcutKey_IsShortcutKeyDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShortcutKey, EnumHelper.GetDescription(shortcut.Hotkey));
        }

        [Fact]
        public void ShippingProfile_ShortcutKeyIsBlank_WhenShortcutIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShippingProfile testObject = CreateShippingProfile(profile, null);
            Assert.Equal(testObject.ShortcutKey, string.Empty);
        }

        [Fact]
        public void ShippingProfile_ShipmentTypeDescriptionIsBlank_WhenShipmentTypeIsNull()
        {
            ShortcutEntity shortcut = new ShortcutEntity
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = null
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, string.Empty);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenProfileNameIsEmpty()
        {
            var testObject = CreateShippingProfile(new ShippingProfileEntity(), new ShortcutEntity());
            var result = Validate(testObject);
            
            Assert.True(result.Failure);
            Assert.Equal("Enter a name for the profile.", result.Message);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenAProfileWithSameNameExists()
        {
            shippingProfileManagerMock
                .SetupGet(m => m.Profiles)
                .Returns(new[] { new ShippingProfileEntity { Name = "same" } });
            
            var testObject = CreateShippingProfile(new ShippingProfileEntity { ShippingProfileID = 5, Name = "same" },
                new ShortcutEntity());
            
            var result = Validate(testObject);
            
            Assert.True(result.Failure);
            Assert.Equal("A profile with the chosen name already exists.", result.Message);
        }

        [Fact]
        public void Validate_ReturnsTrue_WhenBarcodeIsBlank()
        {
            shortcutManagerMock
                .SetupGet(m => m.Shortcuts)
                .Returns(new[] { new ShortcutEntity { Barcode = "" } });

            var testObject = CreateShippingProfile(new ShippingProfileEntity { ShippingProfileID = 5, Name = "name" },
                new ShortcutEntity { Barcode = "" });

            var result = Validate(testObject);

            Assert.True(result.Success);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenAShortcutWithSameBarcodeExists()
        {
            shortcutManagerMock
                .SetupGet(m => m.Shortcuts)
                .Returns(new[] { new ShortcutEntity { Barcode = "same" } });

            var testObject = CreateShippingProfile(new ShippingProfileEntity { Name = "blah" }, new ShortcutEntity
            {
                ShortcutID = 42,
                Barcode = "same"
            });
            
            var result = Validate(testObject);
            
            Assert.True(result.Failure);
            Assert.Equal("The barcode \"same\" is already in use.", result.Message);
        }

        [Theory]
        [InlineData(null, ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.Usps, null)]
        [InlineData(ShipmentTypeCode.FedEx, ShipmentTypeCode.UpsOnLineTools)]
        public void ChangeProvider_ChangesShipmentType(ShipmentTypeCode? initialShipmentType, ShipmentTypeCode? newShipmentType)
        {
            var testObject = CreateShippingProfile(new ShippingProfileEntity { ShipmentType = initialShipmentType },
                new ShortcutEntity());
            
            testObject.ChangeProvider(newShipmentType);
            
            Assert.Equal(newShipmentType, testObject.ShippingProfileEntity.ShipmentType);
        }

        [Fact]
        public void ChangeProvider_ClearsPackages()
        {
            var profile = new ShippingProfileEntity();
            profile.Packages.Add(new UpsProfilePackageEntity());
                
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());
            
            testObject.ChangeProvider(ShipmentTypeCode.Amazon);
            Assert.Empty(profile.Packages);
        }

        [Fact]
        public void ChangeProvider_DelegatesToProfileLoader()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity();
            ShortcutEntity shortcut = new ShortcutEntity();
            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            
            testObject.ChangeProvider(ShipmentTypeCode.Endicia);
            
            mock.Mock<IShippingProfileLoader>().Verify(l=>l.LoadProfileData(profile, true), Times.Once);
        }

        private ShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut)
        {
            return mock.Create<ShippingProfile>(TypedParameter.From(profile), TypedParameter.From(shortcut));
        }
        
        private Result Validate(ShippingProfile testObject)
        {
            return testObject.Validate(shippingProfileManagerMock.Object, shortcutManagerMock.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
