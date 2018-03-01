using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Profiles
{
    public class ShippingProfileTest : IDisposable
    {
        private readonly AutoMock mock;

        public ShippingProfileTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ShippingProfile_ShipmentTypeDescriptionIsShipmentTypeDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, EnumHelper.GetDescription(profile.ShipmentType));
        }

        [Fact]
        public void ShippingProfile_ShortcutKey_IsShortcutKeyDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShortcutKey, EnumHelper.GetDescription(shortcut.Hotkey));
        }

        [Fact]
        public void ShippingProfile_ShortcutKeyIsBlank_WhenShortcutIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShippingProfile testObject = CreateShippingProfile(profile, null);
            Assert.Equal(testObject.ShortcutKey, string.Empty);
        }

        [Fact]
        public void ShippingProfile_ShipmentTypeDescriptionIsBlank_WhenShipmentTypeIsNull()
        {
            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfileEntity profile = new ShippingProfileEntity()
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
            var result = testObject.Validate();
            
            Assert.True(result.Failure);
            Assert.Equal("Enter a name for the profile.", result.Message);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenAProfileWithSameNameExists()
        {
            mock.Mock<IShippingProfileManager>()
                .SetupGet(m => m.Profiles)
                .Returns(new[] { new ShippingProfileEntity() { Name = "same" } });
            
            var testObject = CreateShippingProfile(new ShippingProfileEntity() { ShippingProfileID = 5, Name = "same" },
                new ShortcutEntity());
            
            var result = testObject.Validate();
            
            Assert.True(result.Failure);
            Assert.Equal("A profile with the chosen name already exists.", result.Message);
        }
        
        [Fact]
        public void Validate_ReturnsFailure_WhenAShortcutWithSameBarcodeExists()
        {
            mock.Mock<IShortcutManager>()
                .SetupGet(m => m.Shortcuts)
                .Returns(new[] { new ShortcutEntity() { Barcode = "same" } });

            var testObject = CreateShippingProfile(new ShippingProfileEntity() { Name = "blah" }, new ShortcutEntity()
            {
                ShortcutID = 42,
                Barcode = "same"
            });
            
            var result = testObject.Validate();
            
            Assert.True(result.Failure);
            Assert.Equal("The barcode \"same\" is already in use.", result.Message);
        }

        [Theory]
        [InlineData(null, ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.Usps, null)]
        [InlineData(ShipmentTypeCode.FedEx, ShipmentTypeCode.UpsOnLineTools)]
        public void ChangeProvider_ChangesShipmentType(ShipmentTypeCode? initialShipmentType, ShipmentTypeCode? newShipmentType)
        {
            var testObject = CreateShippingProfile(new ShippingProfileEntity() { ShipmentType = initialShipmentType },
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
            return new ShippingProfile(profile, shortcut, mock.Mock<IShippingProfileManager>().Object,
                mock.Mock<IShortcutManager>().Object, mock.Mock<IShippingProfileLoader>().Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
