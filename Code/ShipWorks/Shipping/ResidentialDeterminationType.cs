using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Various ways for ShipWorks to determine whether or not to set the residentail\commercial flag on a shipment.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ResidentialDeterminationType
    {
        /// <summary>
        /// Set to commercial if the company is non-blank
        /// </summary>
        [Description("Commercial if company is entered")]
        CommercialIfCompany = 0,

        /// <summary>
        /// Always set to residential
        /// </summary>
        [Description("Residential")]
        Residential = 1,

        /// <summary>
        /// Always set to commercial
        /// </summary>
        [Description("Commercial")]
        Commercial = 2,

        /// <summary>
        /// Use address lookup feature of the carrier to determine residential status.  Not supported
        /// by all carriers.
        /// </summary>
        [Description("FedEx address lookup")]
        FedExAddressLookup = 3,

        /// <summary>
        /// Use the status retrieved from address validation
        /// </summary>
        [Description("From Address Validation")]
        FromAddressValidation = 4
    }
}
