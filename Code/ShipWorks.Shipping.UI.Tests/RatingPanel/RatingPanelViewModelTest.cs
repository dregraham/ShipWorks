using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.RatingPanel
{
    public class RatingPanelViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        readonly TestMessenger messenger;
        readonly GenericResult<RateGroup> testResult;
        readonly RateGroup testRateGroup;
        readonly RateGroup successRateGroup = new RateGroup(new List<RateResult> { new RateResult("x", "y") });

        public RatingPanelViewModelTest()
        {
            messenger = new TestMessenger();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            mock.Provide<IMessenger>(messenger);

            testRateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            testResult = GenericResult.FromSuccess(testRateGroup);
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
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, testResult, null));

            Assert.False(testObject.IsLoading);
        }

        [Fact]
        public void IsLoading_DoesNotSetIsLoadingToFalse_WhenNonCorrespondingRatesRetrievedMessagesIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, "Foo", testResult, null));

            Assert.True(testObject.IsLoading);
        }

        [Fact]
        public void IsLoading_SetsIsLoadingToFalse_WhenNonCorrespondingRatesRetrievedMessagesIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, "Foo"));
            messenger.Send(new RatesRetrievingMessage(this, "Bar"));
            messenger.Send(new RatesRetrievedMessage(this, "Foo", testResult, null));

            Assert.True(testObject.IsLoading);

            messenger.Send(new RatesRetrievedMessage(this, "Bar", testResult, null));

            Assert.False(testObject.IsLoading);
        }

        [Fact]
        public void DoesNotLoadIntermediateRates_WhenTheSameHashComesBeforeAndAfterAnother()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, "Foo"));
            messenger.Send(new RatesRetrievingMessage(this, "Bar"));
            messenger.Send(new RatesRetrievingMessage(this, "Foo"));
            messenger.Send(new RatesRetrievedMessage(this, "Foo", testResult, null));

            Assert.Empty(testObject.Rates);

            messenger.Send(new RatesRetrievedMessage(this, "Bar",
                GenericResult.FromSuccess(new RateGroup(new List<RateResult> { new RateResult("x", "y") })), null));

            Assert.Empty(testObject.Rates);

            messenger.Send(new RatesRetrievedMessage(this, "Foo", testResult, null));

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
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromSuccess(rateGroup), null));

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
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromSuccess(rateGroup), null));

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
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromSuccess(rateGroup), null));

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
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromSuccess(rateGroup), null));

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
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromSuccess(rateGroup), null));

            Assert.True(testObject.ShowShipping);
        }

        [Fact]
        public void ClearsMessaging_WhenResultsAreSuccessfulWithRates()
        {
            testResult.Value.Rates.Add(new RateResult("Foo", "3"));

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, testResult, null));

            Assert.False(testObject.ShowEmptyMessage);
            Assert.True(string.IsNullOrEmpty(testObject.EmptyMessage));
        }

        [Fact]
        public void SetsMessaging_WhenResultsAreNotSuccessful()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromError<RateGroup>("Foo"), null));

            Assert.True(testObject.ShowEmptyMessage);
            Assert.Equal("Foo", testObject.EmptyMessage);
        }

        [Fact]
        public void ClearsRates_WhenResultsAreNotSuccessful()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromError<RateGroup>("Foo"), null));

            Assert.Empty(testObject.Rates);
        }

        [Fact]
        public void GetsFootnoteViewModels_WhenResultsHaveFootnotes()
        {
            var viewModel1 = mock.Create<IRateFootnoteFactory>();
            var footnote1 = mock.CreateMock<IRateFootnoteFactory>();
            footnote1.Setup(x => x.CreateViewModel(It.IsAny<ICarrierShipmentAdapter>())).Returns(viewModel1);
            testRateGroup.AddFootnoteFactory(footnote1.Object);

            var viewModel2 = mock.Create<IRateFootnoteFactory>();
            var footnote2 = mock.CreateMock<IRateFootnoteFactory>();
            footnote2.Setup(x => x.CreateViewModel(It.IsAny<ICarrierShipmentAdapter>())).Returns(viewModel2);
            testRateGroup.AddFootnoteFactory(footnote2.Object);

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, testResult, null));

            Assert.Contains(viewModel1, testObject.Footnotes);
            Assert.Contains(viewModel2, testObject.Footnotes);
        }

        [Fact]
        public void SetsSelectedRate_WhenRatesAreRetrieved()
        {
            var testRateResult = new RateResult("Bar", "2") { Selectable = true };
            var carrierAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            carrierAdapter.Setup(x => x.DoesRateMatchSelectedService(testRateResult)).Returns(true);

            var rateGroup = new RateGroup(new[]
            {
                new RateResult("Foo", "1") { Selectable = true },
                testRateResult,
                new RateResult("Baz", "3") { Selectable = true }
            });

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty,
                GenericResult.FromSuccess(rateGroup), carrierAdapter.Object));

            Assert.Equal(3, testObject.Rates.Count());
            Assert.Equal("Bar", testObject.SelectedRate.Description);
        }
        
        [Fact]
        public void ShowEmptyMessage_SetToTrue_WhenRatesNotSupportedMessageIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesNotSupportedMessage(this, string.Empty));

            Assert.True(testObject.ShowEmptyMessage);
        }

        [Fact]
        public void EmptyMessage_Set_WhenRatesNotSupportedMessageIsReceived()
        {
            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesNotSupportedMessage(this, "Bad rates!"));

            Assert.Equal("Bad rates!", testObject.EmptyMessage);
        }

        [Fact]
        public void EmptyMessage_Set_WhenRatesRetrievedMessageContainsMultipleShipments()
        {
            ShipmentEntity shipment = new ShipmentEntity(3);
            OrderEntity order = new OrderEntity(6);
            order.Shipments.Add(shipment);
            order.Shipments.Add(new ShipmentEntity(4));
            shipment.Order = order;

            Mock<ICarrierShipmentAdapter> shipmentAdapter = new Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromSuccess(successRateGroup), shipmentAdapter.Object));

            Assert.True(testObject.ShowEmptyMessage);
            Assert.NotNull(testObject.EmptyMessage);
            Assert.False(string.IsNullOrWhiteSpace(testObject.EmptyMessage));
        }

        [Fact]
        public void EmptyMessage_NotSet_WhenRatesRetrievedMessageContainsSingleShipment()
        {
            ShipmentEntity shipment = new ShipmentEntity(3);
            OrderEntity order = new OrderEntity(6);
            order.Shipments.Add(shipment);
            shipment.Order = order;

            Mock<ICarrierShipmentAdapter> shipmentAdapter = new Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            var testObject = mock.Create<RatingPanelViewModel>();
            messenger.Send(new RatesRetrievingMessage(this, string.Empty));
            messenger.Send(new RatesRetrievedMessage(this, string.Empty, GenericResult.FromSuccess(successRateGroup), shipmentAdapter.Object));

            Assert.False(testObject.ShowEmptyMessage);
            Assert.True(string.IsNullOrWhiteSpace(testObject.EmptyMessage));
        }
        
        public void Dispose()
        {
            mock?.Dispose();
            messenger?.Dispose();
        }
    }
}
