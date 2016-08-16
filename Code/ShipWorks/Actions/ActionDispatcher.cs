using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Users;

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

            // Ensure the action processor is working
            ActionProcessor.StartProcessing();
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

            // Ensure the action processor is working
            ActionProcessor.StartProcessing();
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

            // Ensure the action processor is working
            ActionProcessor.StartProcessing();
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

            // Ensure the action processor is working
            ActionProcessor.StartProcessing();
        }

        /// <summary>
        /// Creates an ActionQueue entry for a scheduled Action whose time has come.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void DispatchScheduledAction(long actionID)
        {
            ActionManager.CheckForChangesNeeded();
            ActionEntity actionEntity = ActionManager.GetAction(actionID);

            if (actionEntity == null || !actionEntity.Enabled)
            {
                // Possible race condition where the action could have been deleted
                // between dispatching the action and getting the entity from the ActionManager
                return;
            }

            // Only dispatch scheduled actions
            if ((ActionTriggerType) actionEntity.TriggerType == ActionTriggerType.Scheduled)
            {
                // Scheduled actions should not be running in any sql adapter context, so create one now
                using (SqlAdapter adapter = new SqlAdapter(false))
                {
                    DispatchAction(actionEntity, null, adapter);
                }
            }

            // Ensure the action processor is working
            ActionProcessor.StartProcessing();
        }

        /// <summary>
        /// Creates an ActionQueue entry for the given UserInitiated action
        /// </summary>
        public static void DispatchUserInitiated(long actionID, IEnumerable<long> orderedSelection)
        {
            ActionEntity action = ActionManager.GetAction(actionID);

            if (action == null || !action.Enabled)
            {
                return;
            }

            UserInitiatedTrigger trigger = (UserInitiatedTrigger) ActionManager.LoadTrigger(action);

            // We are potentially going to be saving selection along with dispatching the action, so do it in a transaction
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // First we need the QueueID that all the children will be saved under
                long actionQueueID = DispatchAction(action, null, adapter);

                // If there is a selection requirement, save the selection
                if (trigger.SelectionRequirement != UserInitiatedSelectionRequirement.None)
                {
                    // Save in chunks for efficiency
                    ActionQueueSelectionCollection chunk = new ActionQueueSelectionCollection();

                    foreach (long id in orderedSelection)
                    {
                        if (action.StoreLimited)
                        {
                            StoreEntity store = StoreManager.GetRelatedStore(id);
                            if (store == null || !action.StoreLimitedList.Contains(store.StoreID))
                            {
                                continue;
                            }
                        }

                        ActionQueueSelectionEntity selected = new ActionQueueSelectionEntity();
                        selected.ActionQueueID = actionQueueID;
                        selected.ObjectID = id;
                        chunk.Add(selected);

                        // Save up to 500 at a time
                        if (chunk.Count >= 500)
                        {
                            adapter.SaveEntityCollection(chunk);

                            chunk = new ActionQueueSelectionCollection();
                        }
                    }

                    // May be an unsaved chunk left
                    if (chunk.Count > 0)
                    {
                        adapter.SaveEntityCollection(chunk);
                    }
                }

                adapter.Commit();
            }

            // Ensure the action processor is working
            ActionProcessor.StartProcessing();
        }

        /// <summary>
        /// A valid trigger has been met and the given action is ready to be dispatched
        /// </summary>
        private static long DispatchAction(ActionEntity action, long? objectID, SqlAdapter adapter)
        {
            log.DebugFormat("Dispatching action '{0}' for {1}", action.Name, objectID);

            ActionQueueEntity entity = new ActionQueueEntity();
            entity.ActionID = action.ActionID;
            entity.ActionQueueType = action.TriggerType == (int) ActionTriggerType.Scheduled ? (int) ActionQueueType.Scheduled : (int) ActionQueueType.UserInterface;
            entity.ActionName = action.Name;
            entity.ActionVersion = action.RowVersion;
            entity.ObjectID = objectID;
            entity.TriggerComputerID = UserSession.Computer.ComputerID;

            if (action.ComputerLimitedType == (int) ComputerLimitedType.TriggeringComputer)
            {
                // It's limited to only running on this computer, so use this computer ID as
                // the only computer that can execute the action
                entity.InternalComputerLimitedList = UserSession.Computer.ComputerID.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                // Just copy over the list of computers that are able to execute the action
                entity.InternalComputerLimitedList = action.InternalComputerLimitedList;
            }

            // Set the initial status and the first step
            entity.Status = (int) ActionQueueStatus.Dispatched;
            entity.NextStep = 0;

            adapter.SaveAndRefetch(entity);

            return entity.ActionQueueID;
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
