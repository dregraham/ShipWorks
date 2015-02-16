using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UspsAccountContractType
    {
        /// <summary>
        /// The contract type is unknown at the moment but can/should be obtained from the postal
        /// provder's API. An account is most likely in this state immediately after creating the 
        /// account until it has been completely processed by Stamps.com.
        /// </summary>
        [Description("Pending")]
        Unknown = 0,

        /// <summary>
        /// The contract type is not applicable. This should also be used for accounts from postal 
        /// providers where ShipWorks is not concerned with the type of postal account.
        /// </summary>
        [Description("Not Applicable")]
        NotApplicable = 1,
        
        /// <summary>
        /// Commercial Based Pricing (CBP)
        /// </summary>
        [Description("CBP")]
        Commercial = 2,

        /// <summary>
        /// Commercial Plus Pricing/Negotiated Service Agreement (CPP/NSA)
        /// </summary>
        [Description("CPP/NSA")]
        CommercialPlus = 3,

        /// <summary>
        /// Reseller - displays as Discounted in the UI rather than Reseller, so customers don't infer
        /// that some third party is making a profit.
        /// </summary>
        [Description("Discounted")]
        Reseller = 4
    }
}
