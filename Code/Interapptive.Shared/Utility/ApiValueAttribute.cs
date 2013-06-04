using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute that may be applied to Enum Fields
    /// Allows setting ApiText, which can be the value that needs to be passed using an api call
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field,
                           AllowMultiple = false)  // do not allow multiuse, there should be only 1 allowed text value
    ]
    public sealed class ApiValueAttribute : Attribute
    {
        private string apiValue = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiValueAttribute(string apiValue)
        {
            this.apiValue = apiValue;
        }

        /// <summary>
        /// The value as required by the specific API being used
        /// </summary>
        public string ApiValue
        {
            get
            {
                return apiValue;
            }
        }
    }
}
