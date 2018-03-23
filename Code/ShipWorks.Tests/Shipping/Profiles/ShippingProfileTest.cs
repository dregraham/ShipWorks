using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
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
        public void Constructor_LoadProfileDataIsCalledWithTrue_WhenProfileEntityPassedIn()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity();
            ShortcutEntity shortcut = new ShortcutEntity();

            CreateShippingProfile(profile, shortcut);
            mock.Mock<IShippingProfileLoader>().Verify(l => l.LoadProfileData(profile, true), Times.Once);
        }

        [Fact]
        public void Constructor_LoadProfileDataIsCalledWithFalse_WhenNoProfileEntityPassedIn()
        {
            var loaderMock = mock.Mock<IShippingProfileLoader>();

            // Testing 
            new ShippingProfile(loaderMock.Object, mock.Mock<IShippingProfileApplicationStrategyFactory>().Object,
                mock.Mock<IShippingManager>().Object, mock.Mock<IMessenger>().Object, mock.Mock<ICarrierShipmentAdapterFactory>().Object);
            
            loaderMock.Verify(l=>l.LoadProfileData(It.IsAny<ShippingProfileEntity>(), false), Times.Once);
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
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift,
                VirtualKey = VirtualKeys.N0
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
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift,
                VirtualKey = VirtualKeys.N0
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal("Ctrl+Shift+0", testObject.ShortcutKey);
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
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift,
                VirtualKey = VirtualKeys.N0
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

            // The initial set 
            mock.Mock<IShippingProfileLoader>().Verify(l => l.LoadProfileData(profile, true), Times.Exactly(2));
        }

        [Fact]
        public void Apply_DelegatesToShippingManagerChangeShipmentType_WhenShipmentAndProfilesShipmentTypesDontMatch()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon};
            ShipmentEntity shipment = new ShipmentEntity {ShipmentTypeCode = ShipmentTypeCode.FedEx};
            var shippingManager = mock.Mock<IShippingManager>();
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());
            
            testObject.Apply(shipment);
            
            shippingManager.Verify(m => m.ChangeShipmentType(ShipmentTypeCode.Amazon, shipment), Times.Once);
        } 
        
        [Fact]
        public void Apply_DoesNotDelegatesToShippingManagerChangeShipmentType_WhenShipmentAndProfilesShipmentTypesMatch()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon};
            ShipmentEntity shipment = new ShipmentEntity {ShipmentTypeCode = ShipmentTypeCode.Amazon};
            var shippingManager = mock.Mock<IShippingManager>();
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());
            
            testObject.Apply(shipment);
            
            shippingManager.Verify(m => m.ChangeShipmentType(It.IsAny<ShipmentTypeCode>(), shipment), Times.Never);
        }
        
        [Fact]
        public void Apply_CreatesProfileApplicationStrategyUsingStrategyFactory()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon};
            ShipmentEntity shipment = new ShipmentEntity {ShipmentTypeCode = ShipmentTypeCode.Amazon};
            var shippingProfileApplicationStrategyFactory = mock.Mock<IShippingProfileApplicationStrategyFactory>();
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());
            
            testObject.Apply(shipment);
            
            shippingProfileApplicationStrategyFactory.Verify(f => f.Create(ShipmentTypeCode.Amazon), Times.Once);
        }
        
        [Fact]
        public void Apply_UsesStrategyToApplyProfile()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon};
            ShipmentEntity shipment = new ShipmentEntity {ShipmentTypeCode = ShipmentTypeCode.Amazon};
            var strategy = mock.Mock<IShippingProfileApplicationStrategy>();
            mock.Mock<IShippingProfileApplicationStrategyFactory>().Setup(f => f.Create(ShipmentTypeCode.Amazon)).Returns(strategy);
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());
            
            testObject.Apply(shipment);
            
            strategy.Verify(s => s.ApplyProfile(profile, shipment), Times.Once);
        }
                
        [Fact]
        public void Apply_DelegatesToCarrierShipmentAdapterFactory()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon};
            ShipmentEntity shipment = new ShipmentEntity {ShipmentTypeCode = ShipmentTypeCode.FedEx};

            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());
            
            testObject.Apply(shipment);

            mock.Mock<ICarrierShipmentAdapterFactory>().Verify(c => c.Get(shipment));
        }
        
        [Fact]
        public void Apply_SendsProfileAppliedMessage()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon};
            ShipmentEntity shipment = new ShipmentEntity {ShipmentTypeCode = ShipmentTypeCode.FedEx};
            var messenger = mock.Mock<IMessenger>();
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());
            
            testObject.Apply(shipment);

            messenger.Verify(m => m.Send(It.IsAny<ProfileAppliedMessage>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ChangeShortcut_SetsShortcutEntityValues()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(new KeyboardShortcutData(KeyboardShortcutCommand.ApplyWeight, VirtualKeys.A, KeyboardShortcutModifiers.Alt), "abcd");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(VirtualKeys.A, testObject.Shortcut.VirtualKey);
            Assert.Equal(KeyboardShortcutModifiers.Alt, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        [Fact]
        public void ChangeShortcut_SetsShortcutEntityValues_WhenKeyboardShortcutValuesAreNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(new KeyboardShortcutData(null, null, null), "abcd");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(null, testObject.Shortcut.VirtualKey);
            Assert.Equal(null, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        [Fact]
        public void ChangeShortcut_SetsShortcutEntityValues_WhenKeyboardShortcutIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(null, "abcd");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(null, testObject.Shortcut.VirtualKey);
            Assert.Equal(null, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        [Fact]
        public void ChangeShortcut_TrimsBarcode()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            ShippingProfile testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(null, "           abcd  ");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(null, testObject.Shortcut.VirtualKey);
            Assert.Equal(null, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        private ShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut)
        {
            var shippingProfile = mock.Create<ShippingProfile>();
            shippingProfile.ShippingProfileEntity = profile;
            shippingProfile.Shortcut = shortcut;
            return shippingProfile;
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
