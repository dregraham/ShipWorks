using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Brightpearl;

namespace ShipWorks.Tests.Stores.Brightpearl
{
    [TestClass]
    public class BrightPearlUtilityTest
    {
        private const string testUrl = @"https://ws-eu1.brightpearl.com/external-request/accountCode/shipworks-service/3.0/action";
        
        [TestMethod]
        public void GetAccountId_GetsAccountID_Test()
        {
            string accountId = BrightpearlUtility.GetAccountId(testUrl);

            Assert.AreEqual("accountCode", accountId);
        }

        [TestMethod]
        public void GetTimeZone_GetsGmtOrCet_Test()
        {
            BrightpearlServerTimeZoneType timeZone = BrightpearlUtility.GetTimeZone(testUrl);

            Assert.AreEqual(BrightpearlServerTimeZoneType.Eu1, timeZone);
        }

        [TestMethod]
        public void GetModuleUrl_GetsModuleUrl_Test()
        {
            string moduleUrl = BrightpearlUtility.GetModuleUrl("accountCode", BrightpearlServerTimeZoneType.Eu1);

            Assert.AreEqual(testUrl, moduleUrl);
        }
    }
}
