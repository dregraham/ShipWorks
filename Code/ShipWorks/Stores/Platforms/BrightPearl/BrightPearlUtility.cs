using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.BrightPearl
{
    public static class BrightPearlUtility
    {
        /// <summary>
        /// Gets the account identifier from the moduleUrl
        /// </summary>
        public static string GetAccountId(string moduleUrl)
        {
            string[] moduleUrlPieces = moduleUrl.Split('/');

            return moduleUrlPieces[4];
        }

        /// <summary>
        /// Gets the time zone from the moduleUrl
        /// </summary>
        public static BrightPearlServerTimeZoneType GetTimeZone(string moduleUrl)
        {
            string timeZoneCode = moduleUrl.Substring(11, 3);
            return EnumHelper.GetEnumByApiValue<BrightPearlServerTimeZoneType>(timeZoneCode);
        }

        /// <summary>
        /// Gets the module URL from the accountID and timeZone.
        /// </summary>
        public static string GetModuleUrl(string accountID, BrightPearlServerTimeZoneType timeZone)
        {
            return string.Format(@"https://ws-{0}.brightpearl.com/external-request/{1}/shipworks-service/3.0/action",
                EnumHelper.GetApiValue(timeZone),
                accountID);
        }

    }
}
