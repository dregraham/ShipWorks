using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Actions;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Actions.Triggers;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Utility class for setting up the core shipping action functionality.
    /// </summary>
    public static class ShippingActionUtility
    {
        /// <summary>
        /// Get the print action for the given shipment code
        /// </summary>
        public static ActionEntity GetPrintAction(ShipmentTypeCode code)
        {
            string identifier = GetIdentifier(code, "Print");

            // By default auto-printing is enabled
            bool enabled = (code != ShipmentTypeCode.UpsWorldShip);
            
            return CreateAction("Print labels", CreateProcessedTrigger(code), typeof(PrintShipmentsTask), code, identifier, enabled, true, null);
        }

        /// <summary>
        /// Get the email action for the given shipment code
        /// </summary>
        public static ActionEntity GetEmailAction(ShipmentTypeCode code)
        {
            string identifier = GetIdentifier(code, "Email");

            return CreateAction("Send email", CreateProcessedTrigger(code), typeof(EmailTask), code, identifier, false, true, t =>
                {
                    EmailTask task = (EmailTask) t;

                    task.DelayDelivery = false;
                    task.DelayType = EmailDelayType.ShipDate;
                    task.DelayTimeOfDay = new TimeSpan(0, 16, 0, 0, 0);
                });
        }

        /// <summary>
        /// Get the ship status action for the given shipment code
        /// </summary>
        public static ActionEntity GetShipStatusAction(ShipmentTypeCode code)
        {
            string identifier = GetIdentifier(code, "StatusShipped");

            return CreateAction("Set shipped status", CreateProcessedTrigger(code), typeof(SetOrderStatusTask), code, identifier, true, false, t =>
            {
                SetOrderStatusTask task = (SetOrderStatusTask) t;

                task.Status = "Shipped";
            });
        }

        /// <summary>
        /// Get the void action for the given shipment code
        /// </summary>
        public static ActionEntity GetVoidStatusAction(ShipmentTypeCode code)
        {
            string identifier = GetIdentifier(code, "StatusVoided");

            return CreateAction("Set voided status", CreateVoidedTrigger(code), typeof(SetOrderStatusTask), code, identifier, true, false, t =>
                {
                    SetOrderStatusTask task = (SetOrderStatusTask) t;

                    task.Status = "Voided";
                });
        }

        /// <summary>
        /// Create a shipment processed trigger for the given shipment type
        /// </summary>
        private static ActionTrigger CreateProcessedTrigger(ShipmentTypeCode code)
        {
            ShipmentProcessedTrigger trigger = new ShipmentProcessedTrigger();
            trigger.RestrictType = true;
            trigger.ShipmentType = code;
            trigger.RestrictStandardReturn = true;
            trigger.ReturnShipmentsOnly = false;

            return trigger;
        }

        /// <summary>
        /// Create a shipment voided trigger for the given shipemnt type
        /// </summary>
        private static ActionTrigger CreateVoidedTrigger(ShipmentTypeCode code)
        {
            ShipmentVoidedTrigger trigger = new ShipmentVoidedTrigger();
            trigger.RestrictType = true;
            trigger.ShipmentType = code;

            trigger.RestrictStandardReturn = true;
            trigger.ReturnShipmentsOnly = false;

            return trigger;
        }

        /// <summary>
        /// Get the internal identifier to associate with an action
        /// </summary>
        private static string GetIdentifier(ShipmentTypeCode code, string name)
        {
            return string.Format("Ship{0}-{1}", (int) code, name);
        }

        /// <summary>
        /// See if we can load the action with the given identifier directly from the database.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreTooManyParams]
        private static ActionEntity CreateAction(
            string baseName, 
            ActionTrigger trigger,
            Type taskType, 
            ShipmentTypeCode code, 
            string identifier, 
            bool enabled,
            bool computerLimited,
            MethodInvoker<ActionTask> settingsApplier)
        {
            // See if we can get it from the ActionManager
            ActionEntity action = ActionManager.Actions.SingleOrDefault(a => a.InternalOwner == identifier);
            if (action != null)
            {
                return action;
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                ActionCollection actions = ActionCollection.Fetch(adapter, ActionFields.InternalOwner == identifier);
                if (actions.Count == 1)
                {
                    action = actions[0];
                }

                if (actions.Count > 1)
                {
                    throw new InvalidOperationException("How did we get more than one?");
                }

                if (action == null)
                {
                    action = new ActionEntity();
                    action.Name = string.Format("{0} ({1})", baseName, ShipmentTypeManager.GetType(code).ShipmentTypeName);
                    action.Enabled = enabled;
                    action.InternalOwner = identifier;

                    action.ComputerLimitedType = (int) (computerLimited ? ComputerLimitedType.TriggeringComputer : ComputerLimitedType.None);
                    action.InternalComputerLimitedList = string.Empty;

                    action.StoreLimited = false;
                    action.StoreLimitedList = new long[0];

                    action.TriggerType = (int) trigger.TriggerType;
                    action.TriggerSettings = trigger.GetXml();
                    action.TaskSummary = "";

                    adapter.SaveAndRefetch(action);

                    // Get the task binding, used to create the task.
                    ActionTaskDescriptorBinding binding = new ActionTaskDescriptorBinding(ActionTaskManager.GetDescriptor(taskType));

                    // Create the task
                    ActionTask task = binding.CreateInstance();
                    task.Entity.StepIndex = 0;

                    // Allow the callback to apply specific settings to the task.
                    if (settingsApplier != null)
                    {
                        settingsApplier.Invoke(task);
                    }

                    // Save the task
                    task.Save(action, adapter);
                }

                adapter.Commit();
            }

            return action;
        }
    }
}
