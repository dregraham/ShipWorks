using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Insurance.InsureShip.Enums
{
    /// <summary>
    /// InsureShip API Response codes
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum InsureShipResponseCode
    {
        [Description("Success")]
        [ApiValue("204")]
        Success = 0,

        [Description("Incorrect Affiliate ID")]
        [ApiValue("311")]
        IncorrectAffiliateID = 1,

        [Description("Incorrect Insurance rate in the insurance field")]
        [ApiValue("312")]
        IncorrectInsuranceRate = 2,

        [Description("Bad Credentials")]
        [ApiValue("401")]
        BadCredentials = 3,

        [Description("Credit Card Payment failed")]
        [ApiValue("402")]
        CreditCardPaymentFailed = 4,

        [Description("Conflict")]
        [ApiValue("409")]
        Conflict = 5,

        [Description("Missing Required Parameter")]
        [ApiValue("412")]
        MissingRequiredParameter = 6,

        [Description("Unknown Failure")]
        [ApiValue("419")]
        UnknownFailure = 7
    }
}
