using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

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

        [Description("A scheduled time")]
        Scheduled = 6,

        [Description("A custom button is clicked")]
        UserInitiated = 7,

        [Description("No trigger"), Hidden]
        None = 8,
    }
}
