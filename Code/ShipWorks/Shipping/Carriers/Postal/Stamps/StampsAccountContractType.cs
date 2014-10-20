
namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    public enum StampsAccountContractType
    {
        /// <summary>
        /// The contract type is unknown at the moment but can/should be obtained from the postal
        /// provder's API.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The contract type is not applicable. This should also be used for accounts from postal 
        /// providers where ShipWorks is not concerned with the type of postal account.
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
