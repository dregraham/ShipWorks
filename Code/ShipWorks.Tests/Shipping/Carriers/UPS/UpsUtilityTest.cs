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
        public void CorrectSmartPickupError_ChangesStToSaint_WhenCityStartsWithSt()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("St Louis");

            Assert.Equal("Saint Louis", fixedCity);
        }

        [Fact]
        public void CorrectSmartPickupError_ChangesSteToSaint_WhenCityStartsWithSte()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("Ste Genevieve");

            Assert.Equal("Saint Genevieve", fixedCity);
        }

        [Fact]
        public void CorrectSmartPickupError_ChangesStPeriodToSaint_WhenCityStartsWithStPeriod()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("St. Paul");

            Assert.Equal("Saint Paul", fixedCity);
        }

        [Fact]
        public void CorrectSmartPickupError_ChangesSaintToSt_WhenCityStartsWithSaint()
        {
            string fixedCity = UpsUtility.CorrectSmartPickupError("Saint Paul");

            Assert.Equal("St Paul", fixedCity);
        }
    }
}
