using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Validation
{
    public class LocalRateValidationResultFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IDialog> dialog;
        private readonly Mock<IIndex<string, IDialog>> dialogIndex;

        public LocalRateValidationResultFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            dialog = mock.Mock<IDialog>();
            dialogIndex = mock.CreateMock<IIndex<string, IDialog>>();
            dialogIndex.Setup(x => x["UpsLocalRateDiscrepancyDialog"])
                .Returns(dialog.Object);
            mock.Provide(dialogIndex.Object);
        }

        [Fact]
        public void Create_GetsPassedLocalRateValidationResult_WhenNoDiscrepancies()
        {
            var testObject = mock.Create<LocalRateValidationResultFactory>();
            var result = testObject.Create(1, 0, () => { });
            Assert.IsType<SuccessfulLocalRateValidationResult>(result);
        }

        [Fact]
        public void Create_GetsFailedLocalRateValidationResult_WhenDiscrepancies()
        {
            var testObject = mock.Create<LocalRateValidationResultFactory>();
            var result = testObject.Create(1, 2, () => { });
            Assert.IsType<FailedLocalRateValidationResult>(result);
        }

        [Fact]
        public void Create_GetsUpsLocalRateDiscrepancyDialogFromIIndex()
        {
            var testObject = mock.Create<LocalRateValidationResultFactory>();
            testObject.Create(1, 1, () => { });
            
           dialogIndex.Verify(i=>i["UpsLocalRateDiscrepancyDialog"], Times.Once);
        }

        [Fact]
        public void Create_SetsSnoozeMethod()
        {
            var testObject = mock.Create<LocalRateValidationResultFactory>();
            Action snooze = () => { };
            testObject.Create(1, 1, snooze);
            mock.Mock<IUpsLocalRateDiscrepancyViewModel>()
                .VerifySet(m=>m.Snooze = snooze, Times.Once);
        }

        [Fact]
        public void Create_SetsCloseMethod()
        {
            var testObject = mock.Create<LocalRateValidationResultFactory>();
            Action snooze = () => { };
            testObject.Create(1, 1, snooze);
            mock.Mock<IUpsLocalRateDiscrepancyViewModel>()
                .VerifySet(m => m.Close = dialog.Object.Close, Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}