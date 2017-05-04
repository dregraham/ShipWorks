using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class UpsPackageEntityTest
    {
        [Theory]
        [InlineData(1, 1, 5, 5)]
        [InlineData(1, 6, 1, 6)]
        [InlineData(7, 1, 1, 7)]
        [InlineData(8, 8, 8, 8)]
        [InlineData(9, 9, 1, 9)]
        [InlineData(0, 0, 0, 0)]
        public void LongestSide_IsExpectedValue(double length, double width, double height, double expectedValue)
        {
            var package = new UpsPackageEntity()
            {
                DimsLength = length,
                DimsHeight = height,
                DimsWidth = width,
            };

            Assert.Equal(expectedValue, package.LongestSide);
        }

        [Theory]
        [InlineData(1, 1, 5, 4)]
        [InlineData(1, 6, 1, 4)]
        [InlineData(7, 1, 1, 4)]
        [InlineData(8, 8, 8, 32)]
        [InlineData(9, 9, 1, 20)]
        [InlineData(0, 0, 0, 0)]
        public void Girth_IsExpectedValue(double length, double width, double height, double expectedValue)
        {
            var package = new UpsPackageEntity()
            {
                DimsLength = length,
                DimsHeight = height,
                DimsWidth = width,
            };

            Assert.Equal(expectedValue, package.Girth);
        }

        [Theory]
        [InlineData(125, 1, 1, 129, false)]
        [InlineData(126, 1, 1, 130, false)]
        [InlineData(126.1, 1, 1, 130, false)]
        [InlineData(127, 1, 1, 131, true)]
        public void IsLargePackage_IsExpectedValue(double length,
            double width,
            double height,
            double expectedLongestSidePlusGirth,
            bool expectedValue)
        {
            var package = new UpsPackageEntity()
            {
                DimsLength = length,
                DimsHeight = height,
                DimsWidth = width,
            };

            // this is just here to make sure the test is correct
            Assert.Equal(expectedLongestSidePlusGirth, package.Girth + package.LongestSide);

            Assert.Equal(expectedValue, package.IsLargePackage);
        }

        [Theory]
        [InlineData(.5, true, .1, .6)]
        [InlineData(2, true, 3, 5)]
        [InlineData(4, false, 10, 4)]
        [InlineData(4, true, 0, 4)]
        public void TotalWeight_IsExpectedValue(double weight,
            bool dimsAddWeight,
            double dimsWeight,
            double expectedValue)
        {
            var package = new UpsPackageEntity()
            {
                Weight = weight,
                DimsAddWeight = dimsAddWeight,
                DimsWeight = dimsWeight
            };

            Assert.Equal(expectedValue, package.TotalWeight);
        }

        [Theory]
        [InlineData(5, 10, 1, 100, 7)]
        [InlineData(10.1, 10, 1, 100, 11)]
        [InlineData(5, 127, 1, 1, 90)]
        [InlineData(95, 127, 1, 1, 95)]
        public void BillableWeight(double weight, double length, double width, double height, int expectedValue)
        {
            var package = new UpsPackageEntity()
            {
                Weight = weight,
                DimsLength = length,
                DimsHeight = height,
                DimsWidth = width
            };

            Assert.Equal(expectedValue, package.BillableWeight);
        }

        [Theory]
        [InlineData(10, 1, 100, 10)]
        [InlineData(10, 12, 100, 12)]
        [InlineData(127, 1, 1, 1)]
        [InlineData(127, 1, 5, 5)]
        public void SecondLongestSide(double length, double width, double height, int expectedValue)
        {
            var package = new UpsPackageEntity()
            {
                DimsLength = length,
                DimsHeight = height,
                DimsWidth = width
            };

            Assert.Equal(expectedValue, package.SecondLongestSize);
        }
    }
}
