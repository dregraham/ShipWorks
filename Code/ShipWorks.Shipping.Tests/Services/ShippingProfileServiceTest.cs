using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShippingProfileServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        
        public ShippingProfileServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetAll_ReturnsShortcutAssociatedWithProfile()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity { ShippingProfileID = 42 };

            var shortcuts = new[] { shortcut };
            var profiles = new[] { profile };

            mock.Mock<IShortcutManager>().SetupGet(m => m.Shortcuts).Returns(shortcuts);
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(profiles);

            var shippingProfiles = mock.Create<ShippingProfileService>().GetAll();

            ShippingProfile shippingProfile = shippingProfiles.Single();

            Assert.Equal(shortcut, shippingProfile.Shortcut);
            Assert.Equal(profile, shippingProfile.ShippingProfileEntity);
        }

        [Fact]
        public void Get_ReturnsShortcutAssociatedWithProfile()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity { ShippingProfileID = 42 };

            var shortcuts = new[] { shortcut };
            var profiles = new[] { profile };

            mock.Mock<IShortcutManager>().SetupGet(m => m.Shortcuts).Returns(shortcuts);
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(profiles);

            ShippingProfile shippingProfile = mock.Create<ShippingProfileService>().Get(42);

            Assert.Equal(shortcut, shippingProfile.Shortcut);
            Assert.Equal(profile, shippingProfile.ShippingProfileEntity);
        }

        [Fact]
        public void Create_ReturnsNewShippingProfile()
        {
            var newProfile = mock.Create<ShippingProfileService>().CreateEmptyShippingProfile();
            
            Assert.True(newProfile.Shortcut.IsNew);
            Assert.Equal((int) KeyboardShortcutCommand.ApplyProfile, newProfile.Shortcut.Action);
            
            Assert.True(newProfile.ShippingProfileEntity.IsNew);
            Assert.Equal(string.Empty, newProfile.ShippingProfileEntity.Name);
            Assert.False(newProfile.ShippingProfileEntity.ShipmentTypePrimary);
        }

        [Fact]
        public void Save_DelegatesToManagers()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShippingProfileID = 42,
                Name = "name"
            };

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.Save(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Success);
            mock.Mock<IShippingProfileManager>().Verify(m => m.SaveProfile(profile, sqlAdapter.Object), Times.Once);
            mock.Mock<IShortcutManager>().Verify(m => m.Save(shortcut, sqlAdapter.Object), Times.Once);
        }

        [Fact]
        public void Save_CommitsTransaction()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShippingProfileID = 42,
                Name = "name"
            };

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            var testObject = mock.Create<ShippingProfileService>();
            testObject.Save(CreateShippingProfile(profile, shortcut));

            sqlAdapter.Verify(a=>a.Commit(), Times.Once);
        }

        [Fact]
        public void Save_ReturnsFailure_WhenAdapterThrowsORMConcurrencyException()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShippingProfileID = 42,
                Name = "name"
            };

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            mock.Mock<IShippingProfileManager>()
                .Setup(m => m.SaveProfile(profile, sqlAdapter.Object))
                .Throws(new ORMConcurrencyException("blah", profile));

            ShippingProfile shippingProfile = CreateShippingProfile(profile, shortcut);

            var testObject = mock.Create<ShippingProfileService>();

            var result = testObject.Save(shippingProfile);

            Assert.Equal("Your changes cannot be saved because another use has deleted the profile.", result.Message);
        }

        [Fact]
        public void Save_ReturnsFailure_WhenAdapterThrowsORMQueryExecutionException()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShippingProfileID = 42,
                Name = "name"
            };

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            mock.Mock<IShippingProfileManager>()
                .Setup(m => m.SaveProfile(profile, sqlAdapter.Object))
                .Throws(new ORMQueryExecutionException("","",null, null, null));

            ShippingProfile shippingProfile = CreateShippingProfile(profile, shortcut);

            var testObject = mock.Create<ShippingProfileService>();

            var result = testObject.Save(shippingProfile);

            Assert.True(result.Failure);
            Assert.Equal("An error ocurred saving your profile.", result.Message);
        }

        [Fact]
        public void Save_ReturnsFailure_WhenProfileHasNoName()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity { ShippingProfileID = 42 };

            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.Save(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Failure);
            Assert.Equal("Enter a name for the profile.", result.Message);
        }

        [Fact]
        public void Save_ReturnsFailure_WhenProfileNameIsUsed()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShippingProfileID = 42,
                Name = "blah"
            };

            mock.Mock<IShippingProfileManager>().Setup(m => m.Profiles)
                .Returns(new[] { new ShippingProfileEntity { Name = "blah" } });

            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.Save(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Failure);
            Assert.Equal("A profile with the chosen name already exists.", result.Message);
        }

        [Fact]
        public void Save_ReturnsFailure_WhenBarcodeIsUsed()
        {
            ShortcutEntity shortcut = new ShortcutEntity
            {
                ShortcutID = 42,
                Barcode = "blip"
            };
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                Name = "blah"
            };

            mock.Mock<IShortcutManager>().SetupGet(m => m.Shortcuts)
                .Returns(new[] { new ShortcutEntity { Barcode = "blip" } });

            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.Save(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Failure);
            Assert.Equal("The barcode \"blip\" is already in use.", result.Message);
        }

        [Fact]
        public void Delete_DelegatesToManager()
        {

            ShortcutEntity shortcut = new ShortcutEntity();
            ShippingProfileEntity profile = new ShippingProfileEntity();

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.Delete(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Success);
            mock.Mock<IShippingProfileManager>().Verify(m => m.DeleteProfile(profile, sqlAdapter.Object), Times.Once);
            mock.Mock<IShortcutManager>().Verify(m => m.Delete(shortcut, sqlAdapter.Object), Times.Once);
        }

        [Fact]
        public void Delete_CommitsTransaction()
        {

            ShortcutEntity shortcut = new ShortcutEntity();
            ShippingProfileEntity profile = new ShippingProfileEntity();

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            var testObject = mock.Create<ShippingProfileService>();
            testObject.Delete(CreateShippingProfile(profile, shortcut));

            sqlAdapter.Verify(a=>a.Commit(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsFailure_WhenOrmExceptionThrown()
        {
            ShortcutEntity shortcut = new ShortcutEntity();
            ShippingProfileEntity profile = new ShippingProfileEntity();

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());
            
            mock.Mock<IShippingProfileManager>()
                .Setup(m => m.DeleteProfile(profile, sqlAdapter.Object))
                .Throws(new ORMQueryExecutionException("", "", null, null, null));

            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.Delete(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Failure);
            Assert.Equal("An error occured when deleting the profile.", result.Message);
        }

        [Fact]
        public void GetAvailableHotkeys_ReturnsExistingHotkeysAndHotkeyOfCurrentProfile()
        {
            mock.Mock<IShortcutManager>()
                .Setup(m => m.GetAvailableHotkeys())
                .Returns(new List<Hotkey> { Hotkey.CtrlShift0 });

            ShortcutEntity shortcut = new ShortcutEntity
            {
                Hotkey = Hotkey.CtrlShift1
            };


            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.GetAvailableHotkeys(CreateShippingProfile(null, shortcut));

            Assert.Equal(2, result.Count());
            Assert.Contains(Hotkey.CtrlShift0, result);
            Assert.Contains(Hotkey.CtrlShift1, result);
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