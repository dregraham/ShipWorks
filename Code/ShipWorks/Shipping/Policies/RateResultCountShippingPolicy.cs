using System;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// An implementation of the IShippingPolicy interface that will alter the behavior of
    /// the rate control to only show the first "n" rates where "n" is defined by the 
    /// configuration data provided to the configure method.
    /// </summary>
    public class RateResultCountShippingPolicy : IShippingPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RateResultCountShippingPolicy"/> class.
        /// </summary>
        public RateResultCountShippingPolicy()
        {
            RateResultQuantity = 1;
        }

        /// <summary>
        /// Gets the quantity of rate results that should be initially displayed in the rate control.
        /// </summary>
        /// <value>The rate result quantity.</value>
        public int RateResultQuantity { get; private set; }

        /// <summary>
        /// Uses the configuration data provided to configure the shipping policy.
        /// </summary>
        /// <param name="configuration">The configuration data. The data is expected to be
        /// an integer value indicating the maximum number of rate results that should appear
        /// in the rate grid by default.</param>
        /// <exception cref="System.ArgumentException">The configuration for the 
        /// RateResultCountShippingPolicy was not in the expected format. An integer 
        /// value is expected.</exception>
        public virtual void Configure(string configuration)
        {
            int numericConfiguration = 0;
            if (int.TryParse(configuration, out numericConfiguration))
            {
                if (numericConfiguration > RateResultQuantity)
                {
                    RateResultQuantity = numericConfiguration;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified target is applicable to the shipping policy.
        /// </summary>
        /// <param name="target">The target is expected to be a rate control.</param>
        /// <returns><c>true</c> if the specified target is applicable; otherwise, <c>false</c>.</returns>
        public virtual bool IsApplicable(object target)
        {
            return target is RateControl;
        }

        /// <summary>
        /// Applies the policy to the specified target. This will configure the rate control
        /// to not show all rates by default and will set the restriction rate count based on
        /// the configuration of the policy.
        /// </summary>
        /// <param name="target">The object the policy will be applied towards. The target is 
        /// expected to be a rate control.</param>
        public virtual void Apply(object target)
        {
            RateControl rateControl = target as RateControl;

            if (rateControl != null)
            {
                rateControl.ShowAllRates = false;
                rateControl.RestrictedRateCount = RateResultQuantity;

                // Tell the control that we only want to show a single rate 
                // when the count is 1 (i.e. don't show the "more" rates link)
                rateControl.ShowSingleRate = RateResultQuantity == 1;
            }
        }
    }
}
