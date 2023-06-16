using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.Utility
{
    [Flags]
    public enum HiddenForContext
    {
        None = 0,
        NewShipment = 1,
        Rates = 2
    }

    /// <summary>
    /// Attribute for decorating enumeration values that should not be visible in provided context.
    /// </summary>
    public class HiddenForAttribute : Attribute
    {
        public HiddenForAttribute(HiddenForContext context)
        {
            Context = context;
        }
        public HiddenForContext Context { get; set; }
    }
}
