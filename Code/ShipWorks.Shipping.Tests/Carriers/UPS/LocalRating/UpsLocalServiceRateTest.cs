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
        public void AddSurcharge_AmountIsAddedToBaseAmount()
        {
            UpsLocalServiceRate testObject = new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, "4", 10.10M, "3");
            testObject.AddAmount(.42M, "blah");

            Assert.Equal(10.52M, testObject.Amount);
        }

        [Theory]
        [InlineData("Service Rate : $10.10")]
        [InlineData("Total : $10.52")]
        [InlineData("\tblah : $0.42")]
        [InlineData("Rate Calculation for Ups3DaySelect")]
        public void Log_LogSinglePackage(string textToFind)
        {
            UpsLocalServiceRate testObject = new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, "4", 10.10M, "3");

            testObject.AddAmount(.42M, "blah");
            testObject.Log(logEntry);

            Assert.Contains(textToFind, logEntry.ToString());
        }

        [Fact]
        public void Merge_AmountIsAddedToAmount()
        {
            UpsLocalServiceRate testObject = new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, "4", 10.10M, "3");
            UpsLocalServiceRate packageToAdd = new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, "4", 20.10M, "77");

            testObject.AddAmount(packageToAdd);

            Assert.Equal(30.20M, testObject.Amount);
        }

        [Theory]
        [InlineData("Package 1:")]
        [InlineData("Package 2:")]
        [InlineData("Billable Weight : 77")]
        [InlineData("Service Rate : $20.10")]

        public void Log_LogsSecondPackage(string textToFind)
        {
            UpsLocalServiceRate testObject = new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, "4", 10.10M, "3");
            UpsLocalServiceRate packageToAdd = new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, "4", 20.10M, "77");

            testObject.AddAmount(packageToAdd);
            testObject.Log(logEntry);

            Assert.Contains(textToFind, logEntry.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}