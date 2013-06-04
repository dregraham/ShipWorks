using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Simple container for error code/message pairs returned from PayPal
    /// </summary>
    [Serializable]
    public class PayPalErrorItem
    {
        public string Code { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalErrorItem(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
