using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateDiscrepancyViewModelTest : IDisposable
    {
        private readonly AutoMock mock;

        public UpsLocalRateDiscrepancyViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Load_LoadsMessage()
        {
            var testObject = mock.Create<UpsLocalRateDiscrepancyViewModel>();
            testObject.Load("msg", new Uri("http://www.shipworks.com/"));

            Assert.Equal("msg", testObject.Message);
        }

        [Fact]
        public void Load_LoadsUri()
        {
            var testObject = mock.Create<UpsLocalRateDiscrepancyViewModel>();
            Uri helpArticleUrl = new Uri("http://www.shipworks.com/");
            testObject.Load("msg", helpArticleUrl);

            Assert.Equal(helpArticleUrl, testObject.HelpArticleUrl);
        }

        [Fact]
        public void SnoozeClickCommand_DelegatesToLocalRateValidator()
        {
            var testObject = mock.Create<UpsLocalRateDiscrepancyViewModel>();
            testObject.Load("msg", new Uri("http://www.shipworks.com/"));
            testObject.SnoozeClickCommand.Execute(null);

            mock.Mock<IUpsLocalRateValidator>().Verify(v=>v.Snooze(), Times.Once);
        }

        [Fact]
        public void SnoozeClickCommand_ExecutesCloseAction()
        {
            var testObject = mock.Create<UpsLocalRateDiscrepancyViewModel>();
            testObject.Load("msg", new Uri("http://www.shipworks.com/"));

            bool closed = false;
            testObject.Close = () => closed = true;

            testObject.SnoozeClickCommand.Execute(null);

            Assert.True(closed);
        }

        [Fact]
        public void CloseClickCommand_ExecutesCloseAction()
        {
            var testObject = mock.Create<UpsLocalRateDiscrepancyViewModel>();
            testObject.Load("msg", new Uri("http://www.shipworks.com/"));

            bool closed = false;
            testObject.Close = () => closed = true;

            testObject.CloseClickCommand.Execute(null);

            Assert.True(closed);
        }

        [Fact]
        public void CloseClickCommand_DoesNotCallSnoozeOnLocalRateValidator()
        {
            var testObject = mock.Create<UpsLocalRateDiscrepancyViewModel>();
            testObject.Load("msg", new Uri("http://www.shipworks.com/"));
            testObject.CloseClickCommand.Execute(null);

            mock.Mock<IUpsLocalRateValidator>().Verify(v => v.Snooze(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}