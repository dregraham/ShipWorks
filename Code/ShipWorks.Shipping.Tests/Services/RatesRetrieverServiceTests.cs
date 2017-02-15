using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class RatesRetrieverServiceTests : IDisposable
    {
        private readonly AutoMock mock;
        private readonly IMessenger messenger;
        private readonly Mock<IRateHashingService> rateHashingService;
        private readonly Mock<ISchedulerProvider> schedulerProvider;

        public RatesRetrieverServiceTests()
        {
            messenger = new TestMessenger();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            schedulerProvider = mock.WithMockImmediateScheduler();

            mock.Provide<IMessenger>(messenger);

            rateHashingService = mock.CreateMock<IRateHashingService>();

            mock.Override<IIndex<ShipmentTypeCode, IRateHashingService>>()
                .Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                .Returns(rateHashingService.Object);
        }

        [Fact]
        public void RatesRetrievingMessageIsNotSent_WhenThrottlePeriodHasNotElapsed()
        {
            TestScheduler testScheduler = new TestScheduler();
            schedulerProvider.Setup(sp => sp.Default).Returns(testScheduler);

            using (var testObject = mock.Create<RatesRetrieverService>())
            {
                testObject.InitializeForCurrentSession();

                messenger.OfType<RatesRetrievedMessage>().Subscribe(x => Assert.True(false, "Message should not have been sent"));

                messenger.Send(new ShipmentChangedMessage(this, mock.Create<ICarrierShipmentAdapter>()));
                testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(249).Ticks);
            }
        }

        [Fact]
        public void RatesRetrievingMessageIsSent_OnlyAfterThrottlePeriodHasElapsed()
        {
            TestScheduler testScheduler = new TestScheduler();
            schedulerProvider.Setup(sp => sp.Default).Returns(testScheduler);

            using (var testObject = mock.Create<RatesRetrieverService>())
            {
                testObject.InitializeForCurrentSession();

                RatesRetrievedMessage message = null;
                messenger.OfType<RatesRetrievedMessage>().Subscribe(x => message = x);

                messenger.Send(new ShipmentChangedMessage(this, mock.Create<ICarrierShipmentAdapter>()));
                testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(250).Ticks);

                Assert.NotNull(message);
            }
        }

        [Fact]
        public void SendsRatesRetrievingMessage_BeforeRatesAreRetrieved()
        {
            rateHashingService.Setup(x => x.GetRatingHash(It.IsAny<ShipmentEntity>())).Returns("Foo");

            var shipmentAdapter = mock.Create<ICarrierShipmentAdapter>();

            using (var testObject = mock.Create<RatesRetrieverService>())
            {
                testObject.InitializeForCurrentSession();

                RatesRetrievingMessage message = default(RatesRetrievingMessage);
                messenger.OfType<RatesRetrievingMessage>().Subscribe(x => message = x);

                messenger.Send(new ShipmentChangedMessage(this, shipmentAdapter));

                Assert.Equal(testObject, message.Sender);
                Assert.Equal("Foo", message.RatingHash);
            }
        }

        [Fact]
        public void SendsRatesRetrievedMessage_AfterRatesAreRetrieved()
        {
            rateHashingService.Setup(x => x.GetRatingHash(It.IsAny<ShipmentEntity>())).Returns("Foo");

            var results = GenericResult.FromError<RateGroup>("Bar");
            mock.Mock<IRatesRetriever>()
                .Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                .Returns(results);

            var shipmentAdapter = mock.Create<ICarrierShipmentAdapter>();

            using (var testObject = mock.Create<RatesRetrieverService>())
            {
                testObject.InitializeForCurrentSession();

                RatesRetrievedMessage message = null;
                messenger.OfType<RatesRetrievedMessage>().Subscribe(x => message = x);

                messenger.Send(new ShipmentChangedMessage(this, shipmentAdapter));

                Assert.Equal(testObject, message.Sender);
                Assert.Equal("Foo", message.RatingHash);
                Assert.Equal("Bar", message.ErrorMessage);
                Assert.Equal(shipmentAdapter.Shipment.ShipmentID, message.ShipmentAdapter.Shipment.ShipmentID);
            }
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
