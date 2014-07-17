using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.BrightPearl;

namespace ShipWorks.Tests.Stores.BrightPearl
{
    [TestClass]
    public class BrightPearlUtilityTest
    {
        private const string testUrl = @"https://ws-eu1.brightpearl.com/external-request/accountCode/shipworks-service/3.0/action";
        
        [TestMethod]
        public void GetAccountId_GetsAccountID_Test()
        {
            string accountId = BrightPearlUtility.GetAccountId(testUrl);

            Assert.AreEqual("accountCode", accountId);
        }

        [TestMethod]
        public void GetTimeZone_GetsGmtOrCet_Test()
        {
            BrightPearlServerTimeZoneType timeZone = BrightPearlUtility.GetTimeZone(testUrl);

            Assert.AreEqual(BrightPearlServerTimeZoneType.Eu1, timeZone);
        }

        [TestMethod]
        public void GetModuleUrl_GetsModuleUrl_Test()
        {
            string moduleUrl = BrightPearlUtility.GetModuleUrl("accountCode", BrightPearlServerTimeZoneType.Eu1);

            Assert.AreEqual(testUrl, moduleUrl);
        }
    }
}
