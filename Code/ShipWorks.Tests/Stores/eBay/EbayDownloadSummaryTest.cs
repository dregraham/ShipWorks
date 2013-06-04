using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay;

namespace ShipWorks.Tests.Stores.eBay
{
    /// <summary>
    /// Summary description for SellerTransactionResponseSummary
    /// </summary>
    [TestClass]
    public class EbayDownloadSummaryTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EbayDownloadSummary_ThrowsInvalidOperationException_WhenTransactionsPerPageIsZero_Test()
        {
            EbayDownloadSummary testObject = new EbayDownloadSummary(0, 0, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EbayDownloadSummary_ThrowsInvalidOperationException_WhenTransactionsPerPageIsNegative_Test()
        {
            EbayDownloadSummary testObject = new EbayDownloadSummary(0, -1, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void NumberOfPages_ReturnsPageCountOfZero_WhenTransactionCountIsZero_Test()
        {
            EbayDownloadSummary testObject = new EbayDownloadSummary(0, 25, DateTime.Now, DateTime.Now);
            Assert.AreEqual(0, testObject.NumberOfPages);
        }

        [TestMethod]
        public void NumberOfPages_ReturnsPageCountOfOne_WhenTransactionCountIsLessThanPageSize_Test()
        {
            EbayDownloadSummary testObject = new EbayDownloadSummary(12, 25, DateTime.Now, DateTime.Now);
            Assert.AreEqual(1, testObject.NumberOfPages);
        }

        [TestMethod]
        public void NumberOfPages_ReturnsPageCountOfOne_WhenTransactionCountEqualsPageSize_Test()
        {
            EbayDownloadSummary testObject = new EbayDownloadSummary(25, 25, DateTime.Now, DateTime.Now);
            Assert.AreEqual(1, testObject.NumberOfPages);
        }

        [TestMethod]
        public void NumberOfPages_ReturnsPageCountOfTwo_WhenTransactionCountGreaterThanPageSize_Test()
        {

            EbayDownloadSummary testObject = new EbayDownloadSummary(40, 25, DateTime.Now, DateTime.Now);
            Assert.AreEqual(2, testObject.NumberOfPages);
        }

        [TestMethod]
        public void NumberOfPages_ReturnsPageCountOfTwo_WhenTransactionCountIsDoubleThePageSize_Test()
        {

            EbayDownloadSummary testObject = new EbayDownloadSummary(50, 25, DateTime.Now, DateTime.Now);
            Assert.AreEqual(2, testObject.NumberOfPages);
        }
    }
}
