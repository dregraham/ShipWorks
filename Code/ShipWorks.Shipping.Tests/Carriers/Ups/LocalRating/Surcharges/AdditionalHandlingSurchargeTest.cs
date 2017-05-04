using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class AdditionalHandlingSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AdditionalHandlingSurcharge testObject;

        public AdditionalHandlingSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new AdditionalHandlingSurcharge(
                new Dictionary<UpsSurchargeType, double>
                {
                    {UpsSurchargeType.AdditionalHandling, 123}
                });
        }

        [Fact]
        public void Apply_AddsAdditionalHandlingSurcharge_WhenAdditionalHandlingEnabled()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.UpsNextDayAir,
                    Packages = {new UpsPackageEntity {AdditionalHandlingEnabled = true}}
                };
            
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.AdditionalHandling)));
        }

        [Fact]
        public void Apply_AddsAdditionalHandlingSurcharge_WhenLongestSideIsGreaterThan48()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.UpsNextDayAir,
                    Packages =
                    {
                        new UpsPackageEntity
                        {
                            AdditionalHandlingEnabled = false,
                            DimsLength = 0,
                            DimsHeight = 0,
                            DimsWidth = 50
                        }
                    }
                };

            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.AdditionalHandling)));
        }


        [Fact]
        public void Apply_AddsAdditionalHandlingSurcharge_WhenSecondLongestSideIsGreaterThan30()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity
                {
                    Service = (int) UpsServiceType.UpsNextDayAir,
                    Packages =
                    {
                        new UpsPackageEntity
                        {
                            AdditionalHandlingEnabled = false,
                            DimsLength = 0,
                            DimsHeight = 32,
                            DimsWidth = 33
                        }
                    }
                };

            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.AdditionalHandling)));
        }

        [Fact]
        public void Apply_DoesNotAddAdditionalHandlingSurcharge_WhenShipmentDoesNotQualify()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity
                {
                    Service = (int)UpsServiceType.UpsNextDayAir,
                    Packages =
                    {
                        new UpsPackageEntity
                        {
                            AdditionalHandlingEnabled = false,
                            DimsLength = 3,
                            DimsHeight = 3,
                            DimsWidth = 3
                        }
                    }
                };

            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.AdditionalHandling)), Times.Never);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}