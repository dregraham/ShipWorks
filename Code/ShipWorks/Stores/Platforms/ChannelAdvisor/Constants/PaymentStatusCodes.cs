using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Constants
{
    /// <summary>
    /// A class of constants for payment statuses with version 6 of the 
    /// Channel Advisor API - the enumerations that were available in previous
    /// versions are no longer available as these values are now passed
    /// along as string values. The intent of this class is to avoid 
    /// having "magic" string values littered throughout the Channel Advisor
    /// codebase.
    /// </summary>
    public static class PaymentStatusCodes
    {
        public const string NoChange = "NoChange";
        public const string NotSubmitted = "NotSubmitted";
        public const string Submitted = "Submitted";
        public const string Deposited = "Deposited";
        public const string Cleared = "Cleared";
        public const string Failed = "Failed";
    }
}
