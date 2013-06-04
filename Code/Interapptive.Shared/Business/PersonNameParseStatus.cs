using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Denotes how a person name was handled and saved to the 
    /// database
    /// </summary>
    public enum PersonNameParseStatus
    {
        Unknown = 0,

        // First/Middle/Last were found 
        Simple = 1,

        // too complex, unable to parse
        Unparsed = 2,

        // the name was a company name and therefore not parsed 
        CompanyFound = 3,

        // the name included a prefix that needs to be preserved
        PrefixFound = 4
    }
}
