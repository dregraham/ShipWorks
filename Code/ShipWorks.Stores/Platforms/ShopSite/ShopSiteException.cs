using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Handleable exceptions thrown during ShopSite API communication
    /// </summary>
    public class ShopSiteException : Exception
    {
        public ShopSiteException()
        {

        }

        public ShopSiteException(string message) 
            : base(message)
        {

        }

        public ShopSiteException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
