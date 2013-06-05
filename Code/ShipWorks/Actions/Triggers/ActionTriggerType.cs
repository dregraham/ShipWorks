using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// All possible action triggers
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionTriggerType
    {
        [Description("An order is downloaded")]
        OrderDownloaded = 0,

        [Description("A download finishes")]
        DownloadFinished = 1,

        [Description("A shipment is processed")]
        ShipmentProcessed = 2,

        [Description("A shipment is voided")]
        ShipmentVoided = 3,

        [Description("A filter's content changes")]
        FilterContentChanged = 5,

        [Description("At a specific time")]
        Cron = 6
    }
}
