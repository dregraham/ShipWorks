using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules
{
    /// <summary>
    /// An interface for implementing rules for the eBay Global Shipping Program
    /// </summary>
    public interface IGlobalShippingProgramRule
    {
        /// <summary>
        /// Evaluates the specified order against a rule of the Global Shipping Program.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>A Boolean value indicating whether the rule passed.</returns>
        bool Evaluate(EbayOrderEntity ebayOrder);
    }
}
