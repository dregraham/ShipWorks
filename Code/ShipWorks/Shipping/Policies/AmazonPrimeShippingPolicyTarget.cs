using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Target for AmazonShipmentShippingPolicy
    /// </summary>
    public class AmazonPrimeShippingPolicyTarget
    {
        /// <summary>
        /// The shipment to test
        /// </summary>
        public ShipmentEntity Shipment;

        /// <summary>
        /// Should the Amazon carrier be allowed
        /// </summary>
        public bool Allowed;

        /// <summary>
        /// Amazon credentials
        /// </summary>
        public IAmazonCredentials AmazonCredentials;

        /// <summary>
        /// Amazon order entity
        /// </summary>
        public IAmazonOrder AmazonOrder;
    }
}
