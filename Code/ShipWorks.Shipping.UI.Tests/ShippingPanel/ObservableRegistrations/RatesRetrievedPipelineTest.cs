using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class RatesRetrievedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> messenger;
        readonly Mock<IShipmentViewModel> shipmentViewModel;
        readonly ShippingPanelViewModel shippingPanel;

        public RatesRetrievedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            messenger = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(messenger);
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            shipmentViewModel = mock.Mock<IShipmentViewModel>();
            shippingPanel = mock.Create<ShippingPanelViewModel>();
        }

        [Fact]
        public void Register_DelegatesToViewModelUpdateServices_WhenViewModelHasShipmentTypeOfAmazon()
        {
            var testObject = mock.Create<RatesRetrievedPipeline>();
                                  
            testObject.Register(shippingPanel);
            shippingPanel.ShipmentType = ShipmentTypeCode.Amazon;
            shippingPanel.ShipmentViewModel = shipmentViewModel.Object;

            messenger.OnNext(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromError<RateGroup>("blah"), null));

            shipmentViewModel.Verify(s => s.RefreshServiceTypes(), Times.Once());
        }

        [Fact]
        public void Register_DoesNotCallUpdateServices_WhenViewModelHasShipmentTypeNotAmazon()
        {
            var testObject = mock.Create<RatesRetrievedPipeline>();

            testObject.Register(shippingPanel);
            shippingPanel.ShipmentType = ShipmentTypeCode.Usps;
            shippingPanel.ShipmentViewModel = shipmentViewModel.Object;

            messenger.OnNext(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromError<RateGroup>("blah"), null));

            shipmentViewModel.Verify(s => s.RefreshServiceTypes(), Times.Never());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
