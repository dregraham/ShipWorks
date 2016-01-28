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

        public RatingPanelViewModelTest()
        {
            messenger = new TestMessenger();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            mock.Provide<IMessenger>(messenger);

            testRateGroup = new RateGroup(Enumerable.Empty<RateResult>());
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

            messenger.Send(new RatesRetrievedMessage(this, "Foo", testRateGroup, null));

            Assert.Empty(testObject.Rates);
        }

        [Fact]
        public void LoadsRates_WhenRatesRetrievedMessageIsReceived()
        {
            var rateGroup = new RateGroup(new[]
            {
                new RateResult("Foo", "1"),
                new RateResult("Bar", "2"),
                new RateResult("Baz", "3")
            });

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, rateGroup, null));

            Assert.Equal(3, testObject.Rates.Count());
            Assert.Contains("Foo", testObject.Rates.Select(x => x.Description));
            Assert.Contains("Bar", testObject.Rates.Select(x => x.Description));
            Assert.Contains("Baz", testObject.Rates.Select(x => x.Description));
        }

        [Fact]
        public void DoesNotShowExtraAmountFields_WhenNoRatesHaveAnyExtraAmounts()
        {
            var rateGroup = new RateGroup(new[]
            {
                new RateResult("Foo", "1") { Selectable = true },
                new RateResult("Bar", "2") { Selectable = true }
            });

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, rateGroup, null));

            Assert.False(testObject.ShowDuties);
            Assert.False(testObject.ShowTaxes);
            Assert.False(testObject.ShowShipping);
        }

        [Fact]
        public void ShowDutiesIsTrue_WhenOneRateHasDuties_AndRateIsSelectable()
        {
            var rateGroup = new RateGroup(new[]
            {
                new RateResult("Foo", "1"),
                new RateResult("Bar", "2", 1.23M, new RateAmountComponents(0, 3, 0), null) { Selectable = true }
            });

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, rateGroup, null));

            Assert.True(testObject.ShowDuties);
        }

        [Fact]
        public void ShowTaxesIsTrue_WhenOneRateHasTaxes_AndRateIsSelectable()
        {
            var rateGroup = new RateGroup(new[]
            {
                new RateResult("Foo", "1"),
                new RateResult("Bar", "2", 1.23M, new RateAmountComponents(3, 0, 0), null) { Selectable = true }
            });

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, rateGroup, null));

            Assert.True(testObject.ShowTaxes);
        }

        [Fact]
        public void ShowShippingIsTrue_WhenOneRateHasShipping_AndRateIsSelectable()
        {
            var rateGroup = new RateGroup(new[]
            {
                new RateResult("Foo", "1"),
                new RateResult("Bar", "2", 1.23M, new RateAmountComponents(0, 0, 3), null) { Selectable = true }
            });

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, rateGroup, null));

            Assert.True(testObject.ShowShipping);
        }

        public void Dispose()
        {
            mock?.Dispose();
            messenger?.Dispose();
        }
    }
}
