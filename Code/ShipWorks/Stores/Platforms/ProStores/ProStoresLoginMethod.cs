using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// The different ways to login to prostores
    /// </summary>
    public enum ProStoresLoginMethod
    {
        LegacyUserPass = 0,
        ApiToken = 1
    }
}
