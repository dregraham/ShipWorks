using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.RatingPanel.ObservableRegistrations
{
    public class RatesRetrievedPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger subject;

        public RatesRetrievedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            subject = new TestMessenger();
            mock.Provide<IMessenger>(subject);
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void Register_CallsShowSpinner_WhenRatesRetrievingMessageReceived()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner());
        }

        [Fact]
        public void Register_CallsShowSpinnerOnce_WhenMultipleOrderSelectionChangingMessageReceived()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Once);
        }

        [Fact]
        public void Register_CallsShowSpinnerTwice_AfterShippingDialogWasOpenedAndClosed()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.Send(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.Send(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Exactly(2));
        }

        [Fact]
        public void Register_ObservesOnDispatcher_WhenRatesRetrievingMessageReceived()
        {
            var scheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(scheduler);
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Never);

            scheduler.Dispatcher.Start();

            viewModel.Verify(x => x.ShowSpinner());
        }

        [Fact]
        public void Register_ShowSpinnerIsNotCalled_WhenWindowIsNotOpen()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Never);
        }

        [Fact]
        public void Register_ShowSpinnerIsNotCalled_WhenWindowIsClosed()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Never);
        }

        [Fact]
        public void Register_LoadRatesIsCalled_WhenRatesRetrievedMatchesRetrieving()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);
            var retrievedMessage = CreateRetrievedMessageWithHash("Foo");

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(retrievedMessage);

            viewModel.Verify(x => x.LoadRates(retrievedMessage));
        }

        [Fact]
        public void Register_LoadRatesIsCalledOnce_WhenMultipleRetrievingMessagesAreReceived()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(new RatesRetrievingMessage(this, "Bar"));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(CreateRetrievedMessageWithHash("Foo"));

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()), Times.Once);
        }

        [Fact]
        public void Register_LoadRatesIsCalledOnce_WhenMessagesAreInterleved()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);
            var notCalledMessage = CreateRetrievedMessageWithHash("Foo");
            var message = CreateRetrievedMessageWithHash("Bar");

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(new RatesRetrievingMessage(this, "Bar"));
            subject.Send(notCalledMessage);
            subject.Send(message);

            viewModel.Verify(x => x.LoadRates(notCalledMessage), Times.Never);
            viewModel.Verify(x => x.LoadRates(message), Times.Once);
        }

        [Fact]
        public void Register_LoadRatesIsNotCalled_WhenWindowIsNotOpen()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(CreateRetrievedMessageWithHash("Foo"));

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()), Times.Never);
        }

        [Fact]
        public void Register_LoadRatesIsNotCalled_WhenWindowIsClosed()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(CreateRetrievedMessageWithHash("Foo"));

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()), Times.Never);
        }

        [Fact]
        public void Register_LoadRatesIsCalled_AfterThrottleTime()
        {
            var scheduler = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(scheduler);

            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Foo"));
            subject.Send(CreateRetrievedMessageWithHash("Foo"));

            scheduler.Default.AdvanceBy(249);
            scheduler.Dispatcher.Start();

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()), Times.Never);

            scheduler.Default.Start();
            scheduler.Dispatcher.Start();

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()));
        }

        [Fact]
        public void Register_LoadRatesIsNotCalled_WhenRatesRetrievedDoesNotMatchRetrieving()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.Send(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.Send(new RatesRetrievingMessage(this, "Bar"));
            subject.Send(CreateRetrievedMessageWithHash("Foo"));

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()), Times.Never);
        }

        private RatesRetrievedMessage CreateRetrievedMessageWithHash(string hash)
        {
            return new RatesRetrievedMessage(this, hash,
                GenericResult.FromSuccess(new RateGroup(Enumerable.Empty<RateResult>())),
                mock.Build<ICarrierShipmentAdapter>());
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
