using System;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Enum representing FedExFreightSpecialServices
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [Flags]
    public enum FedExFreightSpecialServicesType
    {
        [Description("None")]
        [ApiValue("")]
        None = 0x0000,

        [Description("Call Before Delivery")]
        [ApiValue("CALL_BEFORE_DELIVERY")]
        CallBeforeDelivery = 0x0001,

        [Description("Freezable Protection")]
        [ApiValue("PROTECTION_FROM_FREEZING")]
        FreezableProtection = 0x0002,

        [Description("Limited Access Pickup")]
        [ApiValue("LIMITED_ACCESS_PICKUP")]
        LimitedAccessPickup = 0x0004,

        [Description("Limited Access Delivery")]
        [ApiValue("LIMITED_ACCESS_DELIVERY")]
        LimitedAccessDelivery = 0x0008,
        
        [Description("Poison")]
        [ApiValue("POISON")]
        Poison = 0x0020,
        
        [Description("Food")]
        [ApiValue("FOOD")]
        Food = 0x0080,
        
        [Description("Do Not Stack Pallets")]
        [ApiValue("DO_NOT_STACK_PALLETS")]
        DoNotStackPallets = 0x0200,
        
        [Description("Do Not Break Down Pallets")]
        [ApiValue("DO_NOT_BREAK_DOWN_PALLETS")]
        DoNotBreakDownPallets = 0x1000,

        [Description("Top Load Only")]
        [ApiValue("TOP_LOAD")]
        TopLoad = 0x2000,

        [Description("Oversize/Extreme length")]
        [ApiValue("EXTREME_LENGTH")]
        ExtremeLength = 0x4000,

        [Description("Liftgate at delivery")]
        [ApiValue("LIFTGATE_DELIVERY")]
        LiftgateAtDelivery = 0x8000,

        [Description("Liftgate at pickup")]
        [ApiValue("LIFTGATE_PICKUP")]
        LiftgateAtPickup = 0x10000,

        [Description("Inside delivery")]
        [ApiValue("INSIDE_DELIVERY")]
        INSIDE_DELIVERY = 0x20000,

        [Description("Inside pickup")]
        [ApiValue("INSIDE_PICKUP")]
        INSIDE_PICKUP = 0x40000,

        [Description("Freight Guarantee")]
        [ApiValue("FREIGHT_GUARANTEE")]
        FreightGuarantee = 0x80000,
    }
}
