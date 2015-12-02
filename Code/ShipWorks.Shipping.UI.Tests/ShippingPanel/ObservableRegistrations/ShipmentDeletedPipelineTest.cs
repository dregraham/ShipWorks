using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ShipmentDeletedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> testSubject;

        public ShipmentDeletedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testSubject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(testSubject);
        }

        [Fact]
        public void Register_CallsUnloadShipment_WhenCurrentShipmentHasDeletedId()
        {
            var viewModel = mock.CreateShippingPanelViewModel(v => v.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = 123 }));

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new ShipmentDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment());
        }

        [Fact]
        public void Register_DoesNotCallUnloadShipment_WhenCurrentShipmentDoesNotHaveDeletedId()
        {
            var viewModel = mock.CreateShippingPanelViewModel();

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new ShipmentDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
            testSubject.Dispose();
        }
    }
}
