using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Constants
{
    /// <summary>
    /// A class of constants for checkout statuses with version 6 of the 
    /// Channel Advisor API - the enumerations that were available in previous
    /// versions are no longer available as these values are now passed
    /// along as string values. The intent of this class is to avoid 
    /// having "magic" string values littered throughout the Channel Advisor
    /// codebase.
    /// </summary>
    public static class CheckoutStatusCodes
    {
        public const string NoChange = "NoChange";
        public const string NotVisited = "NotVisited";
        public const string Visited = "Visited";
        public const string OnHold = "OnHold";
        public const string Completed = "Completed";
        public const string CompletedOffline = "CompletedOffline";
        public const string Cancelled = "Cancelled";
    }
}
