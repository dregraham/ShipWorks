using System;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response
{
    /// <summary>
    /// An general exception thrown when inspecting an open account response.
    /// </summary>
    public class UpsOpenAccountResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountResponseException" /> class.
        /// </summary>
        /// <param name="response">The response.</param>
        public UpsOpenAccountResponseException(OpenAccountResponse response)
            : this(response, string.Empty, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountResponseException" /> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="message">The message.</param>
        public UpsOpenAccountResponseException(OpenAccountResponse response, string message)
            : this(response, message, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountResponseException" /> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpsOpenAccountResponseException(OpenAccountResponse response, string message, Exception innerException)
            : base(message, innerException)
        {
            NativeResponse = response;
        }

        /// <summary>
        /// Gets the native UPS response.
        /// </summary>
        /// <value>The response.</value>
        public OpenAccountResponse NativeResponse { get; private set; }
    }
}
