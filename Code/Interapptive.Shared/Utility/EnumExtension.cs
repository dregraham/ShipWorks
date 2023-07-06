using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.Utility
{
    public static class EnumExtension
    {
        public static bool IsHiddenFor(this Enum value, HiddenForContext context)
        {
            return EnumHelper.IsHiddenFor(value, context);
        }
    }
}
