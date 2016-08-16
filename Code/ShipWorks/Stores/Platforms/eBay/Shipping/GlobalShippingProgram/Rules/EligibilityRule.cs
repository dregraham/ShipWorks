using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules
{
    /// <summary>
    /// An implementation of the Global Shipping Program interface that inspects the IsEligibleForGlobalShippingProgram
    /// property of an eBay order.
    /// </summary>
    public class EligibilityRule : IGlobalShippingProgramRule
    {
        /// <summary>
        /// Evaluates the specified order against a rule of the Global Shipping Program.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>A Boolean value indicating whether the rule passed.</returns>
        public bool Evaluate(IEbayOrderEntity ebayOrder)
        {
            bool isEligible = false;

            if (ebayOrder != null)
            {
                isEligible = ebayOrder.GspEligible;
            }

            return isEligible;
        }
    }
}
