using System;
using Xunit;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsServiceLevelConverterTest
    {
        private UpsServiceRate serviceRate;
        private UpsTransitTime upsTransitTime;

        [Fact]
        public void GetServiceLevel_ReturnsThreeDays_WhenPassedUps3DaySelect()
        {
            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(UpsServiceType.Ups3DaySelect, null);

            Assert.Equal(ServiceLevelType.ThreeDays, serviceLevel);
        }

        [Fact]
        public void GetServiceLevel_ReturnsAnytime_WhenPassedUpsGroundAndNegativeOneDays()
        {
            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(UpsServiceType.UpsGround, -1);

            Assert.Equal(ServiceLevelType.Anytime, serviceLevel);
        }

        [Fact]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUps2DayAirAMAndNull()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.Ups2DayAirAM, 1, false, 5);
            upsTransitTime = new UpsTransitTime(UpsServiceType.Ups2DayAirAM, 1, DateTime.Today);

            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.Equal(ServiceLevelType.TwoDays, serviceLevel);
        }

        [Fact]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUpsGroundAndServiceRateDefines2Days()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.UpsGround, 1, false, 2);
            upsTransitTime = new UpsTransitTime(UpsServiceType.UpsGround, 1, DateTime.Today);

            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.Equal(ServiceLevelType.TwoDays, serviceLevel);
        }

        [Fact]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUpsGroundAndTransitTimeDefines2Days()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.UpsGround, 1, false, null);
            upsTransitTime = new UpsTransitTime(UpsServiceType.UpsGround, 2, DateTime.Today);

            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.Equal(ServiceLevelType.TwoDays, serviceLevel);
        }
    }
}
