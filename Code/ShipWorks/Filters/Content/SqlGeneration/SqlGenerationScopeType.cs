using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// How a child scope is generated
    /// </summary>
    public enum SqlGenerationScopeType
    {
        /// <summary>
        /// All child rows of the parent scope must pass
        /// </summary>
        EveryChild,

        /// <summary>
        /// Any child row of the parent scope must pass
        /// </summary>
        AnyChild,

        /// <summary>
        /// A specific quantity of the children should exist
        /// </summary>
        QuantityOfChild,

        /// <summary>
        /// No child rows of the parent scope can pass
        /// </summary>
        NoChild,

        /// <summary>
        /// A n:1 parent reference
        /// </summary>
        Parent,
    }
}
