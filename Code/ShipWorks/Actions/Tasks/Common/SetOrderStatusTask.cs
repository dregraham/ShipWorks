using System.Collections.Generic;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Tokens;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for setting the status of an order
    /// </summary>
    [ActionTask("Set local order status", "OrderStatus", ActionTaskCategory.UpdateLocally)]
    public class SetOrderStatusTask : ActionTask
    {
        string status = string.Empty;

        /// <summary>
        /// Create the editor for editing the settings of the task
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new SetOrderStatusTaskEditor(this);
        }

        /// <summary>
        /// How to label the input selection for the task
        /// </summary>
        public override string InputLabel
        {
            get { return "Set status of:"; }
        }

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// Run the task over the given input
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            foreach (long orderID in inputKeys)
            {
                OrderEntity prototype = new OrderEntity(orderID) { IsNew = false };
                prototype.LocalStatus = TemplateTokenProcessor.ProcessTokens(status, orderID);

                context.CommitWork.AddForSave(prototype);
            }
        }

        /// <summary>
        /// The status token that will be evaluated and applied to each order.
        /// </summary>
        public string Status
        {
            get 
            { 
                return status; 
            }
            
            set 
            {
                // This is to handle a pre Alpha6 bug where we serialized null status values
                status = value ?? string.Empty; 
            }
        }
    }
}
