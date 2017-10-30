using System;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    /// <summary>
    /// Options for a FedEx rate request
    /// </summary>
    [Flags]
    public enum FedExRateRequestOptions
    {
        /// <summary>
        /// No specific options
        /// </summary>
        None = 0,

        /// <summary>
        /// SmartPost should be included
        /// </summary>
        SmartPost = 1,

        /// <summary>
        /// OneRate should be included
        /// </summary>
        OneRate = 2,

        /// <summary>
        /// Express Freight should be included
        /// </summary>
        ExpressFreight = 4,
    }
}