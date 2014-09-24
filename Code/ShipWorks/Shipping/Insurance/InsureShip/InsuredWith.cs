using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum InsuredWith
    {
        [Description("Not with API")]
        [ApiValue("0")]
        NotWithApi = 0,

        [Description("Sucessfully Insured Via the API")]
        [ApiValue("1")]
        SuccessfullyInsuredViaApi = 1,

        [Description("Failed To Insure Via the API")]
        [ApiValue("2")]
        FailedToInsureViaApi = 2
    }
}

