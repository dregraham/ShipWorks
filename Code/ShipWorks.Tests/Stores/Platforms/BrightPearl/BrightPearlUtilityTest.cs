using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Stores.Platforms.Brightpearl;

namespace ShipWorks.Tests.Stores.Brightpearl
{
    public class BrightPearlUtilityTest
    {
        private const string testUrl = @"https://ws-eu1.brightpearl.com/external-request/accountCode/shipworks-service/3.0/action";
        
        [Fact]
        public void GetAccountId_GetsAccountID_Test()
        {
            string accountId = BrightpearlUtility.GetAccountId(testUrl);

            Assert.Equal("accountCode", accountId);
        }

        [Fact]
        public void GetTimeZone_GetsGmtOrCet_Test()
        {
            BrightpearlServerTimeZoneType timeZone = BrightpearlUtility.GetTimeZone(testUrl);

            Assert.Equal(BrightpearlServerTimeZoneType.Eu1, timeZone);
        }

        [Fact]
        public void GetModuleUrl_GetsModuleUrl_Test()
        {
            string moduleUrl = BrightpearlUtility.GetModuleUrl("accountCode", BrightpearlServerTimeZoneType.Eu1);

            Assert.Equal(testUrl, moduleUrl);
        }
    }
}
