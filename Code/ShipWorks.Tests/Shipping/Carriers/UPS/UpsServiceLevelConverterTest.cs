using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    [TestClass]
    public class UpsServiceLevelConverterTest
    {
        private UpsServiceRate serviceRate;
        private UpsTransitTime upsTransitTime;

        [TestMethod]
        public void GetServiceLevel_ReturnsThreeDays_WhenPassedUps3DaySelect_Test()
        {
            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(UpsServiceType.Ups3DaySelect, null);

            Assert.AreEqual(ServiceLevelType.ThreeDays, serviceLevel);
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsAnytime_WhenPassedUpsGroundAndNegativeOneDays_Test()
        {
            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(UpsServiceType.UpsGround, -1);

            Assert.AreEqual(ServiceLevelType.Anytime, serviceLevel);
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUps2DayAirAMAndNull_Test()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.Ups2DayAirAM, 1, false, 5);
            upsTransitTime = new UpsTransitTime(UpsServiceType.Ups2DayAirAM, 1, DateTime.Today);

            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.AreEqual(ServiceLevelType.TwoDays, serviceLevel);
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUpsGroundAndServiceRateDefines2Days_Test()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.UpsGround, 1, false, 2);
            upsTransitTime = new UpsTransitTime(UpsServiceType.UpsGround, 1, DateTime.Today);

            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.AreEqual(ServiceLevelType.TwoDays, serviceLevel);
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUpsGroundAndTransitTimeDefines2Days_Test()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.UpsGround, 1, false, null);
            upsTransitTime = new UpsTransitTime(UpsServiceType.UpsGround, 2, DateTime.Today);

            ServiceLevelType serviceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.AreEqual(ServiceLevelType.TwoDays, serviceLevel);
        }
    }
}
