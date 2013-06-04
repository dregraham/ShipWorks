using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Enumerates the possible row-level changes that can occur
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AuditChangeType
    {
        [Description("Add")]
        [ImageResource("add16")]
        Insert = TriggerAction.Insert,

        [Description("Edit")]
        [ImageResource("edit16")]
        Update = TriggerAction.Update,

        [Description("Delete")]
        [ImageResource("delete16")]
        Delete = TriggerAction.Delete
    }
}
