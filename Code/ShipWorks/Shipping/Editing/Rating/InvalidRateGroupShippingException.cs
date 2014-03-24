using System;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Shipping exception that contains a RateGroup with the error pre-populated
    /// </summary>
    public class InvalidRateGroupShippingException : ShippingException
    {
        public RateGroup InvalidRates { get; private set; }

        public InvalidRateGroupShippingException(RateGroup invalidRates)
        {
            InvalidRates = invalidRates;
        }

        public InvalidRateGroupShippingException(RateGroup invalidRates, string message)
            : base(message)
        {
            InvalidRates = invalidRates;
        }

        public InvalidRateGroupShippingException(RateGroup invalidRates, string message, Exception inner)
            : base(message, inner)
        {
            InvalidRates = invalidRates;
        }
    }
}
