using Autofac;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class EditableShippingProfileRepositoryTest
    {
        private readonly AutoMock mock;

        public EditableShippingProfileRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Delete_DelegatesToManager()
        {
            ShortcutEntity shortcut = new ShortcutEntity();
            ShippingProfileEntity profile = new ShippingProfileEntity();

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            var testObject = mock.Create<EditableShippingProfileRepository>();
            var result = testObject.Delete(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Success);
            mock.Mock<IShippingProfileManager>().Verify(m => m.DeleteProfile(profile, sqlAdapter.Object), Times.Once);
            mock.Mock<IShortcutManager>().Verify(m => m.Delete(shortcut, sqlAdapter.Object), Times.Once);
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

            var testObject = mock.Create<EditableShippingProfileRepository>();
            var result = testObject.Delete(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Failure);
            Assert.Equal("An error occurred when deleting the profile.", result.Message);
        }

        [Fact]
        public void Delete_CommitsTransaction()
        {

            ShortcutEntity shortcut = new ShortcutEntity();
            ShippingProfileEntity profile = new ShippingProfileEntity();

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.CreateTransacted());

            var testObject = mock.Create<EditableShippingProfileRepository>();
            testObject.Delete(CreateShippingProfile(profile, shortcut));

            sqlAdapter.Verify(a => a.Commit(), Times.Once);
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

            mock.Create<EditableShippingProfileRepository>().GetAll();

            mock.Mock<IShippingProfileFactory>().Verify(s => s.CreateEditable(profile, shortcut));
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

            mock.Create<EditableShippingProfileRepository>().Get(42);

            mock.Mock<IShippingProfileFactory>().Verify(s => s.CreateEditable(profile, shortcut));
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

            var testObject = mock.Create<EditableShippingProfileRepository>();
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

            var testObject = mock.Create<EditableShippingProfileRepository>();
            testObject.Save(CreateShippingProfile(profile, shortcut));

            sqlAdapter.Verify(a => a.Commit(), Times.Once);
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

            var shippingProfile = CreateShippingProfile(profile, shortcut);

            var testObject = mock.Create<EditableShippingProfileRepository>();

            var result = testObject.Save(shippingProfile);

            Assert.Equal("Your changes cannot be saved because another user has deleted the profile.", result.Message);
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
                .Throws(new ORMQueryExecutionException("", "", null, null, null));

            var shippingProfile = CreateShippingProfile(profile, shortcut);

            var testObject = mock.Create<EditableShippingProfileRepository>();

            var result = testObject.Save(shippingProfile);

            Assert.True(result.Failure);
            Assert.Equal("An error occurred saving your profile.", result.Message);
        }

        [Fact]
        public void Save_ReturnsFailure_WhenProfileHasNoName()
        {
            ShortcutEntity shortcut = new ShortcutEntity { RelatedObjectID = 42 };
            ShippingProfileEntity profile = new ShippingProfileEntity { ShippingProfileID = 42 };

            var testObject = mock.Create<EditableShippingProfileRepository>();
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

            var testObject = mock.Create<EditableShippingProfileRepository>();
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

            var testObject = mock.Create<EditableShippingProfileRepository>();
            var result = testObject.Save(CreateShippingProfile(profile, shortcut));

            Assert.True(result.Failure);
            Assert.Equal("The barcode \"blip\" is already in use.", result.Message);
        }

        private EditableShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut) =>
            mock.Create<EditableShippingProfile>(TypedParameter.From(profile), TypedParameter.From(shortcut));
    }
}
