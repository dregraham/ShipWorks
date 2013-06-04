using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Defines the basic insurance types.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum InsuranceProvider
    {
        /// <summary>
        /// Invalid (This enum used to be InsuranceType, and 0 was None.  But now insured\notinsured is a bitfield, but this remains to maintain the values for each enum)
        /// </summary>
        [Description("None")]
        Invalid = 0,

        /// <summary>
        /// ShipWorks Insurance
        /// </summary>
        [Description("ShipWorks")]
        ShipWorks = 1,

        /// <summary>
        /// This would be like UPS or FedEx declared value, or Endicia insurance, or whatever the insurance offering is
        /// directly provided by the shipping carrier whose API was used.
        /// </summary>
        [Description("Carrier")]
        Carrier = 2
    }
}
