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

            viewModel.Verify(x => x.IsDomestic, Times.Never);
        }

        [Fact]
        public void Register_GetsDomesticStatusFromAdapter_WhenOriginCountryChanges() =>
            VerifyGetsDomesticStatusFromAdapterForPropertyChange(x => x.Origin.CountryCode = "UK");

        [Fact]
        public void Register_GetsDomesticStatusFromAdapter_WhenOriginPostalCodeChanges() =>
            VerifyGetsDomesticStatusFromAdapterForPropertyChange(x => x.Origin.PostalCode = "12345");

        [Fact]
        public void Register_GetsDomesticStatusFromAdapter_WhenOriginStateProvCodeChanges() =>
            VerifyGetsDomesticStatusFromAdapterForPropertyChange(x => x.Origin.StateProvCode = "IL");

        [Fact]
        public void Register_GetsDomesticStatusFromAdapter_WhenDestinationCountryChanges() =>
            VerifyGetsDomesticStatusFromAdapterForPropertyChange(x => x.Destination.CountryCode = "UK");

        [Fact]
        public void Register_GetsDomesticStatusFromAdapter_WhenDestinationPostalCodeChanges() =>
            VerifyGetsDomesticStatusFromAdapterForPropertyChange(x => x.Destination.PostalCode = "12345");

        [Fact]
        public void Register_GetsDomesticStatusFromAdapter_WhenDestinationStateProvCodeChanges() =>
            VerifyGetsDomesticStatusFromAdapterForPropertyChange(x => x.Destination.StateProvCode = "IL");

        [Theory]
        [InlineData(false, "International")]
        [InlineData(true, "Domestic")]
        public void Register_SetsTextCorrectly_DependingOnIsDomestic(bool isDomestic, string text)
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>(s => s.Setup(x => x.IsDomestic).Returns(isDomestic)).Object;
            var testObject = mock.Create<DomesticInternationalDisplayPipeline>();
            testObject.Register(viewModel);

            viewModel.Origin.CountryCode = "UK";

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(250).Ticks);
            testScheduler.Dispatcher.Start();

            Assert.Equal(text, viewModel.DomesticInternationalText);
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

            viewModel.Verify(x => x.IsDomestic, Times.Never);
        }

        private void VerifyGetsDomesticStatusFromAdapterForPropertyChange(Action<ShippingPanelViewModel> changeProperty)
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>(s => s.Setup(x => x.IsDomestic).Verifiable()).Object;
            var testObject = mock.Create<DomesticInternationalDisplayPipeline>();
            testObject.Register(viewModel);

            changeProperty(viewModel);

            testScheduler.Default.AdvanceBy(TimeSpan.FromMilliseconds(250).Ticks);
            testScheduler.Dispatcher.Start();

            mock.VerifyAll = true;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
