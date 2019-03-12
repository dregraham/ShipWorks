﻿using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs
{
    /// <summary>
    /// Amazon rate info
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class AmazonRateTag
    {
        /// <summary>
        /// The shipping service id of the rate
        /// </summary>
        public string ShippingServiceId { get; set; }

        /// <summary>
        /// The description of the rate
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The carrier name of the rate
        /// </summary>
        public string CarrierName { get; set; }

        /// <summary>
        /// ServiceTypeID - the pnumber of the rate stored in the database
        /// </summary>
        public int ServiceTypeID { get; set; }
    }
}
