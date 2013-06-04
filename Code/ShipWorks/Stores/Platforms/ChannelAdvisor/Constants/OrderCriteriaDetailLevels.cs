using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Constants
{
    /// <summary>
    /// A class of constants for detail level of the order criteria with version 6 of the 
    /// Channel Advisor API - the enumerations that were available in previous
    /// versions are no longer available as these values are now passed
    /// along as string values. The intent of this class is to avoid 
    /// having "magic" string values littered throughout the Channel Advisor
    /// codebase.
    /// </summary>
    public static class OrderCriteriaDetailLevels
    {
        public const string Low = "Low";
        public const string Medium = "Medium";
        public const string High = "High";
        public const string Complete = "Complete";
    }
}
