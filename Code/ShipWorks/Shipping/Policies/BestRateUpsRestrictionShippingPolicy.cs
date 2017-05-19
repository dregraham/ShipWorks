using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// If Tango says we can't use UPS for BestRate, filter out ups api rating
    /// </summary>
    public class BestRateUpsRestrictionShippingPolicy : IShippingPolicy
    {
        private bool isRestricted;

        /// <summary>
        /// Uses the configuration data provided to configure the shipping policy.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <exception cref="System.ArgumentException">configuration</exception>
        public void Configure(string configuration)
        {
            bool parsedConfiguration;

            if (bool.TryParse(configuration, out parsedConfiguration))
            {
                if (parsedConfiguration)
                {
                    isRestricted = true;
                }
            }
            else
            {
                throw new ArgumentException($@"Unknown configuration value '{configuration}.' Expected 'true' or 'false.'", "configuration");
            }
        }

        /// <summary>
        /// We can act on a list of UpsRatingMethod
        /// </summary>
        public bool IsApplicable(object target)
        {
            return isRestricted && target is List<UpsRatingMethod>;
        }

        /// <summary>
        /// Filter out
        /// </summary>
        public void Apply(object target)
        {
            if (!isRestricted)
            {
                return;
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (!IsApplicable(target))
            {
                throw new ArgumentException(@"target not of type ListList<UpsRatingMethod>", "target");
            }

            List<UpsRatingMethod> availableRatingMethods = target as List<UpsRatingMethod>;

            if (availableRatingMethods != null)
            {
                if (isRestricted)
                {
                    availableRatingMethods.RemoveAll(r => r != UpsRatingMethod.LocalOnly);
                }
                else
                {
                    availableRatingMethods.RemoveAll(r => r != UpsRatingMethod.LocalWithApiFailover);
                }
            }
        }
    }
}