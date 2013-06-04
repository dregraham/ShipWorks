using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS InvoiceRegistration response status codes
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsInvoiceRegistrationResponseStatusCode
    {
        [Description("Failed")]
        [ApiValue("0")]
        Failed = 0,

        [Description("Success")]
        [ApiValue("1")]
        Success = 1
    }
}
