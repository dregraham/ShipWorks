using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// An interface intended for populating a list of token suggestions based on a particular usage.
    /// </summary>
    public interface ITokenSuggestionFactory
    {
        /// <summary>
        /// Gets an array of token suggestions for a particular usage.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <returns>An array of TokenSuggestion objects.</returns>
        TokenSuggestion[] GetSuggestions(TokenUsage usage);

        ///// <summary>
        ///// Gets an array of token suggestions for a particular usage where the suggestions can 
        ///// be tailored specifically to the orders that were provided.
        ///// </summary>
        ///// <param name="usage">The usage.</param>
        ///// <param name="orders">The orders.</param>
        ///// <returns></returns>
        //TokenSuggestion[] GetSuggestions(TokenUsage usage, List<OrderEntity> orders);
    }
}
