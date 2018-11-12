using System;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// An implementation of the IShippingPolicy interface that will determine if
    /// a shipment can display Amazon as a carrier.
    /// </summary>
    public class AmazonShipmentShippingPolicy : IShippingPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonShipmentShippingPolicy"/> class.
        /// </summary>
        public AmazonShipmentShippingPolicy()
        {
            AllAmazonOrdersAllowed = false;
            OnlyAmazonPrimeOrdersAllowed = false;
        }

        /// <summary>
        /// Returns true of only Amazon Prime orders are allowed to use Amazon as a carrier.
        /// </summary>
        public bool OnlyAmazonPrimeOrdersAllowed { get; private set; }

        /// <summary>
        /// Returns true if all Amazon orders are allowed to use Amazon as a carrier.
        /// </summary>
        public bool AllAmazonOrdersAllowed { get; private set; }

        /// <summary>
        /// Uses the configuration data provided to configure the shipping policy.
        /// </summary>
        public virtual void Configure(string configuration)
        {
            if (AmazonPrimeShippingRestrictionType.TryParse(configuration, out AmazonPrimeShippingRestrictionType amazonPrimeShippingRestrictionType))
            {
                OnlyAmazonPrimeOrdersAllowed = amazonPrimeShippingRestrictionType.HasFlag(AmazonPrimeShippingRestrictionType.OnlyPrime);
                AllAmazonOrdersAllowed = amazonPrimeShippingRestrictionType.HasFlag(AmazonPrimeShippingRestrictionType.AllOrders);
            }
            else
            {
                throw new ArgumentException("configuration is not a valid AmazonPrimeShippingRestrictionType.");
            }
        }

        /// <summary>
        /// Determines whether the specified target is applicable to the shipping policy.
        /// </summary>
        /// <param name="target">The target is expected to be a AmazonPrimeShippingPolicyTarget.</param>
        /// <returns><c>true</c> if the specified target is applicable; otherwise, <c>false</c>.</returns>
        public virtual bool IsApplicable(object target)
        {
            return target is AmazonPrimeShippingPolicyTarget;
        }

        /// <summary>
        /// Applies the policy to the specified target. This will set the target Allowed property based on
        /// the configuration of the policy.
        /// </summary>
        public virtual void Apply(object target)
        {
            AmazonPrimeShippingPolicyTarget theTarget = target as AmazonPrimeShippingPolicyTarget;
            MethodConditions.EnsureArgumentIsNotNull(theTarget, nameof(theTarget));

            if (IsApplicable(target))
            {
                if (theTarget.Shipment == null || theTarget.AmazonOrder == null)
                {
                    theTarget.Allowed = false;
                    return;
                }
                
                if (AllAmazonOrdersAllowed || (OnlyAmazonPrimeOrdersAllowed && theTarget.AmazonOrder.IsPrime))
                {
                    theTarget.Allowed = theTarget.AmazonCredentials != null;
                    return;
                }
            }
            else
            {
                theTarget.Allowed = false;
            }
        }
    }
}
