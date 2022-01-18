using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    [Obfuscation]
    public enum TrackingStatus
    {
        // Default value for existing shipments
        [Description("Not Tracked")]
        NotTracked = 0,
        
        // Value for shipments that will be tracked
        [Description("Pending")]
        Pending = 1,
        
        [Description("Accepted")]
        [ApiValue("AC")]
        Accepted = 2,
        
        [Description("In Transit")]
        [ApiValue("IT")]
        InTransit = 3,
        
        [Description("Delivered")]
        [ApiValue("DE")]
        InTransit = 4,
        
        [Description("Exception")]
        [ApiValue("EX")]
        InTransit = 5,
        
        [Description("Unknown")]
        [ApiValue("UN")]
        InTransit = 6,
        
        [Description("Delivery Attempt")]
        [ApiValue("AT")]
        InTransit = 7,
        
        [Description("Not Yet In System")]
        [ApiValue("NY")]
        NotYetInSystem = 8
    }
}