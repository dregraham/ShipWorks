using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Business.Geography
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CurrencyCode
    {
        [Display(Name = @"US Dollar", ShortName = @"$")]
        [ApiValue("en-US")]
        USD = 0,

        [Display(Name = @"British Pound", ShortName = @"£")]
        [ApiValue("en-GB")]
        GBP = 1,

        [Display(Name = @"Japanese Yen", ShortName = @"¥")]
        [ApiValue("ja-JP")]
        JPY
    }
}