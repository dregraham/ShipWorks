using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Base for all ProStores exceptions we care about catching
    /// </summary>
    public class ProStoresException : Exception
    {
        public ProStoresException()
        {

        }

        public ProStoresException(string message)
            : base(message)
        {

        }

        public ProStoresException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
