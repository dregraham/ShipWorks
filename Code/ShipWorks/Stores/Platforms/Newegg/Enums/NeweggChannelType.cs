using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

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
        [Description("Unavailable")]
        Marketplace = 0,

        /// <summary>
        /// NewEgg for Business
        /// </summary>
        [Description("Unavailable")]
        Business = 1  
    }
}
