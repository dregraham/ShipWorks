using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    public class AmazonMwsThrottleWaitCancelException : AmazonException
    {
        public AmazonMwsThrottleWaitCancelException() : 
            base("Waiting for Amazon to stop throttling was canceled.", null)
        {

        }
    }
}
