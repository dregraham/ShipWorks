using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Base for all exceptions thrown by the FedEx integration
    /// </summary>
    public class FedExException : Exception
    {
        public FedExException()
        {

        }

        public FedExException(string message)
            : base(message)
        {

        }

        public FedExException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public virtual long Code
        {
            get { return 0; }
        }
    }
}
