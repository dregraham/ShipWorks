using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Details for phase 1 of authenticating a user to NetworkSolutions
    /// </summary>
    public class NetworkSolutionsUserKey
    {
        public string UserKey { get; set; }
        public string LoginUrl { get; set; }
        public string FailureUrl { get; set; }
    }
}
