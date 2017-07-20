using System.Collections;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Factory for creating ActionProcessors
    /// </summary>
    [Component]
    public class ActionProcessorFactory : IActionProcessorFactory
    {
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionProcessorFactory(IConfigurationData configurationData)
        {
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Create a Standard ActionProcessor
        /// </summary>
        public IEnumerable<ActionProcessor> CreateStandard()
        {
            List<ActionProcessor> actionProcessors = new List<ActionProcessor>();
            
            bool defaultPrint = configurationData.FetchReadOnly().UseParallelActionQueue;

            // If default print is enabled, add the default print gateway. ONLY if we are in the UI
            // We dont want the background process printing because it will print out of order with both 
            // the UI and background process sending documents to the printer at the same time
            if (defaultPrint && configurationData.ExecutionModeActionQueueType == ActionQueueType.UserInterface)
            {
                actionProcessors.Add(new ActionProcessor(new ActionQueueGatewayDefaultPrint()));
            }

            // Always add the standard gateway
            actionProcessors.Add(new ActionProcessor(new ActionQueueGatewayStandard(configurationData)));

            return actionProcessors;
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
