using System;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// GlobalPost services we support
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [Flags]
    public enum GlobalPostServiceAvailability
    {
        [Description("None")]
        None = 0,

        [Description("GlobalPost")]
        GlobalPost = 1,

        [Description("SmartSaver")]
        SmartSaver = 2,

        [Description("InternationalFirst")]
        InternationalFirst = 4,

        [Description("InternationalPriority")]
        InternationalPriority = 8,

        [Description("InternationalExpress")]
        InternationalExpress = 16
    }
}