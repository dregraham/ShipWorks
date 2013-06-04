using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Ebay.Enums
{
    /// <summary>
    /// An enumeration for denoting the shipping method to use for an order.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EbayShippingMethod
    {
        /// <summary>
        /// The typical shipping method
        /// </summary>
        [Description("Directly to Buyer")]
        DirectToBuyer = 0,

        /// <summary>
        /// The new shipping program being offered by eBay for international orders.
        /// </summary>
        [Description("Global Shipping Program")]
        GlobalShippingProgram = 1,

        /// <summary>
        /// A value to denote that the shipping method was overridden by the user, so
        /// we can make an informed decision about when the shipping method can/should
        /// be changed to GSP. 
        /// </summary>
        [Description("Directly to Buyer")]
        DirectToBuyerOverridden = 2
    }
}
