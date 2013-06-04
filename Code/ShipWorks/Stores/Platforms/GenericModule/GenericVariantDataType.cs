using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Some data for Generic Store's are variant - such as the CustomerID or OnlineStatusCode.  This let's the 
    /// store specifiy if its a string-based value or numeric-based value
    /// </summary>
    public enum GenericVariantDataType
    {
        Numeric = 0,
        Text = 1
    }
}
