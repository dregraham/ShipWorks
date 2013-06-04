using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Failure Severity of a failed SOAP request
    /// </summary>
    public enum NetworkSolutionsFailureSeverity
    {
        Failure = 0,
        PartialFailure = 1,
        Other = 2
    }
}
