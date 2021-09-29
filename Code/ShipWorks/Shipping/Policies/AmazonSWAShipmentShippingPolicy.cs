using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// An implementation of the IShippingPolicy interface that will determine if
    /// a shipment can display Amazon as a carrier.
    /// </summary>
    public class AmazonSWAShipmentShippingPolicy : IShippingPolicy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAShipmentShippingPolicy()
        {
            AllOrdersAllowed = false;
            OnlyAmazonOrdersAllowed = false;
        }

        /// <summary>
        /// Returns true of only Amazon orders are allowed to use Amazon as a carrier.
        /// </summary>
        public bool OnlyAmazonOrdersAllowed { get; private set; }

        /// <summary>
        /// Returns true if all orders are allowed to use Amazon as a carrier.
        /// </summary>
        public bool AllOrdersAllowed { get; private set; }

        /// <summary>
        /// Uses the configuration data provided to configure the shipping policy.
        /// </summary>
        public virtual void Configure(string configuration)
        {
            if (AmazonSWAShippingRestrictionType.TryParse(configuration, out AmazonSWAShippingRestrictionType amazonShippingRestrictionType))
            {
                OnlyAmazonOrdersAllowed = amazonShippingRestrictionType.HasFlag(AmazonSWAShippingRestrictionType.OnlyAmazon);
                AllOrdersAllowed = amazonShippingRestrictionType.HasFlag(AmazonSWAShippingRestrictionType.AllOrders);
            }
            else
            {
                throw new ArgumentException("configuration is not a valid AmazonSWAShipmentShippingPolicy.");
            }
        }

        /// <summary>
        /// Determines whether the specified target is applicable to the shipping policy.
        /// </summary>
        /// <param name="target">The target is expected to be a AmazonPrimeShippingPolicyTarget.</param>
        /// <returns><c>true</c> if the specified target is applicable; otherwise, <c>false</c>.</returns>
        public virtual bool IsApplicable(object target)
        {
            AmazonShippingPolicyTarget policyTarget = target as AmazonShippingPolicyTarget;
            return policyTarget != null && policyTarget.ShipmentType == ShipmentTypeCode.AmazonSWA;
        }

        /// <summary>
        /// Applies the policy to the specified target. This will set the target Allowed property based on
        /// the configuration of the policy.
        /// </summary>
        public virtual void Apply(object target)
        {
            AmazonShippingPolicyTarget theTarget = target as AmazonShippingPolicyTarget;
            MethodConditions.EnsureArgumentIsNotNull(theTarget, nameof(theTarget));

            // Only doing this for the security scan
            if (theTarget == null)
            {
                return;
            }

            if (IsApplicable(target) && (AllOrdersAllowed || (OnlyAmazonOrdersAllowed && !string.IsNullOrWhiteSpace(theTarget?.AmazonOrder?.AmazonOrderID))))
            {
                theTarget.Allowed = true;
            }
            else
            {
                theTarget.Allowed = false;
            }
        }
    }
}
