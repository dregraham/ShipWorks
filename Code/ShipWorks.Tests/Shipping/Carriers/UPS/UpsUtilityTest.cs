using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    [TestClass]
    public class UpsUtilityTest
    {
        [TestMethod]
        public void CorrectSmartPickupError_ChangesStToSaint_WhenCityStartsWithSt_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("St Louis");

            Assert.AreEqual("Saint Louis", fixedCity);
        }

        [TestMethod]
        public void CorrectSmartPickupError_ChangesSteToSaint_WhenCityStartsWithSte_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("Ste Genevieve");

            Assert.AreEqual("Saint Genevieve", fixedCity);
        }

        [TestMethod]
        public void CorrectSmartPickupError_ChangesStPeriodToSaint_WhenCityStartsWithStPeriod_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("St. Paul");

            Assert.AreEqual("Saint Paul", fixedCity);
        }

        [TestMethod]
        public void CorrectSmartPickupError_ChangesSaintToSt_WhenCityStartsWithSaint_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("Saint Paul");

            Assert.AreEqual("St Paul", fixedCity);
        }
    }
}
