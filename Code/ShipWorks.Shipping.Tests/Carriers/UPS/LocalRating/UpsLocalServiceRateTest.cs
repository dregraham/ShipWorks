using System;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalServiceRateTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ILog> log;

        public UpsLocalServiceRateTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            log = mock.CreateMock<ILog>();
        }

        [Fact]
        public void AddAmount_AmountIsAddedToBaseAmount()
        {
            UpsLocalServiceRate testObject = new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);
            testObject.AddAmount(.42M, "blah");

            Assert.Equal(10.52M, testObject.Amount);
        }

        [Fact]
        public void Log_LogsBaseAmount()
        {
            UpsLocalServiceRate testObject = 
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);
            testObject.Log(log.Object);
            log.Verify(l => l.Info("Initial Value : $10.10"));
        }

        [Fact]
        public void Log_LogsService()
        {
            UpsLocalServiceRate testObject =
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);
            testObject.Log(log.Object);
            log.Verify(l => l.Info("Rate Calculation for Ups3DaySelect"));
        }

        [Fact]
        public void Log_LogsAddedSurcharge()
        {
            UpsLocalServiceRate testObject =
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);

            testObject.AddAmount(.42M, "blah");
            testObject.Log(log.Object);

            log.Verify(l => l.Info("\tAdded for 'blah' : $0.42"));
        }

        [Fact]
        public void Logs_Total()
        {
            UpsLocalServiceRate testObject =
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);

            testObject.AddAmount(.42M, "blah");
            testObject.Log(log.Object);

            log.Verify(l => l.Info("Total : $10.52"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}