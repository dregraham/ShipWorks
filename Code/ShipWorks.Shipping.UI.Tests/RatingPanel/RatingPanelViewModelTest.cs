using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.RatingPanel
{
    public class RatingPanelViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        readonly TestMessenger messenger;
        readonly RateGroup testRateGroup;
        readonly List<RateResult> rates;

        public RatingPanelViewModelTest()
        {
            messenger = new TestMessenger();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            mock.Provide<IMessenger>(messenger);

            rates = Enumerable.Empty<RateResult>().ToList();
            testRateGroup = new RateGroup(rates);
        }

        [Fact]
        public void IsLoading_SetToTrue_WhenRatesRetrievingMessageIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));

            Assert.True(testObject.IsLoading);
        }

        [Fact]
        public void IsLoading_SetToFalse_WhenCorrespondingRatesRetrievedMessagesIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, testRateGroup, null));

            Assert.False(testObject.IsLoading);
        }

        [Fact]
        public void IsLoading_DoesNotSetIsLoadingToFalse_WhenNonCorrespondingRatesRetrievedMessagesIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, "Foo", testRateGroup, null));

            Assert.True(testObject.IsLoading);
        }

        [Fact]
        public void IsLoading_SetsIsLoadingToFalse_WhenNonCorrespondingRatesRetrievedMessagesIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, "Foo"));
            messenger.Send(new RatesRetrievingMessage(this, "Bar"));
            messenger.Send(new RatesRetrievedMessage(this, "Foo", testRateGroup, null));

            Assert.True(testObject.IsLoading);

            messenger.Send(new RatesRetrievedMessage(this, "Bar", testRateGroup, null));

            Assert.False(testObject.IsLoading);
        }

        [Fact]
        public void DoesNotLoadIntermediateRates_WhenTheSameHashComesBeforeAndAfterAnother()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, "Foo"));
            messenger.Send(new RatesRetrievingMessage(this, "Bar"));
            messenger.Send(new RatesRetrievingMessage(this, "Foo"));
            messenger.Send(new RatesRetrievedMessage(this, "Foo", testRateGroup, null));

            Assert.Empty(testObject.Rates);

            messenger.Send(new RatesRetrievedMessage(this, "Bar", new RateGroup(new List<RateResult> { new RateResult("x", "y") }), null));

            Assert.Empty(testObject.Rates);

            messenger.Send(new RatesRetrievedMessage(this, "Foo", new RateGroup(new List<RateResult> {
                new RateResult("x", "y"), new RateResult("x", "y") }), null));

            Assert.Equal(2, testObject.Rates.Count());
            ///Assert.Empty(testObject.Rates);
        }

        public void Dispose()
        {
            mock?.Dispose();
            messenger?.Dispose();
        }
    }
}
