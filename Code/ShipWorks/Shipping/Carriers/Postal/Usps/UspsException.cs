using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Base for all exceptions thrown by the USPS integration
    /// </summary>
    public class UspsException : Exception
    {
        public UspsException()
        {

        }

        public UspsException(string message)
            : base(message)
        {

        }

        public UspsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public virtual long Code
        {
            get { return 0; }
        }
    }
}
