using log4net;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// The action queue gateway that retrieves all Incomplete\Dispatched queues that are
    /// of ActionQueueType equal to DefaultPrint from the database.  
    /// </summary>
    public class ActionQueueGatewayDefaultPrint : ActionQueueGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActionQueueGatewayDefaultPrint()
        {
            log = LogManager.GetLogger(typeof(ActionQueueGatewayDefaultPrint));
            isProcessableQueueType = ActionQueueFields.ActionQueueType == (int)ActionQueueType.DefaultPrint;
        }

        /// <summary>
        /// This is a DefaultPrint Action Queue Gateway
        /// </summary>
        public override ActionQueueGatewayType GatewayType => ActionQueueGatewayType.DefaultPrint;
    }
}