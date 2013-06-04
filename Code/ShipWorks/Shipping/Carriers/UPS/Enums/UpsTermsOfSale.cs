using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Terms of sale values for UPS
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsTermsOfSale
    {
        [Description("Not Specified")]
        NotSpecified = 0,

        [Description("Cost and Freight")]
        CostFreight = 1,

        [Description("Cost, Insurance, and Freight")]
        CostInsuranceFreight = 2,

        [Description("Carriage and Insurance Paid")]
        CarriageInsurancePaid = 3,

        [Description("Carriage Paid To")]
        CarriagePaidTo = 4,

        [Description("Delivered at Frontier")]
        DeliveredAtFrontier = 5,

        [Description("Deliver Duty Paid")]
        DeliveryDutyPaid = 6,

        [Description("Deliver Duty Unpaid")]
        DeliveryDutyUnpaid = 7,

        [Description("Delivered Ex Quay")]
        DeliveredExQuay = 8,

        [Description("Delivered Ex Ship")]
        DeliveredExShip = 9,

        [Description("Ex Works")]
        ExWorks = 10,

        [Description("Free Carrier")]
        FreeCarrier = 11,

        [Description("Free Alongside Ship")]
        FreeAlongsideShip = 12,

        [Description("Free On Board")]
        FreeOnBoard = 13
    }
}
