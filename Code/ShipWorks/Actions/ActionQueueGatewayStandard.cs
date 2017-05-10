using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using LogManager = log4net.LogManager;

namespace ShipWorks.Actions
{
    /// <summary>
    /// The standard action queue gateway that retrieves Incomplete\Dispatched queues from the database.
    /// 
    /// If defaultPrint is true, it will NOT retrieve action queue entries of ActionQueueType equal to DefaultPrint.
    /// If defaultPrint is false, it will retrieve action queue entries including ActionQueueType equal to DefaultPrint.
    /// </summary>
    public class ActionQueueGatewayStandard : ActionQueueGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActionQueueGatewayStandard(IConfigurationData configurationData)
        {
            log = LogManager.GetLogger(typeof(ActionQueueGatewayStandard));

            // If default printing is NOT enabled, then this gateway should process default print tasks.
            if (!configurationData.FetchReadOnly().UseParallelActionQueue)
            {
                isProcessableQueueType = ActionQueueFields.ActionQueueType == (int) ActionQueueType.DefaultPrint;
            }

            // By default only process the queue type that matches this process
            isProcessableQueueType |= ActionQueueFields.ActionQueueType == (int) configurationData.ExecutionModeActionQueueType;

            // If the UI isn't running somehwere, and we are the background process, go ahead and do UI actions too since it's not open
            if (configurationData.IncludeUserInterfaceActions)
            {
                // Additionally process UI actions if the UI is not running
                isProcessableQueueType |= ActionQueueFields.ActionQueueType == (int) ActionQueueType.UserInterface;
            }
        }

        /// <summary>
        /// This is a Standard Action Queue Gateway
        /// </summary>
        public override ActionQueueGatewayType GatewayType => ActionQueueGatewayType.Standard;
    }
}