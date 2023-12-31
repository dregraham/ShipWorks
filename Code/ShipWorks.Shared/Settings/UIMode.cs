﻿using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Settings
{
    /// <summary>
    /// Shipworks UI Mode
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum UIMode
    {
        [Description("Batch Mode")]
        Batch = 0,

        [Description("Order Lookup Mode")]
        OrderLookup = 1,

        [Description("Pending")]
        Pending = 2,

        [Description("Products")]
        Products = 3
    }
}
