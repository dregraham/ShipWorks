using System;
using System.Reflection;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// ErrorResponse to return when there is an error
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ErrorResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorResponse(string message)
        {
            Message = message.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; }
    }
}
