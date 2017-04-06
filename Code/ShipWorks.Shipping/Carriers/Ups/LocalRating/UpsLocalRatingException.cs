using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// An exception for Ups Local Rating related errors
    /// </summary>
    public class UpsLocalRatingException : Exception
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
