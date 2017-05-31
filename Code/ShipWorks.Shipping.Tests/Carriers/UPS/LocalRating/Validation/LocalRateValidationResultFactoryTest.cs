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

        public LocalRateValidationResultFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Create_GetsUpsLocalRateDiscrepancyDialogFromIIndex()
        {
            var dialog = mock.Mock<IDialog>();
            var dialogIndex = mock.CreateMock<IIndex<string, IDialog>>();
            dialogIndex.Setup(x => x["UpsLocalRateDiscrepancyDialog"])
                .Returns(dialog.Object);
            mock.Provide(dialogIndex.Object);
            
            var testObject = mock.Create<LocalRateValidationResultFactory>();
            testObject.Create(1, 1);
            
           dialogIndex.Verify(i=>i["UpsLocalRateDiscrepancyDialog"], Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}