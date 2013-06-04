using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Valid Pickup Locations
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPickupLocation
    {
        [Description("Front Door")]
        [ApiValue("Front Door")]
        FrontDoor = 0,

        [Description("Back Door")]
        [ApiValue("Back Door")]
        BackDoor = 1,

        [Description("Side Door")]
        [ApiValue("Side Door")]
        SideDoor = 2,

        [Description("Shipping")]
        [ApiValue("Shipping")]
        Shipping = 3,

        [Description("Receiving")]
        [ApiValue("Receiving")]
        Receiving = 4,

        [Description("Reception")]
        [ApiValue("Reception")]
        Reception = 5,

        [Description("Office")]
        [ApiValue("Office")]
        Office = 6,

        [Description("Mail Room")]
        [ApiValue("Mail Room")]
        MailRoom = 7,

        [Description("Garage")]
        [ApiValue("Garage")]
        Garage = 8,

        [Description("Upstairs")]
        [ApiValue("Upstairs")]
        Upstairs = 9,

        [Description("Downstairs")]
        [ApiValue("Downstairs")]
        Downstairs = 10,

        [Description("Third Party")]
        [ApiValue("Third Party")]
        ThirdParty = 11,

        [Description("Guard Room")]
        [ApiValue("Guard Room")]
        GuardRoom = 12,

        [Description("Warehouse")]
        [ApiValue("Warehouse")]
        Warehouse = 13
    }
}