using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    /// <summary>
    /// Feedback status values used in Filter Conditions
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EbayFeedbackConditionStatusType
    {
        [Description("Has been left for buyer")]
        SellerLeftForBuyer = 0,

        [Description("Has not been left for buyer")]
        SellerNotLeftForBuyer = 1,

        [Description("Received positive feedback")]
        BuyerLeftPositive = 2,

        [Description("Received negative feedback")]
        BuyerLeftNegative = 3,

        [Description("Received neutral feedback")]
        BuyerLeftNeutral = 4,

        [Description("Has not been received")]
        BuyerNotLeft = 5
    }
}
