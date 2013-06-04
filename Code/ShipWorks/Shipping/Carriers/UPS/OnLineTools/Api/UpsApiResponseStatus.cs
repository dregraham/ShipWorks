using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Possible status results from UPS
    /// </summary>
    public enum UpsApiResponseStatus
    {
        Success,
        Transient,
        Hard,
        Warning
    }
}
