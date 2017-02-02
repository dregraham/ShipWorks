using System;
using System.Linq;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ShippingDialogOpeningPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Subject<IShipWorksMessage> subject;

        public ShippingDialogOpeningPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            subject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void Register_CallsUnloadOrder_WhenShippingDialogOpeningMessageIsReceived()
        {
            var testObject = mock.Create<ShippingDialogOpeningPipeline>();

            testObject.Register(mock.Override<ShippingPanelViewModel>().Object);
            subject.OnNext(new ShippingDialogOpeningMessage(this));

            mock.Mock<ShippingPanelViewModel>().Verify(x => x.UnloadOrder());
        }

        [Fact]
        public void Register_ObservesOnWindowsEventLoopScheduler()
        {
            var testScheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(testScheduler);

            var testObject = mock.Create<ShippingDialogOpeningPipeline>();

            testObject.Register(mock.Override<ShippingPanelViewModel>().Object);
            subject.OnNext(new ShippingDialogOpeningMessage(this));

            mock.Mock<ShippingPanelViewModel>().Verify(x => x.UnloadOrder(), Times.Never);
            testScheduler.WindowsFormsEventLoop.Start();

            mock.Mock<ShippingPanelViewModel>().Verify(x => x.UnloadOrder());
        }

        [Fact]
        public void Dispose_DisposesSubscription()
        {
            mock.Override<IObservable<IShipWorksMessage>>()
                .Setup(x => x.Subscribe(It.IsAny<IObserver<IShipWorksMessage>>()))
                .Returns(mock.Mock<IDisposable>().Object);
            var testObject = mock.Create<ShippingDialogOpeningPipeline>();

            testObject.Register(mock.Override<ShippingPanelViewModel>().Object);
            testObject.Dispose();

            mock.Mock<IDisposable>().Verify(x => x.Dispose());
        }

        [Fact]
        public void Dispose_DoesNotThrow_IfRegisterIsNotCalledFirst()
        {
            var testObject = mock.Create<ShippingDialogOpeningPipeline>();

            testObject.Dispose();
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
