using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// The reported service status for the MWS APIs
    /// </summary>
    public class AmazonMwsServiceStatus
    {
        // Status color
        public AmazonMwsServiceStatusColor StatusColor { get; set; }

        // optional information message accompanying color
        public String Message { get; set; }
    }
}
