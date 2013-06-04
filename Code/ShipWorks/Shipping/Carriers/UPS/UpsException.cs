using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Base for all handlable ups exceptions
    /// </summary>
    public class UpsException : CarrierException
    {
        public UpsException()
        {

        }

        public UpsException(string message)
            : base(message)
        {

        }

        public UpsException(string message, Exception innerEx)
            : base(message, innerEx)
        {

        }

        public virtual string ErrorCode
        {
            get { return "0"; }
        }
    }
}
