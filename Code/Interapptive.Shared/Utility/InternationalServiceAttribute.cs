using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute indicating if a service enum is valid for international shipments
    /// </summary>
    public class InternationalServiceAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InternationalServiceAttribute(string countryCodeRestriction = "", bool isInternational = true)
        {
            CountryCodeRestriction = countryCodeRestriction;
            IsInternational = isInternational;
        }

        /// <summary>
        /// The service is internatonal
        /// </summary>
        public bool IsInternational { get; set; }

        /// <summary>
        /// The country the service is restricted to
        /// </summary>
        public string CountryCodeRestriction { get; }
    }
}
