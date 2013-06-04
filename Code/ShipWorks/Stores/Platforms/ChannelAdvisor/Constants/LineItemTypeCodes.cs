using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Constants
{
    /// <summary>
    /// A class of constants for line item type codes with version 6 of the 
    /// Channel Advisor API - the enumerations that were available in previous
    /// versions are no longer available as these values are now passed
    /// along as string values. The intent of this class is to avoid 
    /// having "magic" string values littered throughout the Channel Advisor
    /// codebase.
    /// </summary>
    public static class LineItemTypeCodes
    {
        public const string AdditionalCostOrDiscount = "AdditionalCostOrDiscount";
        public const string BuyerOptInIncentive = "BuyerOptInIncentive";
        public const string GiftWrap = "GiftWrap";
        public const string Listing = "Listing";
        public const string Promotion = "Promotion";
        public const string RecyclingFee = "RecyclingFee";
        public const string SKU = "SKU";
        public const string SalesTax = "SalesTax";
        public const string Shipping = "Shipping";
        public const string ShippingInsurance = "Shipping Insurance";
        public const string VATGiftWrap = "VATGiftWrap";
        public const string VATShipping = "VATShipping";
    }
}
