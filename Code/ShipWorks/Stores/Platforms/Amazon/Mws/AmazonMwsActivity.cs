using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// High-level Amazon operations that are used to declare web client intent so it can prepare
    /// locking/quota handling.
    /// </summary>
    [Flags]
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonMwsActivities
    {
        /// <summary>
        /// No quota handling required
        /// </summary>
        None,

        /// <summary>
        /// Simple credentials testing
        /// </summary>
        TestCredentials,

        /// <summary>
        /// Order downloading
        /// </summary>
        GetOrders,

        /// <summary>
        /// Uploading tracking information
        /// </summary>
        UploadShipment,

        /// <summary>
        /// Order Service health testing
        /// </summary>
        GetOrderServiceStatus,
    }
}
