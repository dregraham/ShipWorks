using System;
using System.Linq;
using System.Reactive.Subjects;
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
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> subject;

        public RatesRetrievedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            subject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void Register_CallsShowSpinner_WhenRatesRetrievingMessageReceived()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner());
        }

        [Fact]
        public void Register_CallsShowSpinnerOnce_WhenMultipleOrderSelectionChangingMessageReceived()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Once);
        }

        [Fact]
        public void Register_CallsShowSpinnerTwice_AfterShippingDialogWasOpenedAndClosed()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.OnNext(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.OnNext(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));

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

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));

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

            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Never);
        }

        [Fact]
        public void Register_ShowSpinnerIsNotCalled_WhenWindowIsClosed()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));

            viewModel.Verify(x => x.ShowSpinner(), Times.Never);
        }

        [Fact]
        public void Register_LoadRatesIsCalled_WhenRatesRetrievedMatchesRetrieving()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);
            var retrievedMessage = CreateRetrievedMessageWithHash("Foo");

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(retrievedMessage);

            viewModel.Verify(x => x.LoadRates(retrievedMessage));
        }

        [Fact]
        public void Register_LoadRatesIsCalledOnce_WhenMultipleRetrievingMessagesAreReceived()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(new RatesRetrievingMessage(this, "Bar"));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(CreateRetrievedMessageWithHash("Foo"));

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

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(new RatesRetrievingMessage(this, "Bar"));
            subject.OnNext(notCalledMessage);
            subject.OnNext(message);

            viewModel.Verify(x => x.LoadRates(notCalledMessage), Times.Never);
            viewModel.Verify(x => x.LoadRates(message), Times.Once);
        }

        [Fact]
        public void Register_LoadRatesIsNotCalled_WhenWindowIsNotOpen()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(CreateRetrievedMessageWithHash("Foo"));

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()), Times.Never);
        }

        [Fact]
        public void Register_LoadRatesIsNotCalled_WhenWindowIsClosed()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<RatesRetrievedPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new OpenShippingDialogMessage(this, Enumerable.Empty<ShipmentEntity>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(CreateRetrievedMessageWithHash("Foo"));

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

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Foo"));
            subject.OnNext(CreateRetrievedMessageWithHash("Foo"));

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

            subject.OnNext(new OrderSelectionChangingMessage(this, Enumerable.Empty<long>()));
            subject.OnNext(new RatesRetrievingMessage(this, "Bar"));
            subject.OnNext(CreateRetrievedMessageWithHash("Foo"));

            viewModel.Verify(x => x.LoadRates(It.IsAny<RatesRetrievedMessage>()), Times.Never);
        }

        private RatesRetrievedMessage CreateRetrievedMessageWithHash(string hash)
        {
            return new RatesRetrievedMessage(this, hash,
                GenericResult.FromSuccess(new RateGroup(Enumerable.Empty<RateResult>())),
                mock.Create<ICarrierShipmentAdapter>());
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
