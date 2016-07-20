using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules
{
    /// <summary>
    /// An implementation of the Global Shipping Program interface that inspects the selected shipping method
    /// property of an eBay order.
    /// </summary>
    public class SelectedShippingMethodRule : IGlobalShippingProgramRule
    {
        /// <summary>
        /// Evaluates the specified order against a rule of the Global Shipping Program.
        /// </summary>
        /// <param name="ebayOrder"></param>
        /// <returns>A Boolean value indicating whether the rule passed.</returns>
        public bool Evaluate(IEbayOrderEntity ebayOrder)
        {
            bool isRuleSatisfied = false;

            if (ebayOrder != null)
            {
                // The shipping method of the order needs to be the global shipping program
                isRuleSatisfied = ebayOrder.SelectedShippingMethod == (int) EbayShippingMethod.GlobalShippingProgram;
            }

            return isRuleSatisfied;
        }
    }
}
