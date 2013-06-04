using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Base for all exceptions thrown by the Stamps.com integration
    /// </summary>
    public class StampsException : Exception
    {
        public StampsException()
        {

        }

        public StampsException(string message)
            : base(message)
        {

        }

        public StampsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public virtual long Code
        {
            get { return 0; }
        }
    }
}
