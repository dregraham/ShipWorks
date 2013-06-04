using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.EquaShip.Enums
{
    /// <summary>
    /// Where in the response objects returned by EquaShip the real errors should be located
    /// </summary>
    public enum ErrorResponseLocation
    {
        // Status property
        Status = 0,

        // Errors property
        Errors = 1
    }
}
