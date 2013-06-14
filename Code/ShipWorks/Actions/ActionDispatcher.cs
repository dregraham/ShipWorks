using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Communication;
using System.Xml.Linq;
using ShipWorks.Actions.Triggers;
using ShipWorks.Users;
using ShipWorks.Shipping;
using log4net;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Kind of like a police dispatcher.  Not really. Anyway, anytime an action happens somewhere in
    /// ShipWorks it should tell the dispatcher about it, and the rest will be taken care of.
    /// </summary>
    public static class ActionDispatcher
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionDispatcher));

        /// <summary>
        /// Called each time an individual order is downloaded into ShipWorks.
        /// </summary>
        public static void DispatchOrderDownloaded(OrderEntity order, bool initialDownload, SqlAdapter adapter)
        {
            List<ActionEntity> actions = GetEligibleActions(ActionTriggerType.OrderDownloaded, order.StoreID);

            // Now we have to check the trigger-specific properties to see if they match...
            foreach (ActionEntity action in actions)
            {
                OrderDownloadedTrigger trigger = new OrderDownloadedTrigger(action.TriggerSettings);

                if (trigger.Restriction == OrderDownloadedRestriction.OnlyInitial && !initialDownload)
                {
                    continue;
                }

                if (trigger.Restriction == OrderDownloadedRestriction.NotInitial && initialDownload)
                {
                    continue;
                }

                DispatchAction(action, order.OrderID, adapter);
            }
        }

        /// <summary>
        /// Called each time a download for a store completes.
        /// </summary>
        public static void DispatchDownloadFinished(long storeID, DownloadResult result, int? quantityNew)
        {
            List<ActionEntity> actions = GetEligibleActions(ActionTriggerType.DownloadFinished, storeID);

            // Now we have to check the trigger-specific properties to see if they match...
            foreach (ActionEntity action in actions)
            {
                DownloadFinishedTrigger trigger = new DownloadFinishedTrigger(action.TriggerSettings);

                if (trigger.RequiredResult != null && trigger.RequiredResult.Value != result)
                {
                    continue;
                }

                if (trigger.OnlyIfNewOrders && (quantityNew == null || quantityNew.Value == 0))
                {
                    continue;
                }

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    DispatchAction(action, null, adapter);
                }
            }
        }

        /// <summary>
        /// Called each time a shipment has been successfully processed
        /// </summary>
        public static void DispatchShipmentProcessed(ShipmentEntity shipment, SqlAdapter adapter)
        {
            List<ActionEntity> actions = GetEligibleActions(ActionTriggerType.ShipmentProcessed, shipment.Order.StoreID);

            // Now we have to check the trigger-specific properties to see if they match...
            foreach (ActionEntity action in actions)
            {
                ShipmentProcessedTrigger trigger = new ShipmentProcessedTrigger(action.TriggerSettings);

                ShipmentTypeCode shipmentType = (ShipmentTypeCode) shipment.ShipmentType;

                // Endicia\Express1 kludge.  The setting in Endicia that allows auto-changing to Express1 if its a profitable transaction
                // converts the shipment to Express1.  But for ease of configuration, and to match what the user expects, we want the actions 
                // to process as if they are Endicia.  This is where we test for if the Express1 shipment was really originally an Endicia shipment,
                // and force Endicia actions to run.
                if (shipmentType == ShipmentTypeCode.PostalExpress1 && shipment.Postal.Endicia.OriginalEndiciaAccountID != null)
                {
                    shipmentType = ShipmentTypeCode.Endicia;
                }

                // Honor the Shipment Type limitation
                if (trigger.RestrictType && trigger.ShipmentType != shipmentType)
                {
                    continue;
                }

                // honor the Return Shipment Only setting
                if (trigger.RestrictStandardReturn && trigger.ReturnShipmentsOnly != shipment.ReturnShipment)
                {
                    continue;
                }

                DispatchAction(action, shipment.ShipmentID, adapter);
            }
        }

        /// <summary>
        /// Called each time a shipment has been successfully voided
        /// </summary>
        public static void DispatchShipmentVoided(ShipmentEntity shipment, SqlAdapter adapter)
        {
            List<ActionEntity> actions = GetEligibleActions(ActionTriggerType.ShipmentVoided, shipment.Order.StoreID);

            // Now we have to check the trigger-specific properties to see if they match...
            foreach (ActionEntity action in actions)
            {
                ShipmentVoidedTrigger trigger = new ShipmentVoidedTrigger(action.TriggerSettings);

                // honor the Shipment Type restriction
                if (trigger.RestrictType && trigger.ShipmentType != (ShipmentTypeCode) shipment.ShipmentType)
                {
                    continue;
                }

                // honor the Return Shipment restriction
                if (trigger.RestrictStandardReturn && trigger.ReturnShipmentsOnly != shipment.ReturnShipment)
                {
                    continue;
                }

                DispatchAction(action, shipment.ShipmentID, adapter);
            }
        }

        /// <summary>
        /// Creates an ActionQueue entry for a scheduled Action whose time has come.
        /// </summary>
        public static void DispatchScheduledAction(long actionID)
        {
            ActionEntity actionEntity = ActionManager.GetAction(actionID);
            ActionTriggerType actionTriggerType = (ActionTriggerType)actionEntity.TriggerType;

            // Get the specific action trigger type and check any specific settings
            switch (actionTriggerType)
            {
                case ActionTriggerType.OrderDownloaded:
                case ActionTriggerType.DownloadFinished:
                case ActionTriggerType.ShipmentProcessed:
                case ActionTriggerType.ShipmentVoided:
                case ActionTriggerType.FilterContentChanged:
                    log.WarnFormat("Scheduled Actions do not support trigger type: {0}", EnumHelper.GetDescription(actionTriggerType));
                    return;
                case ActionTriggerType.Cron:
                    // TODO: Add any cron trigger checks...
                    // CronTrigger cronTrigger = new CronTrigger(actionEntity.TriggerSettings);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Scheduled actions should not be running in any sql adapter context, so create one now
            using (SqlAdapter adapter = new SqlAdapter(false))
            {
                DispatchAction(actionEntity, null, adapter);
            }
        }

        /// <summary>
        /// A valid trigger has been met and the given action is ready to be dispatched
        /// </summary>
        private static void DispatchAction(ActionEntity action, long? objectID, SqlAdapter adapter)
        {
            log.DebugFormat("Dispatching action '{0}' for {1}", action.Name, objectID);

            ActionQueueEntity entity = new ActionQueueEntity();
            entity.ActionID = action.ActionID;
            entity.ActionQueueType = (int)ActionManager.ActionQueueType;
            entity.ActionName = action.Name;
            entity.ActionVersion = action.RowVersion;
            entity.ObjectID = objectID;
            entity.TriggerComputerID = UserSession.Computer.ComputerID;

            // If it's limited to only running on this computer set the ID
            if (action.ComputerLimited)
            {
                entity.RunComputerID = UserSession.Computer.ComputerID;
            }

            // Set the initial status and the first step
            entity.Status = (int) ActionQueueStatus.Dispatched;
            entity.NextStep = 0;

            adapter.SaveEntity(entity);

            // Ensure the action processor is working
            ActionProcessor.StartProcessing();
        }

        /// <summary>
        /// Get all the actions that match have the given trigger, are enabled, and can be used by the specified store.
        /// </summary>
        private static List<ActionEntity> GetEligibleActions(ActionTriggerType trigger, long storeID)
        {
            List<ActionEntity> results = new List<ActionEntity>();

            foreach (ActionEntity action in ActionManager.Actions)
            {
                // Has to be enabled
                if (!action.Enabled)
                {
                    continue;
                }

                // Has to match the trigger
                if (action.TriggerType != (int) trigger)
                {
                    continue;
                }

                // Has to be OK for the given store
                if (action.StoreLimited && !action.StoreLimitedList.Contains(storeID))
                {
                    continue;
                }

                results.Add(action);
            }

            // Order them such that any with an "InternalOwner" go first.  So any that are builtin to ShipWorks and not user created get priority
            // to run first, such as our builtin shipping printing rules, which is why this was created.
            var prioritizedResults = results
                .Where(a => !string.IsNullOrWhiteSpace(a.InternalOwner))
                .Concat(
                results.Where(a => string.IsNullOrWhiteSpace(a.InternalOwner))).ToList();

            // Ensure we didn't drop\duplicate any
            if (prioritizedResults.Count != results.Count && prioritizedResults.Intersect(results).Count() != results.Count)
            {
                throw new InvalidOperationException("Somehow we changed the list while prioritizing.");
            }

            return prioritizedResults;
        }
    }
}
