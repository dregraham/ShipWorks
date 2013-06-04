using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Exceptions related to OnTrac API Errors
    /// </summary>
    public class OnTracApiErrorException : OnTracException
    {
        static Dictionary<string, string> errorReplacements = new Dictionary<string, string>();

        // Static constructor
        static OnTracApiErrorException()
        {
            errorReplacements.Add("Invalid Username", "The specified account number and password are not correct.");
            errorReplacements.Add("Delivery Zip Not Serviced", "OnTrac does not service the delivery address.");
            errorReplacements.Add("Invalid Shipper State", "OnTrac does not service the delivery address.");
            errorReplacements.Add("Pickup Zip Not Serviced", "OnTrac does not service the pickup address.");
        }

        public OnTracApiErrorException()
        {

        }

        public OnTracApiErrorException(string apiError)
            : base(apiError)
        {

        }

        public OnTracApiErrorException(string apiError, Exception inner)
            : base(apiError, inner)
        {

        }

        /// <summary>
        /// Override the outgoing message to provide customized "English" display to users.
        /// </summary>
        public override string Message
        {
            get
            {
                foreach (var pair in errorReplacements)
                {
                    if (base.Message.Contains(pair.Key))
                    {
                        return pair.Value;
                    }
                }

                return string.Format("OnTrac returned the following error:\n\n{0}", base.Message);
            }
        }
    }
}
