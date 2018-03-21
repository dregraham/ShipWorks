using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.Profiles;
using ShipWorks.Templates.Printing;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Sdk;
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
            var profileEntity = new ShippingProfileEntity() { ShippingProfileID = 42 };
            var shortcut = new ShortcutEntity();
            var profile = CreateShippingProfile(profileEntity, shortcut);
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal(profile, testObject.ShippingProfiles.Single());
        }

        [Fact]
        public void Constructor_ProfilesAssociatedWithCorrectShortcut_FromShortcutManager()
        {
            var profileEntity = new ShippingProfileEntity() { ShippingProfileID = 42 };
            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, VirtualKey = null, ModifierKeys = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal("Ctrl + Shift + D", testObject.ShippingProfiles.Single().ShortcutKey);
        }

        [Fact]
        public void Constructor_ProfileShortcutKeyIsBlank_WhenNoMatchingShortcut()
        {
            var profileEntity = new ShippingProfileEntity() { ShippingProfileID = 42 };
            var shortcut = new ShortcutEntity();
            var profile = CreateShippingProfile(profileEntity, shortcut);
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });
            
            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal(string.Empty, testObject.ShippingProfiles.Single().ShortcutKey);
        }

        [Fact]
        public void Constructor_ProfileShortcutKeyIsBlank_WhenAssociatedShortutKeyIsNull()
        {
            var profileEntity = new ShippingProfileEntity() { ShippingProfileID = 42 };
            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, ModifierKeys = null, VirtualKey = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });
            
            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count());
            Assert.Equal(string.Empty, testObject.ShippingProfiles.Single().ShortcutKey);
        }

        [Fact]
        public void Constructor_ShippingProfilesContainsGlobalTypes()
        {
            var profileEntity = new ShippingProfileEntity()
            {
                ShippingProfileID = 42,
                ShipmentType = null
            };

            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, VirtualKey = null, ModifierKeys = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);

            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count);
        }

        [Fact]
        public void Constructor_ShippingProfilesContainsBestRate_WhenShipmentTypeAllowed()
        {
            var profileEntity = new ShippingProfileEntity
            {
                ShippingProfileID = 42,
                ShipmentType = ShipmentTypeCode.BestRate
            };

            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, ModifierKeys = null, VirtualKey = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);

            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile> { profile });

            mock.Mock<IShipmentTypeManager>().SetupGet(s => s.ConfiguredShipmentTypeCodes).Returns(new[] { ShipmentTypeCode.BestRate });
            
            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

            Assert.Equal(1, testObject.ShippingProfiles.Count);
        }

        [Fact]
        public void Delete_DelegatesToShippingProfileManager_WhenUserAnswersQuestionOK()
        {
            mock.Mock<IMessageHelper>().Setup(m => m.ShowQuestion(AnyString)).Returns(DialogResult.OK);
            var profileEntity = new ShippingProfileEntity();
            var profile = CreateShippingProfile(profileEntity, null);
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();
            testObject.SelectedShippingProfile = profile;

            testObject.DeleteCommand.Execute(null);

            mock.Mock<IShippingProfileService>().Verify(m=>m.Delete(profile));
        }

        [Fact]
        public void Delete_DoesNotDelegateToShippingProfileManager_WhenUserAnswersQuestionNo()
        {
            mock.Mock<IMessageHelper>().Setup(m => m.ShowQuestion(AnyString)).Returns(DialogResult.No);

            var profileEntity = new ShippingProfileEntity();
            var profile = CreateShippingProfile(profileEntity, null);
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();
            testObject.SelectedShippingProfile = profile;

            testObject.DeleteCommand.Execute(null);

            mock.Mock<IShippingProfileService>().Verify(m => m.Delete(profile), Times.Never);
        }

        [Fact]
        public void PrintBarCodes_DelegatesToPrintJobFactory()
        {
            var profileEntity = new ShippingProfileEntity();
            var profile = CreateShippingProfile(profileEntity, new ShortcutEntity { Barcode = "blah" });
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles())
                .Returns(new List<ShippingProfile>() { profile });

            var form = new Form();

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>(new TypedParameter(typeof(IWin32Window), form));

            testObject.PrintBarcodesCommand.Execute(null);

            mock.Mock<IPrintJobFactory>().Verify(p => p.CreateBarcodePrintJob(testObject.ShippingProfiles));
        }

        [Fact]
        public void PrintBarCodes_DelegatesToIPrintJobForPreview()
        {
            var profileEntity = new ShippingProfileEntity();
            var profile = CreateShippingProfile(profileEntity, new ShortcutEntity() { Barcode = "blah" });
            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });
            var printJob = mock.Mock<IPrintJob>();
            mock.Mock<IPrintJobFactory>().Setup(f => f.CreateBarcodePrintJob(It.IsAny<IEnumerable<IShippingProfile>>())).Returns(printJob);
            var form = new Form();

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>(new TypedParameter(typeof(IWin32Window), form));

            testObject.PrintBarcodesCommand.Execute(null);

            printJob.Verify(p => p.PreviewAsync(form));
        }

        private ShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut)
        {
            var shippingProfile = mock.Create<ShippingProfile>(TypedParameter.From(profile), TypedParameter.From(shortcut));
            shippingProfile.ShippingProfileEntity = profile;
            shippingProfile.Shortcut = shortcut;
            return shippingProfile;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}