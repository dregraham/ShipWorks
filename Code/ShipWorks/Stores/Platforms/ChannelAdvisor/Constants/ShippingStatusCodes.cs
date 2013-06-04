using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Constants
{
    /// <summary>
    /// A class of constants for shipping with version 6 of the Channel 
    /// Advisor API - the enumerations that were available in previous
    /// versions are no longer available as these values are now passed
    /// along as string values. The intent of this class is to avoid 
    /// having "magic" string values littered throughout the Channel Advisor
    /// codebase.
    /// </summary>
    public static class ShippingStatusCodes
    {
        public const string NoChange = "NoChange";
        public const string Unshipped = "Unshipped";
        public const string PendingShipment = "PendingShipment";
        public const string PartiallyShipped = "PartiallyShipped";
        public const string Shipped = "Shipped";
    }
}
