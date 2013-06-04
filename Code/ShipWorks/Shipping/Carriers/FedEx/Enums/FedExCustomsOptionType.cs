using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration for specifying options for customs; this is only used for certification tests.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExCustomsOptionType
    {
        [Description("None")]
        None = 0,

        [Description("Courtesy Return Label")]
        CourtesyReturnLabel = 1,

        [Description("Exhibition Tradeshow")]
        ExhibitionTradeShow = 2,

        [Description("Faulty Item")]
        FaultyItem = 3,

        [Description("Following Repair")]
        FollowingRepair = 4,

        [Description("For Repair")]
        ForRepair = 5,

        [Description("Item for Loan")]
        ItemForLoan = 6,

        [Description("Other")]
        Other = 7,

        [Description("Rejected")]
        Rejected = 8,

        [Description("Replacement")]
        Replacement = 9,

        [Description("Trial")]
        Trial = 10
    }
}
