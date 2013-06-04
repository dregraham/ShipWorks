using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Editions.Brown
{
    /// <summary>
    /// Restriction of what postal services are allowed to be used in a brown-only install
    /// </summary>
    public enum BrownPostalAvailability
    {
        None        = 0x0000,

        ApoFpoPobox = 0x0001,

        AllServices = 0xFFFF
    }
}
