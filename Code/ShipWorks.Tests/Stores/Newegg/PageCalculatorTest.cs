using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg.Net;

namespace ShipWorks.Tests.Stores.Newegg
{
    [TestClass]
    public class PageCalculatorTest
    {
        PageCalculator testObject;

        public PageCalculatorTest()
        {
            testObject = new PageCalculator();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CalculatePageCount_ThrowsInvalidOperationException_WhenMaxPageSizeIsZero_Test()
        {
            testObject.CalculatePageCount(10, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CalculatePageCount_ThrowsInvalidOperationException_WhenMaxPageSizeIsLessThanZero_Test()
        {
            testObject.CalculatePageCount(10, -1);
        }

        [TestMethod]
        public void CalculatePageCount_WhenZeroResults_Test()
        {
            int pageCount = testObject.CalculatePageCount(0, 25);
            Assert.AreEqual(0, pageCount);
        }

        [TestMethod]
        public void CalculatesPageCount_WhenNumberOfResultsGreaterThanMaxPageSize_Test()
        {
            int pageCount = testObject.CalculatePageCount(42, 25);
            Assert.AreEqual(2, pageCount);
        }

        [TestMethod]
        public void CalculatesPageCount_WhenNumberOfResultsEqualsMaxPageSize_Test()
        {
            int pageCount = testObject.CalculatePageCount(25, 25);
            Assert.AreEqual(1, pageCount);
        }

        [TestMethod]
        public void CalculatesPageCount_WhenNumberOfResultsIsMultipleOfMaxPageSize_Test()
        {
            int pageCount = testObject.CalculatePageCount(50, 25);
            Assert.AreEqual(2, pageCount);
        }
    }
}
