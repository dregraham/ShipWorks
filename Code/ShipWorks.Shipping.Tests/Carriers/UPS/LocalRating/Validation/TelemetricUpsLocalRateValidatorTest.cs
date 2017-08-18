using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Validation
{
    public class TelemetricUpsLocalRateValidatorTest : IDisposable
    {
        private readonly AutoMock mock;

        public TelemetricUpsLocalRateValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ValidateShipments_AddsNotFoundForLocalRateTelemetry_WhenDiscrepancyLocalRateIsNull()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.UpsGround
                }
            };

            Mock<ILocalRateValidationResult> validateionResult = mock.Mock<ILocalRateValidationResult>();
            validateionResult.SetupGet(r => r.RateDiscrepancies)
                .Returns(new[] {new UpsLocalRateDiscrepancy(shipment, null)});
            validateionResult.SetupGet(v => v.ValidatedShipments).Returns(new[] {shipment});

            Mock<IUpsLocalRateValidator> rateValidator = mock.Mock<IUpsLocalRateValidator>();
            rateValidator.Setup(v => v.ValidateShipments(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(validateionResult.Object);

            TelemetricUpsLocalRateValidator testObject = mock.Create<TelemetricUpsLocalRateValidator>();

            testObject.ValidateShipments(new[] {shipment});

            Mock<ITrackedEvent> trackedEvent = mock.Mock<ITrackedEvent>();

            trackedEvent.Verify(t => t.AddProperty("Rate.Local.Amount", "Not found"));
            trackedEvent.Verify(t => t.AddProperty("Rate.Local.Zone", "Not found"));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}