using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExPriorityAlertEnhancementType
    {
        /// <summary>
        /// The none
        /// </summary>
        [Description("None")] 
        [ApiValue("")] 
        None = 0,

        /// <summary>
        /// The priority alert plus
        /// </summary>
        [Description("FedEx Priority Alert Plus\u2122")] 
        [ApiValue("PRIORITY_ALERT_PLUS")] 
        PriorityAlertPlus = 1,

        /// <summary>
        /// The priority alert plus
        /// </summary>
        [Description("FedEx Priority Alert®")] 
        [ApiValue("")]
        PriorityAlert = 2
    }
}
