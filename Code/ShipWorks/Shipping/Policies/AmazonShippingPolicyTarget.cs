using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Target for AmazonShipmentShippingPolicy
    /// </summary>
    public class AmazonShippingPolicyTarget
    {
        private ShipmentTypeCode shipmentType;

        /// <summary>
        /// The shipmenttype this policy targets
        /// </summary>
        public ShipmentTypeCode ShipmentType
        {
            get => shipmentType;
            set
            {
                if (value != ShipmentTypeCode.AmazonSFP &&
                    value != ShipmentTypeCode.AmazonSWA)
                {
                    throw new InvalidOperationException("cannot use non amazon ShipmentType for AmazonShippingPolicyTarget");
                }

                shipmentType = value;
            }
        }

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
