using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.UI.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.UI.Tests.Profiles
{
    public class ShippingProfileManagerDialogViewModelTest : IDisposable
    {
        private readonly AutoMock mock;

        public ShippingProfileManagerDialogViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_PopulatesShippingProfiles_FromShippingProfileManager()
        {
            var profile = new ShippingProfileEntity() { ShippingProfileID = 42 };
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });
            mock.Mock<IShortcutManager>().SetupGet(m => m.Shortcuts).Returns(new List<ShortcutEntity>());

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal(profile, testObject.ShippingProfiles.Single().ShippingProfile);
        }

        [Fact]
        public void Constructor_ProfilesAssociatedWithCorrectShortcut_FromShortcutManager()
        {
            var profile = new ShippingProfileEntity() { ShippingProfileID = 42 };
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });
            mock.Mock<IShortcutManager>().SetupGet(m => m.Shortcuts).Returns(new[]
                { new ShortcutEntity() { RelatedObjectID = 42, Hotkey = Hotkey.CtrlShiftD } });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal(EnumHelper.GetDescription(Hotkey.CtrlShiftD), testObject.ShippingProfiles.Single().ShortcutKey);
        }

        [Fact]
        public void Constructor_ProfileShortcutKeyIsBlank_WhenNoMatchingShortcut()
        {
            var profile = new ShippingProfileEntity() { ShippingProfileID = 42 };
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });
            mock.Mock<IShortcutManager>().SetupGet(m => m.Shortcuts).Returns(new[]
                { new ShortcutEntity() { RelatedObjectID = 43, Hotkey = Hotkey.CtrlShiftD } });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal(string.Empty, testObject.ShippingProfiles.Single().ShortcutKey);
        }

        [Fact]
        public void Constructor_ProfileShortcutKeyIsBlank_WhenAssociatedShortutKeyIsNull()
        {
            var profile = new ShippingProfileEntity() { ShippingProfileID = 42 };
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });
            mock.Mock<IShortcutManager>().SetupGet(m => m.Shortcuts).Returns(new[]
                { new ShortcutEntity() { RelatedObjectID = 42, Hotkey = null } });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal(string.Empty, testObject.ShippingProfiles.Single().ShortcutKey);
        }

        [Fact]
        public void Delete_DelegatesToShippingProfileManager_WhenUserAnswersQuestionOK()
        {
            mock.Mock<IMessageHelper>().Setup(m => m.ShowQuestion(AnyString)).Returns(DialogResult.OK);
            var profile = new ShippingProfileEntity();
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();
            testObject.SelectedShippingProfile = new ShippingProfileAndShortcut(profile, null);

            testObject.DeleteCommand.Execute(null);

            mock.Mock<IShippingProfileManager>().Verify(m=>m.DeleteProfile(profile));
        }

        [Fact]
        public void Delete_DoesNotDelegateToShippingProfileManager_WhenUserAnswersQuestionNo()
        {
            mock.Mock<IMessageHelper>().Setup(m => m.ShowQuestion(AnyString)).Returns(DialogResult.No);
            var profile = new ShippingProfileEntity();
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();
            testObject.SelectedShippingProfile = new ShippingProfileAndShortcut(profile, null);

            testObject.DeleteCommand.Execute(null);

            mock.Mock<IShippingProfileManager>().Verify(m => m.DeleteProfile(profile), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}