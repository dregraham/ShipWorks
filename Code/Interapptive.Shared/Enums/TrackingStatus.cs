using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TrackingStatus
    {
        // Default value for existing shipments
        [Description("Not Tracked")]
        NotTracked = 0,
        
        // Value for shipments that will be tracked
        [Description("Pending")]
        Pending = 1,
        
        // Shipment uploaded to hub, but haven't gotten any updates
        [Description("Awaiting Update")]
        AwaitingUpdate = 2,

        [Description("Accepted")]
        [ApiValue("AC")]
        Accepted = 3,
        
        [Description("In Transit")]
        [ApiValue("IT")]
        InTransit = 4,
        
        [Description("Delivered")]
        [ApiValue("DE")]
        Delivered = 5,
        
        [Description("Exception")]
        [ApiValue("EX")]
        Exception = 6,
        
        [Description("Unknown")]
        [ApiValue("UN")]
        Unknown = 7,
        
        [Description("Delivery Attempt")]
        [ApiValue("AT")]
        Attempted = 8,
        
        [Description("Not Yet In System")]
        [ApiValue("NY")]
        NotYetInSystem = 9
    }
}