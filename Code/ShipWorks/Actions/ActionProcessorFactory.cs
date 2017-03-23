using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Factory for creating ActionProcessors
    /// </summary>
    [Component]
    public class ActionProcessorFactory : IActionProcessorFactory
    {
        /// <summary>
        /// Create a Standard ActionProcessor
        /// </summary>
        public ActionProcessor CreateStandard()
        {
            ActionQueueGateway gateway = new ActionQueueGatewayStandard();

            return new ActionProcessor(gateway);
        }

        /// <summary>
        /// Create an Error ActionProcessor
        /// </summary>
        public ActionProcessor CreateError(List<long> queueList)
        {
            ActionQueueGateway gateway = new ActionQueueGatewayErrorList(queueList);

            return new ActionProcessor(gateway);
        }
    }
}
