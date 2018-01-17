using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Flags]
    public enum FedExEmailNotificationType
    {
        None = 0x0000,

        Ship = 0x0001,

        Exception = 0x0002,

        Deliver = 0x0004,

        EstimatedDelivery = 0x0008
    }
}
