using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CurrencyType
    {
        [Description("USD")]
        [ApiValue("USD")]
        USD = 0,

        [Description("CAD")]
        [ApiValue("CAD")]
        CAD = 1,

        [Description("UKL")]
        [ApiValue("UKL")]
        UKL = 2,

        [Description("EUR")]
        [ApiValue("EUR")]
        EUR = 3
    }
}
