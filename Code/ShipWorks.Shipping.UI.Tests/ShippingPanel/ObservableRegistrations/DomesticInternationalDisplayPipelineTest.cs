using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class DomesticInternationalDisplayPipelineTest : IDisposable
    {
        readonly TestSchedulerProvider testScheduler;
        readonly AutoMock mock;

        public DomesticInternationalDisplayPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);
        }

        [Fact]
        public void Register_GetsDomesticStatusFromAdapter_Waits250Milliseconds()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<DomesticInternationalDisplayPipeline>();
            testObject.Register(viewModel.Object);

            viewModel.Object.Origin.CountryCode = "UK";

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(250).Ticks - 1);

            viewModel.Verify(x => x.UpdateServices(), Times.Never);
        }

        [Theory]
        [InlineData(false, "International")]
        [InlineData(true, "Domestic")]
        public void Register_SetsTextCorrectly_DependingOnIsDomestic(bool isDomestic, string expectedText)
        {
            var viewModel = mock.Create<ShippingPanelViewModel>();
            var testObject = mock.Create<DomesticInternationalDisplayPipeline>();
            testObject.Register(viewModel);

            viewModel.IsDomestic = isDomestic;

            Assert.Equal(expectedText, viewModel.DomesticInternationalText);
        }

        [Fact]
        public void Register_DoesNotChangeDomesticProperty_WhenShipmentAdapterChanges()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>(s =>
            {
                s.SetupSequence(x => x.ShipmentAdapter)
                    .Returns(mock.CreateMock<ICarrierShipmentAdapter>(null).Object)
                    .Returns(mock.CreateMock<ICarrierShipmentAdapter>(null).Object);
            });

            var testObject = mock.Create<DomesticInternationalDisplayPipeline>();
            testObject.Register(viewModel.Object);

            viewModel.Object.Origin.CountryCode = "UK";

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(250).Ticks);

            viewModel.Verify(x => x.UpdateServices(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
