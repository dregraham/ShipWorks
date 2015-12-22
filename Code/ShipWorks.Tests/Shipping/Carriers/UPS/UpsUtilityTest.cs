using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsUtilityTest
    {
        [Fact]
        public void CorrectSmartPickupError_ChangesStToSaint_WhenCityStartsWithSt_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("St Louis");

            Assert.Equal("Saint Louis", fixedCity);
        }

        [Fact]
        public void CorrectSmartPickupError_ChangesSteToSaint_WhenCityStartsWithSte_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("Ste Genevieve");

            Assert.Equal("Saint Genevieve", fixedCity);
        }

        [Fact]
        public void CorrectSmartPickupError_ChangesStPeriodToSaint_WhenCityStartsWithStPeriod_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("St. Paul");

            Assert.Equal("Saint Paul", fixedCity);
        }

        [Fact]
        public void CorrectSmartPickupError_ChangesSaintToSt_WhenCityStartsWithSaint_Test()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("Saint Paul");

            Assert.Equal("St Paul", fixedCity);
        }
    }
}
