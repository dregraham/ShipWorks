using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// An implementation of the IShippingPolicy interface that will alter the behavior of
    /// the rate control to show all rates.
    /// </summary>
    public class NonRestrictedRateCountShippingPolicy : RateResultCountShippingPolicy, IShippingPolicy
    {
        /// <summary>
        /// Uses the configuration data provided to configure the shipping policy.
        /// </summary>
        /// <param name="configuration">The configuration data. The data is expected to be
        /// an integer value indicating the maximum number of rate results that should appear
        /// in the rate grid by default.</param>
        public override void Configure(string configuration)
        { }


        /// <summary>
        /// Applies the policy to the specified target. This will configure the rate control
        /// to show all rates.
        /// </summary>
        /// <param name="target">The object the policy will be applied towards. The target is
        /// expected to be a rate control.</param>
        public override void Apply(object target)
        {
            RateControl rateControl = target as RateControl;

            if (rateControl != null)
            {
                rateControl.ShowAllRates = true;
                rateControl.ShowSingleRate = false;
            }
        }
    }
}
