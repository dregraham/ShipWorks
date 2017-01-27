using System;
using Autofac.Extras.Moq;
using Moq;
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
            testObject.Create("scanmessagetext", "title", "displaytext", "continuetext");

            viewModel.Verify(v => v.Load("scanmessagetext", "displaytext", "continuetext"));
        }

        [Fact]
        public void Create_SetsTextOnDialog()
        {
            Mock<IAutoPrintConfirmationDialog> dialog = mock.Mock<IAutoPrintConfirmationDialog>();
            testObject.Create("scanmessagetext", "title", "displaytext", "continuetext");

            dialog.VerifySet(d => d.Text = "title");
        }
        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}