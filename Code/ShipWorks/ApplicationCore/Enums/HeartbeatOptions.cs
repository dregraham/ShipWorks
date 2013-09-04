using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Enums
{
    [Flags]
    public enum HeartbeatOptions
    {
        None = 0x00,
        ChangesExpected = 0x01,
        ForceGridReload = 0x02
    }
}
