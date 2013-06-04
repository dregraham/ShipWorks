using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DownloadedPresenceType
    {
        [Description("For the first time")]
        InitialDownload,

        [Description("As an update")]
        AlreadyPresent,

        [Description("For the first time or as an update")]
        Either
    }
}
