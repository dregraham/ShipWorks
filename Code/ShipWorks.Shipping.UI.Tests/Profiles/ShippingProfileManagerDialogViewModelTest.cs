using System;
using System.Linq;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
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

        //[Fact]
        //public void Constructor_PopulatesShippingProfiles_FromShippingProfileManager()
        //{
        //    var profile = new ShippingProfileEntity() {ShippingProfileID = 42};
        //    mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });

        //    var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();

        //    Assert.Equal(1, testObject.ShippingProfiles.Count());
        //    //Assert.Equal(profile, testObject.ShippingProfiles.Single());
        //}

        [Fact]
        public void Delete_DelegatesToShippingProfileManager_WhenUserAnswersQuestionYes()
        {
            mock.Mock<IMessageHelper>().Setup(m => m.ShowQuestion(AnyString)).Returns(DialogResult.Yes);
            var profile = new ShippingProfileEntity();
            mock.Mock<IShippingProfileManager>().SetupGet(m => m.Profiles).Returns(new[] { profile });

            var testObject = mock.Create<ShippingProfileManagerDialogViewModel>();
            testObject.SelectedShippingProfile = profile;

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
            testObject.SelectedShippingProfile = profile;

            testObject.DeleteCommand.Execute(null);

            mock.Mock<IShippingProfileManager>().Verify(m => m.DeleteProfile(profile), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}