using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Services;
using ShipWorks.SingleScan.AutoPrintConfirmation;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class AutoPrintConfirmationDlgFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AutoPrintConfirmationDlgFactory testObject;

        public AutoPrintConfirmationDlgFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<AutoPrintConfirmationDlgFactory>();
        }

        [Fact]
        public void Create_LoadsMessageTextOnViewModel()
        {
            Mock<IAutoPrintConfirmationDlgViewModel> viewModel = mock.Mock<IAutoPrintConfirmationDlgViewModel>();
            MessagingText continueText = new MessagingText()
            {
                Body = "displaytext",
                Continue = "continuetext",
                Title = "title"
            };

            testObject.Create("scanmessagetext", continueText);

            viewModel.Verify(v => v.Load("scanmessagetext", "displaytext", "continuetext"));
        }

        [Fact]
        public void Create_SetsTextOnDialog()
        {
            Mock<IAutoPrintConfirmationDialog> dialog = mock.Mock<IAutoPrintConfirmationDialog>();
            MessagingText continueText = new MessagingText()
            {
                Body = "displaytext",
                Continue = "continuetext",
                Title = "title"
            };

            testObject.Create("scanmessagetext", continueText);

            dialog.VerifySet(d => d.Text = "title");
        }
        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}