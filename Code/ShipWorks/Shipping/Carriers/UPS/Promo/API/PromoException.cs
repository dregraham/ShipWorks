using System;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// An general exception for UPS Promo
    /// </summary>
    public class UpsPromoException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPromoException" /> class.
        /// </summary>
        public UpsPromoException(string message) 
            : base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPromoException" /> class.
        /// </summary>
        public UpsPromoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
