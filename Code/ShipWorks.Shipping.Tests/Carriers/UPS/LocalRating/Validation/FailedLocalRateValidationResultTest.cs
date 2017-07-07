using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Carriers.UPS.Enums;
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
            var discrepancies = new List<UpsLocalRateDiscrepancy>
            {
                new UpsLocalRateDiscrepancy(new ShipmentEntity(),
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1"))
            };

            var testObject = new FailedLocalRateValidationResult(discrepancies, Enumerable.Empty<ShipmentEntity>(), mockDialog.Object, mockViewModel.Object);

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
            ShipmentEntity[] shipments = Enumerable.Repeat(new ShipmentEntity(), 10).ToArray();
            
            var discrepancies = new List<UpsLocalRateDiscrepancy>
            {
                new UpsLocalRateDiscrepancy(shipments[0],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[1],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[2],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[3],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[4],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[5],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[6],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[7],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[8],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
            };

            var testObject = new FailedLocalRateValidationResult(discrepancies, shipments, mockDialog.Object, mockViewModel.Object);

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
            ShipmentEntity[] shipments = Enumerable.Repeat(new ShipmentEntity(), 10).ToArray();

            var discrepancies = new List<UpsLocalRateDiscrepancy>
            {
                new UpsLocalRateDiscrepancy(shipments[0],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[1],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[2],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[3],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[4],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[5],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[6],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[7],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[8],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
            };

            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            workFlowResult.Setup(w => w.NewErrors).Returns(new List<string>());

            var testObject = new FailedLocalRateValidationResult(discrepancies, shipments, mockDialog.Object, mockViewModel.Object);
            testObject.HandleValidationFailure(workFlowResult.Object);

            mockDialog.Verify(d=>d.ShowDialog(), Times.Once);
        }

        [Fact]
        public void HandleValidationFailure_SetsDialogDataContext_AndNoOtherErrors()
        {
            ShipmentEntity[] shipments = Enumerable.Repeat(new ShipmentEntity(), 10).ToArray();

            var discrepancies = new List<UpsLocalRateDiscrepancy>
            {
                new UpsLocalRateDiscrepancy(shipments[0],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[1],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[2],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[3],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[4],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[5],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[6],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[7],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[8],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
            };

            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            workFlowResult.Setup(w => w.NewErrors).Returns(new List<string>());

            var testObject = new FailedLocalRateValidationResult(discrepancies, shipments, mockDialog.Object, mockViewModel.Object);
            testObject.HandleValidationFailure(workFlowResult.Object);

            mockDialog.VerifySet(d => d.DataContext = mockViewModel.Object, Times.Once());
        }

        [Fact]
        public void HandleValidationFailure_LoadsViewModel()
        {
            ShipmentEntity[] shipments = Enumerable.Repeat(new ShipmentEntity(), 10).ToArray();

            var discrepancies = new List<UpsLocalRateDiscrepancy>
            {
                new UpsLocalRateDiscrepancy(shipments[0],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[1],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[2],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[3],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[4],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[5],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[6],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[7],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
                new UpsLocalRateDiscrepancy(shipments[8],
                    new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1")),
            };

            Mock<IProcessShipmentsWorkflowResult> workFlowResult = mock.CreateMock<IProcessShipmentsWorkflowResult>();
            workFlowResult.Setup(w => w.NewErrors).Returns(new List<string>());

            var testObject = new FailedLocalRateValidationResult(discrepancies, shipments, mockDialog.Object, mockViewModel.Object);
            testObject.HandleValidationFailure(workFlowResult.Object);

            string expectedMessage =
                "9 of the 10 successfully processed UPS shipments had local rates that did not match the rates on your UPS account. Please review and update your local rates.";
            var expectedUri = new Uri("http://support.shipworks.com/support/solutions/articles/4000103804-ups-local-rating-troubleshooting-guide");

            mockViewModel.Verify(vm => vm.Load(expectedMessage, It.Is<Uri>(uri => uri == expectedUri)));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}