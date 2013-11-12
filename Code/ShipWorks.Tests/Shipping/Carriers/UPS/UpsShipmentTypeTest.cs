using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    [TestClass]
    public class UpsShipmentTypeTest
    {
        UpsShipmentType testObject = new UpsOltShipmentType();

        private UpsServiceRate serviceRate;
        private UpsTransitTime upsTransitTime;

        [TestMethod]
        public void GetServiceLevel_ReturnsThreeDays_WhenPassedUps3DaySelect_Test()
        {
            ServiceLevelType serviceLevel = testObject.GetServiceLevel(UpsServiceType.Ups3DaySelect, null);

            Assert.AreEqual(ServiceLevelType.ThreeDays, serviceLevel); 
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsAnytime_WhenPassedUpsGroundAndNegativeOneDays_Test()
        {
            ServiceLevelType serviceLevel = testObject.GetServiceLevel(UpsServiceType.UpsGround, -1);

            Assert.AreEqual(ServiceLevelType.Anytime, serviceLevel);
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUps2DayAirAMAndNull_Test()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.Ups2DayAirAM, 1, false, 5);
            upsTransitTime = new UpsTransitTime(UpsServiceType.Ups2DayAirAM, 1, DateTime.Today);

            ServiceLevelType serviceLevel = testObject.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.AreEqual(ServiceLevelType.TwoDays, serviceLevel);
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUpsGroundAndServiceRateDefines2Days_Test()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.UpsGround, 1, false, 2);
            upsTransitTime = new UpsTransitTime(UpsServiceType.UpsGround, 1, DateTime.Today);

            ServiceLevelType serviceLevel = testObject.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.AreEqual(ServiceLevelType.TwoDays, serviceLevel);
        }

        [TestMethod]
        public void GetServiceLevel_ReturnsTwoDays_WhenPassedUpsGroundAndTransitTimeDefines2Days_Test()
        {
            serviceRate = new UpsServiceRate(UpsServiceType.UpsGround, 1, false, null);
            upsTransitTime = new UpsTransitTime(UpsServiceType.UpsGround, 2, DateTime.Today);

            ServiceLevelType serviceLevel = testObject.GetServiceLevel(serviceRate, upsTransitTime);

            Assert.AreEqual(ServiceLevelType.TwoDays, serviceLevel);
        }

        [TestMethod]
        public void CalculateExpectedDeliveryDate_Returns2DaysFromNow_Ship2DayOnAMonday_Test()
        {
            DateTime aMonday = new DateTime(2013, 11, 11);
            DateTime? deliveryDate = UpsShipmentType.CalculateExpectedDeliveryDate(2, aMonday);

            Assert.AreEqual(aMonday.AddDays(2), deliveryDate);
        }

        [TestMethod]
        public void CalculateExpectedDeliveryDate_Returns4DaysFromNow_Ship2DayOnAFriday_Test()
        {
            DateTime aFriday = new DateTime(2013, 11, 15);
            DateTime? deliveryDate = UpsShipmentType.CalculateExpectedDeliveryDate(2, aFriday);

            Assert.AreEqual(aFriday.AddDays(4), deliveryDate);
        }

    }
}
