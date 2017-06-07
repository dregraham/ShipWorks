﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Validation
{
    public class FailedLocalRateValidationResultTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IDialog> mockDialog;
        private readonly Mock<IUpsLocalRateDiscrepancyViewModel> mockViewModel;

        public FailedLocalRateValidationResultTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mockDialog = mock.Mock<IDialog>();
            mockViewModel = mock.Mock<IUpsLocalRateDiscrepancyViewModel>();
        }

        [Fact]
        public void Message_IsSetCorrectly_WhenSingleDiscrepancy()
        {
            var testObject = new FailedLocalRateValidationResult(1, 1, mockDialog.Object, mockViewModel.Object);

            string expectedMessage = "The UPS shipment had local rates that did not match the rates on your UPS account. Please review and update your local rates.";

            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            List<string> errors = new List<string> { "blah" };
            workFlowResult.Setup(w => w.NewErrors).Returns(errors);

            testObject.HandleValidationFailure(workFlowResult.Object);

            Assert.Equal(expectedMessage, errors.First());
        }

        [Fact]
        public void HandleValidationFailure_IsSetCorrectly_WhenMultipleDiscrepancies()
        {
            var testObject = new FailedLocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);

            string expectedMessage =
                "9 of the 10 successfully processed UPS shipments had local rates that did not match the rates on your UPS account. Please review and update your local rates.";

            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            List<string> errors = new List<string> {"blah"};
            workFlowResult.Setup(w => w.NewErrors).Returns(errors);

            testObject.HandleValidationFailure(workFlowResult.Object);

            Assert.Equal(expectedMessage, errors.First());
        }

        [Fact]
        public void HandleValidationFailure_ShowsMessage_AndNoOtherErrors()
        {
            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            workFlowResult.Setup(w => w.NewErrors).Returns(new List<string>());

            var testObject = new FailedLocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);
            testObject.HandleValidationFailure(workFlowResult.Object);

            mockDialog.Verify(d=>d.ShowDialog(), Times.Once);
        }

        [Fact]
        public void HandleValidationFailure_SetsDialogDataContext_AndNoOtherErrors()
        {
            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            workFlowResult.Setup(w => w.NewErrors).Returns(new List<string>());

            var testObject = new FailedLocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);
            testObject.HandleValidationFailure(workFlowResult.Object);

            mockDialog.VerifySet(d => d.DataContext = mockViewModel.Object, Times.Once());
        }

        [Fact]
        public void HandleValidationFailure_LoadsViewModel()
        {
            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            workFlowResult.Setup(w => w.NewErrors).Returns(new List<string>());

            var testObject = new FailedLocalRateValidationResult(10, 9, mockDialog.Object, mockViewModel.Object);
            testObject.HandleValidationFailure(workFlowResult.Object);

            string expectedMessage =
                "9 of the 10 successfully processed UPS shipments had local rates that did not match the rates on your UPS account. Please review and update your local rates.";
            var expectedUri = new Uri("http://support.shipworks.com/support/solutions/articles/4000103270-ups-local-rating");

            mockViewModel.Verify(vm => vm.Load(expectedMessage, It.Is<Uri>(uri => uri == expectedUri)));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}