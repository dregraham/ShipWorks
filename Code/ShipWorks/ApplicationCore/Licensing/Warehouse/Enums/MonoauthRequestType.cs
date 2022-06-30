using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.Enums
{
    /// <summary>
    /// The type of monoauth request
    /// </summary>
    public enum MonoauthRequestType
    {
        CreateOrderSource = 0,
        UpdateOrderSourceCredentials = 1,
        CreateCarrier = 2,
        UpdateCarrierCredentials = 3,
    }
}