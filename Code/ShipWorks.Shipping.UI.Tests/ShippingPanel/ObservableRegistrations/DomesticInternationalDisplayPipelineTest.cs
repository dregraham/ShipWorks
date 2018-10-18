using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
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
            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupGet(s => s.Shipment).Returns(new ShipmentEntity());

            var viewModel = mock.Create<ShippingPanelViewModel>();

            LoadedOrderSelection LoadedOrderSelection = new LoadedOrderSelection(
                new OrderEntity(),
                new[] { shipmentAdapter.Object },
                new KeyValuePair<long, ShippingAddressEditStateType>[] { new KeyValuePair<long, ShippingAddressEditStateType>(1, ShippingAddressEditStateType.Editable) });

            viewModel.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] { LoadedOrderSelection }));
            viewModel.LoadShipment(shipmentAdapter.Object);

            // Force the property to be different from what we set it to a few lines down
            // this ensures that the test object gets notified of the property changing
            viewModel.IsDomestic = !isDomestic;

            var testObject = mock.Create<DomesticInternationalDisplayPipeline>();
            testObject.Register(viewModel);

            viewModel.IsDomestic = isDomestic;

            testScheduler.Dispatcher.Start();
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
