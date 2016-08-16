using System;
using System.Collections.Generic;
using System.Linq;
namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute that may be applied to Enum Fields
    /// Really this allows for a second description field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public sealed class DetailsAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DetailsAttribute(string details)
        {
            Details = details;
        }

        /// <summary>
        /// The value as required by the specific API being used
        /// </summary>
        public string Details { get; }
    }
}
