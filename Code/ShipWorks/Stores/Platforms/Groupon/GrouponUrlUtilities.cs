using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore;
using System.Web;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// A utility class to encapsulate Groupon URL management.
    /// </summary>
    public static class GrouponUrlUtilities
    {
       
        /// <summary>
        /// Returns the URL to Groupon item page
        /// </summary>
        public static string GetItemUrl(string permalink)
        {
            return "http://www.groupon.com/deals/" + permalink;
        }
    }
}
