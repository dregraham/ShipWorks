using System;
using System.Text;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Validation
{
    public class UpsLocalRateDiscrepancyTest : IDisposable
    {
        public UpsLocalRateDiscrepancyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        private readonly AutoMock mock;

        [Fact]
        public void GetLogMessage_ReturnsCorrectLogMessage_WhenLocalRateAndApiRateNotNull()
        {
            var shipment = new ShipmentEntity(42) {ShipmentCost = 10.10m};
            var mockedLocalRate = mock.Mock<IUpsLocalServiceRate>();
            mockedLocalRate.SetupGet(r => r.Amount).Returns(15.44m);
            mockedLocalRate.Setup(r => r.Log(It.IsAny<StringBuilder>()))
                .Callback<StringBuilder>(builder => builder.Append("Hello World"));

            var apiRate = new UpsServiceRate(UpsServiceType.Ups2DayAir, 17.33m, false, 10);

            var testObject = new UpsLocalRateDiscrepancy(shipment, mockedLocalRate.Object, apiRate);

            var expectedLogMessage = new StringBuilder();
            expectedLogMessage.AppendLine("Shipment ID: 42");
            expectedLogMessage.AppendLine("Local Rate: $15.44");
            expectedLogMessage.AppendLine("API Rate: $17.33");
            expectedLogMessage.AppendLine("Label Cost: $10.10");
            expectedLogMessage.Append("Hello World");

            Assert.Equal(expectedLogMessage.ToString(), testObject.GetLogMessage());
        }

        [Fact]
        public void GetLogMessage_ReturnsCorrectLogMessage_WhenLocalRateNotNull_AndApiRateNull()
        {
            var shipment = new ShipmentEntity(42) {ShipmentCost = 10.10m};
            var mockedLocalRate = mock.Mock<IUpsLocalServiceRate>();
            mockedLocalRate.SetupGet(r => r.Amount).Returns(15.44m);
            mockedLocalRate.Setup(r => r.Log(It.IsAny<StringBuilder>()))
                .Callback<StringBuilder>(builder => builder.Append("Hello World"));

            var testObject = new UpsLocalRateDiscrepancy(shipment, mockedLocalRate.Object, null);

            var expectedLogMessage = new StringBuilder();
            expectedLogMessage.AppendLine("Shipment ID: 42");
            expectedLogMessage.AppendLine("Local Rate: $15.44");
            expectedLogMessage.AppendLine("API Rate: Not found");
            expectedLogMessage.AppendLine("Label Cost: $10.10");
            expectedLogMessage.Append("Hello World");

            Assert.Equal(expectedLogMessage.ToString(), testObject.GetLogMessage());
        }

        [Fact]
        public void GetLogMessage_ReturnsCorrectLogMessage_WhenLocalRateNotNull_AndConstructorWithoutApiRateUsed()
        {
            var shipment = new ShipmentEntity(42) {ShipmentCost = 10.10m};
            var mockedLocalRate = mock.Mock<IUpsLocalServiceRate>();
            mockedLocalRate.SetupGet(r => r.Amount).Returns(15.44m);
            mockedLocalRate.Setup(r => r.Log(It.IsAny<StringBuilder>()))
                .Callback<StringBuilder>(builder => builder.Append("Hello World"));

            var testObject = new UpsLocalRateDiscrepancy(shipment, mockedLocalRate.Object);

            var expectedLogMessage = new StringBuilder();
            expectedLogMessage.AppendLine("Shipment ID: 42");
            expectedLogMessage.AppendLine("Local Rate: $15.44");
            expectedLogMessage.AppendLine("API Rate: Not found");
            expectedLogMessage.AppendLine("Label Cost: $10.10");
            expectedLogMessage.Append("Hello World");

            Assert.Equal(expectedLogMessage.ToString(), testObject.GetLogMessage());
        }

        [Fact]
        public void GetLogMessage_ReturnsCorrectLogMessage_WhenLocalRateNull_AndApiRateNotNull()
        {
            var shipment = new ShipmentEntity(42) {ShipmentCost = 10.10m};
            var apiRate = new UpsServiceRate(UpsServiceType.Ups2DayAir, 17.33m, false, 10);

            var testObject = new UpsLocalRateDiscrepancy(shipment, null, apiRate);

            var expectedLogMessage = new StringBuilder();
            expectedLogMessage.AppendLine("Shipment ID: 42");
            expectedLogMessage.AppendLine("Local Rate: Not found");
            expectedLogMessage.AppendLine("API Rate: $17.33");
            expectedLogMessage.AppendLine("Label Cost: $10.10");

            Assert.Equal(expectedLogMessage.ToString(), testObject.GetLogMessage());
        }

        [Fact]
        public void GetUserMessage_ReturnsCorrectMessage_WhenLocalRateAndApiRateNotNull()
        {
            var shipment = new ShipmentEntity(42)
            {
                ShipmentCost = 10.10m,
                Ups = new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.Ups3DaySelect
                }
            };

            var mockedLocalRate = mock.Mock<IUpsLocalServiceRate>();
            mockedLocalRate.SetupGet(r => r.Amount).Returns(15.44m);
            mockedLocalRate.Setup(r => r.Log(It.IsAny<StringBuilder>()))
                .Callback<StringBuilder>(builder => builder.Append("Hello World"));

            var apiRate = new UpsServiceRate(UpsServiceType.Ups2DayAir, 17.33m, false, 10);

            var testObject = new UpsLocalRateDiscrepancy(shipment, mockedLocalRate.Object, apiRate);

            var expectedLogMessage = "There was a discrepancy between the local rate ($15.44) " +
                                        "and the API rate ($17.33) using UPS 3 Day Select®."
                                        + Environment.NewLine + "Hello World";

            Assert.Equal(expectedLogMessage, testObject.GetUserMessage());
        }

        [Fact]
        public void GetUserMessage_ReturnsCorrectMessage_WhenLocalRateIsNull_AndApiRateNotNull()
        {
            var shipment = new ShipmentEntity(42)
            {
                ShipmentCost = 10.10m,
                Ups = new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.Ups3DaySelect
                }
            };

            var apiRate = new UpsServiceRate(UpsServiceType.Ups2DayAir, 17.33m, false, 10);

            var testObject = new UpsLocalRateDiscrepancy(shipment, null, apiRate);

            var expectedLogMessage = "There was a discrepancy between the local rate (not found) " +
                                        "and the API rate ($17.33) using UPS 3 Day Select®." + Environment.NewLine;

            Assert.Equal(expectedLogMessage, testObject.GetUserMessage());
        }

        [Fact]
        public void GetUserMessage_ReturnsCorrectMessage_WhenLocalRateNotNull_AndApiRateNull()
        {
            var shipment = new ShipmentEntity(42)
            {
                ShipmentCost = 10.10m,
                Ups = new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.Ups3DaySelect
                }
            };

            var mockedLocalRate = mock.Mock<IUpsLocalServiceRate>();
            mockedLocalRate.SetupGet(r => r.Amount).Returns(15.44m);
            mockedLocalRate.Setup(r => r.Log(It.IsAny<StringBuilder>()))
                .Callback<StringBuilder>(builder => builder.Append("Hello World"));

            var testObject = new UpsLocalRateDiscrepancy(shipment, mockedLocalRate.Object, null);

            var expectedLogMessage = "There was a discrepancy between the local rate ($15.44) " +
                                        "and the API rate (not found) using UPS 3 Day Select®."
                                        + Environment.NewLine + "Hello World";

            Assert.Equal(expectedLogMessage, testObject.GetUserMessage());
        }

        [Fact]
        public void GetUserMessage_ReturnsCorrectMessage_WhenLocalRateNotNull_AndConstructorWithoutApiRateUsed()
        {
            var shipment = new ShipmentEntity(42)
            {
                ShipmentCost = 10.10m,
                Ups = new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.Ups3DaySelect
                }
            };

            var mockedLocalRate = mock.Mock<IUpsLocalServiceRate>();
            mockedLocalRate.SetupGet(r => r.Amount).Returns(15.44m);
            mockedLocalRate.Setup(r => r.Log(It.IsAny<StringBuilder>()))
                .Callback<StringBuilder>(builder => builder.Append("Hello World"));

            var testObject = new UpsLocalRateDiscrepancy(shipment, mockedLocalRate.Object);

            var expectedLogMessage = "There was a discrepancy between the local rate ($15.44) " +
                                        "and the API rate (not found) using UPS 3 Day Select®."
                                        + Environment.NewLine + "Hello World";

            Assert.Equal(expectedLogMessage, testObject.GetUserMessage());
        }
    }
}