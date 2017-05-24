using System;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating
{
    /// <summary>
    /// An exception for Ups Local Rating related errors
    /// </summary>
    public class UpsLocalRatingException : UpsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingException" /> class.
        /// </summary>
        public UpsLocalRatingException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UpsLocalRatingException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpsLocalRatingException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
