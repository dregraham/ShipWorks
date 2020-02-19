using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// ErrorResponse to return when there is an error
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorResponse(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; }
    }
}
