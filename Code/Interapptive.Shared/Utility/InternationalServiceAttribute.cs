using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.Utility
{
    public class InternationalServiceAttribute : Attribute
    {
        public InternationalServiceAttribute(string countryCodeRestriction = "", bool isInternational = true)
        {
            CountryCodeRestriction = countryCodeRestriction;
            IsInternational = isInternational;
        }

        public bool IsInternational { get; set; }
        public string CountryCodeRestriction { get; }
    }
}
