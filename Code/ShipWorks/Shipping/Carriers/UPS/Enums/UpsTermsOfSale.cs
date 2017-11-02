using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Terms of sale values for UPS
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsTermsOfSale
    {
        [Description("Not Specified")]
        [ApiValue("")]
        NotSpecified = 0,

        [Description("Cost and Freight")]
        [ApiValue("CFR")]
        CostFreight = 1,

        [Description("Cost, Insurance, and Freight")]
        [ApiValue("CIF")]
        CostInsuranceFreight = 2,

        [Description("Carriage and Insurance Paid")]
        [ApiValue("CIP")]
        CarriageInsurancePaid = 3,

        [Description("Carriage Paid To")]
        [ApiValue("CPT")]
        CarriagePaidTo = 4,

        [Description("Delivered at Frontier")]
        [ApiValue("DAF")]
        DeliveredAtFrontier = 5,

        [Description("Deliver Duty Paid")]
        [ApiValue("DDP")]
        DeliveryDutyPaid = 6,

        [Description("Deliver Duty Unpaid")]
        [ApiValue("DDU")]
        DeliveryDutyUnpaid = 7,

        [Description("Delivered Ex Quay")]
        [ApiValue("DEQ")]
        DeliveredExQuay = 8,

        [Description("Delivered Ex Ship")]
        [ApiValue("DES")]
        DeliveredExShip = 9,

        [Description("Ex Works")]
        [ApiValue("EXW")]
        ExWorks = 10,

        [Description("Free Carrier")]
        [ApiValue("FCA")]
        FreeCarrier = 11,

        [Description("Free Alongside Ship")]
        [ApiValue("FAS")]
        FreeAlongsideShip = 12,

        [Description("Free On Board")]
        [ApiValue("FOB")]
        FreeOnBoard = 13
    }
}
