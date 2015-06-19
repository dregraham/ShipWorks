using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Newegg.Enums
{
    /// <summary>
    /// An enumeration for use in the Newegg API request criteria that will determine what API to connect to
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum NeweggChannelType
    {
        /// <summary>
        /// NewEgg Markplace
        /// </summary>
        [Description("Newegg US Marketplace")]
        US = 0,

        /// <summary>
        /// NewEgg for Business
        /// </summary>
        [Description("Newegg Business Marketplace")]
        Business = 1,

        /// <summary>
        /// NewEgg for Canada
        /// Not Yet Supported U
        /// </summary>
        [Description("Newegg Canada Marketplace")]
        Canada = 2
    }
}
