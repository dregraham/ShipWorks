using System;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
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

        public RatesRetrieverServiceTests()
        {
            messenger = new TestMessenger();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            mock.Provide<IMessenger>(messenger);

            rateHashingService = mock.CreateMock<IRateHashingService>();

            mock.Override<IIndex<ShipmentTypeCode, IRateHashingService>>()
                .Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                .Returns(rateHashingService.Object);
        }

        [Fact]
        public void RatesRetrievingMessageIsNotSent_WhenThrottlePeriodHasNotElapsed()
        {
            var schedulerProvider = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(schedulerProvider);

            using (var testObject = mock.Create<RatesRetrieverService>())
            {
                testObject.InitializeForCurrentSession();

                messenger.OfType<RatesRetrievedMessage>().Subscribe(x => Assert.True(false, "Message should not have been sent"));

                messenger.Send(new ShipmentChangedMessage(this, mock.Create<ICarrierShipmentAdapter>()));
                schedulerProvider.Default.AdvanceBy(TimeSpan.FromMilliseconds(249).Ticks);
            }
        }

        [Fact]
        public void RatesRetrievingMessageIsSent_OnlyAfterThrottlePeriodHasElapsed()
        {
            var schedulerProvider = new TestSchedulerProvider();
            mock.Provide<ISchedulerProvider>(schedulerProvider);

            using (var testObject = mock.Create<RatesRetrieverService>())
            {
                testObject.InitializeForCurrentSession();

                RatesRetrievedMessage message = null;
                messenger.OfType<RatesRetrievedMessage>().Subscribe(x => message = x);

                messenger.Send(new ShipmentChangedMessage(this, mock.Create<ICarrierShipmentAdapter>()));
                schedulerProvider.Default.AdvanceBy(TimeSpan.FromMilliseconds(250).Ticks);
                schedulerProvider.TaskPool.Start();

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

                RatesRetrievingMessage message = null;
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

            var results = GenericResult.FromError<RateGroup>("Foo");
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
                Assert.Equal(results, message.Results);
                Assert.Equal(shipmentAdapter, message.ShipmentAdapter);
            }
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
