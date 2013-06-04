using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.iParcel.Enums
{
    /// <summary>
    /// Available  i-parcel service types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum iParcelServiceType
    {
        /// <summary>
        /// The immediate service type
        /// </summary>
        [Description("Immediate")]
        [ApiValue("112")]
        Immediate = 0,

        /// <summary>
        /// The preferred service type
        /// </summary>
        [Description("Preferred")]
        [ApiValue("115")]
        Preferred = 1,

        /// <summary>
        /// The saver service type
        /// </summary>
        [Description("Saver")]
        [ApiValue("119")]
        Saver = 2,

        /// <summary>
        /// The saver deferred service type
        /// </summary>
        [Description("Saver Deferred")]
        [ApiValue("128")]
        SaverDeferred = 3
    }
}
