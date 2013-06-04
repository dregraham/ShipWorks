using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Hashing algorithms
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum SigningAlgorithm
    {
        SHA1,
        SHA256,
        SHA512
    }
}
