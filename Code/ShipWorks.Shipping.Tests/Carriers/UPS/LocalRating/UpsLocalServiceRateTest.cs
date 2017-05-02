using System;
using System.Text;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalServiceRateTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly StringBuilder logEntry;

        public UpsLocalServiceRateTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            logEntry = new StringBuilder();
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
            testObject.Log(logEntry);
            Assert.Contains("Initial Value : $10.10", logEntry.ToString());
        }

        [Fact]
        public void Log_LogsService()
        {
            UpsLocalServiceRate testObject =
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);
            testObject.Log(logEntry);
            Assert.Contains("Rate Calculation for Ups3DaySelect", logEntry.ToString());
        }

        [Fact]
        public void Log_LogsAddedSurcharge()
        {
            UpsLocalServiceRate testObject =
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);

            testObject.AddAmount(.42M, "blah");
            testObject.Log(logEntry);

            Assert.Contains("\tAdded for 'blah' : $0.42", logEntry.ToString());
        }

        [Fact]
        public void Logs_Total()
        {
            UpsLocalServiceRate testObject =
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, 10.10M, false, 3);

            testObject.AddAmount(.42M, "blah");
            testObject.Log(logEntry);

            Assert.Contains("Total : $10.52", logEntry.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}