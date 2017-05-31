using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Validation
{
    public class LocalRateValidationResultTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IDialog> mockDialog;
        private readonly Mock<IUpsLocalRateDiscrepancyViewModel> mockViewModel;

        public LocalRateValidationResultTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mockDialog = mock.Mock<IDialog>();
            mockViewModel = mock.Mock<IUpsLocalRateDiscrepancyViewModel>();
        }

        [Fact]
        public void Message_IsBlankIfNoDiscrepancies()
        {
            var testObject = new LocalRateValidationResult(1, 0, mockDialog.Object, mockViewModel.Object);

            Assert.Empty(testObject.Message);
        }

        [Fact]
        public void Message_IsSetCorrectly_WhenSingleDiscrepancy()
        {
            var testObject = new LocalRateValidationResult(1, 1, mockDialog.Object, mockViewModel.Object);

            string expectedMessage =
                "The Ups shipment had local rates that did not match the rates on your UPS account. Please review and update your local rates.";

            Assert.Equal(expectedMessage, testObject.Message);
        }

        [Fact]
        public void Message_IsSetCorrectly_WhenMultipleDiscrepancies()
        {
            var testObject = new LocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);

            string expectedMessage =
                "9 of 10 UPS shipments had local rates that did not match the rates on your UPS account. Please review and update your local rates.";

            Assert.Equal(expectedMessage, testObject.Message);
        }

        [Fact]
        public void ShowMessage_ShowsMessage_WhenDiscrepancy()
        {
            var testObject = new LocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);
            testObject.ShowMessage();

            mockDialog.Verify(d=>d.ShowDialog(), Times.Once);
        }

        [Fact]
        public void ShowMessage_SetsDialogDataContext_WhenDiscrepancy()
        {
            var testObject = new LocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);
            testObject.ShowMessage();

            mockDialog.VerifySet(d => d.DataContext = mockViewModel.Object, Times.Once());
        }

        [Fact]
        public void ShowMessage_LoadsViewModel_WhenDiscrepancy()
        {
            var testObject = new LocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);
            testObject.ShowMessage();

            var expectedUri = new Uri("http://support.shipworks.com/support/solutions/articles/4000103270-ups-local-rating");

            mockViewModel.Verify(vm => vm.Load(testObject.Message, It.Is<Uri>(uri => uri == expectedUri)));
        }

        [Fact]
        public void ShowMessage_DoesNotShowMessage_WhenNoDiscrepancy()
        {
            var testObject = new LocalRateValidationResult(10, 0, mockDialog.Object, mockViewModel.Object);
            testObject.ShowMessage();

            mockDialog.Verify(d => d.ShowDialog(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}