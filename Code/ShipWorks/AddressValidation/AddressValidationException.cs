using System;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Thrown when a known error occurs during address validation
    /// </summary>
    [Serializable]
    public class AddressValidationException : Exception
    {
        /// <summary>
        /// Creates a new AddressValidationException
        /// </summary>
        public AddressValidationException()
        {

        }

        /// <summary>
        /// Creates a new AddressValidationException with the specified message
        /// </summary>
        public AddressValidationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new AddressValidationException with the specified message an inner exception
        /// </summary>
        public AddressValidationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
