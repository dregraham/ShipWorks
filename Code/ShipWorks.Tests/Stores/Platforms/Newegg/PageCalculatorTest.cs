using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Stores.Platforms.Newegg.Net;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class PageCalculatorTest
    {
        PageCalculator testObject;

        public PageCalculatorTest()
        {
            testObject = new PageCalculator();
        }

        [Fact]
        public void CalculatePageCount_ThrowsInvalidOperationException_WhenMaxPageSizeIsZero_Test()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CalculatePageCount(10, 0));
        }

        [Fact]
        public void CalculatePageCount_ThrowsInvalidOperationException_WhenMaxPageSizeIsLessThanZero_Test()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CalculatePageCount(10, -1));
        }

        [Fact]
        public void CalculatePageCount_WhenZeroResults_Test()
        {
            int pageCount = testObject.CalculatePageCount(0, 25);
            Assert.Equal(0, pageCount);
        }

        [Fact]
        public void CalculatesPageCount_WhenNumberOfResultsGreaterThanMaxPageSize_Test()
        {
            int pageCount = testObject.CalculatePageCount(42, 25);
            Assert.Equal(2, pageCount);
        }

        [Fact]
        public void CalculatesPageCount_WhenNumberOfResultsEqualsMaxPageSize_Test()
        {
            int pageCount = testObject.CalculatePageCount(25, 25);
            Assert.Equal(1, pageCount);
        }

        [Fact]
        public void CalculatesPageCount_WhenNumberOfResultsIsMultipleOfMaxPageSize_Test()
        {
            int pageCount = testObject.CalculatePageCount(50, 25);
            Assert.Equal(2, pageCount);
        }
    }
}
