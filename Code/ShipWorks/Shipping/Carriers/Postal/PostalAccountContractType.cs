
namespace ShipWorks.Shipping.Carriers.Postal
{
    public enum PostalAccountContractType
    {
        /// <summary>
        /// The contract type is unknown at the moment but can/should be obtained from the postal
        /// provder's API (most likely because ShipWorks has not requested this information 
        /// from the postal provider yet).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The contract type is not applicable (most likely due to the postal provider's
        /// API not offering a way to obtain this information). This should also be used
        /// for accounts from postal providers where ShipWorks is not concerned with the type
        /// of postal account.
        /// </summary>
        NotApplicable = 1,
        
        /// <summary>
        /// Commercial Based Pricing (CBP)
        /// </summary>
        Commercial = 2,

        /// <summary>
        /// Commercial Plus Pricing/Negotiated Service Agreement (CPP/NSA)
        /// </summary>
        CommercialPlus = 3
    }
}
