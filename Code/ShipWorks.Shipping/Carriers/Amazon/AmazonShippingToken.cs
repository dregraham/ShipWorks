using System;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Object used to store, encrypt and decrypt an Amazon shipping token
    /// </summary>
    public class AmazonShippingToken
    {
        /// <summary>
        /// Date of the error
        /// </summary>
        public DateTime ErrorDate { get; set; }

        /// <summary>
        /// Reason for the error
        /// </summary>
        public string ErrorReason { get; set; }
    }
}
