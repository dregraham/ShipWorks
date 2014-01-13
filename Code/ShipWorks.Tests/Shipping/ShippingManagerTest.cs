using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shipping
{
    [TestClass]
    public class ShippingManagerTest
    {
        [TestMethod]
        public void CalculateExpectedDeliveryDate_Returns2DaysFromNow_Ship2DayOnAMonday_Test()
        {
            DateTime aMonday = new DateTime(2013, 11, 11);
            DateTime? deliveryDate = ShippingManager.CalculateExpectedDeliveryDate(2, aMonday, DayOfWeek.Saturday, DayOfWeek.Sunday);

            Assert.AreEqual(aMonday.AddDays(2), deliveryDate);
        }

        [TestMethod]
        public void CalculateExpectedDeliveryDate_Returns4DaysFromNow_Ship2DayOnAFriday_Test()
        {
            DateTime aFriday = new DateTime(2013, 11, 15);
            DateTime? deliveryDate = ShippingManager.CalculateExpectedDeliveryDate(2, aFriday, DayOfWeek.Saturday, DayOfWeek.Sunday);

            Assert.AreEqual(aFriday.AddDays(4), deliveryDate);
        }
    }
}
