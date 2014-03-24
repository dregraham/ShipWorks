using System;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class BestRateException : ShippingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BestRateException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public BestRateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
