﻿using System;
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
        NewShipment = 0b1,
        Rates = 0b10,
        Profiles = 0b100,
    }

    /// <summary>
    /// Attribute for decorating enumeration values that should not be visible in provided context.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HiddenForAttribute : Attribute
    {
        public HiddenForAttribute(HiddenForContext context)
        {
            Context = context;
        }
        public HiddenForContext Context { get; set; }
    }
}
