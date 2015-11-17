using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;

namespace ShipWorks.AddressValidation.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AddressType
    {
        [Description("")]
        [Details("")]
        NotChecked = 0,

        [Description("International")]
        [Details("International")]
        [ImageResource("earth16")]
        WillNotValidate = 1,

        [Description("Invalid")]
        [Details("The address matching system was unable to find an exact match for the city, state, and ZIP Code.\r\nWe suggest correcting this before creating your label")]
        [ImageResource("error16")]
        Invalid = 2,

        [Description("Ambigous")]
        [Details("The address matching system was unable to find an exact match for an Apartment or Suite Number.\r\nOnly the street address, city, state, and ZIP Code fields have been validated.")]
        [ImageResource("warning16")]
        SecondaryNotFound = 3,

        [Description("Ambigous")]
        [Details("The address matching system was unable to find an exact match for the street address. \r\nOnly the city, state, and ZIP Code fields have been validated.")]
        [ImageResource("warning16")]
        PrimaryNotFound = 4,

        [Description("Military")]
        [Details("Military")]
        [ImageResource("shield")]
        Military = 5,

        [Description("US Territory")]
        [Details("US Territory")]
        [ImageResource("island")]
        UsTerritory = 6,

        [Description("PO Box")]
        [Details("PO Box")]
        [ShipmentTypeIcon("usps")]
        PoBox = 7,

        [Description("Residential")]
        [Details("Residential")]
        [ImageResource("house")]
        Residential = 8,

        [Description("Commercial")]
        [Details("Commercial")]
        [ImageResource("building")]
        Commercial = 9,

        [Description("Valid")]
        [Details("Address is correct but Residential/Commercial Status is unknown.")]
        [ImageResource("check16")]
        Valid = 10, 

        [Description("Error")]
        [Details("Error communicating with Address Validation Server.")]
        Error = 11
    }
}
