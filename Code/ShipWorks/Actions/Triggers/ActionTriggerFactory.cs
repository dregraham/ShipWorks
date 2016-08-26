using System;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// Simple factory class for creating an ActionTrigger instance from an ActionTriggerType
    /// </summary>
    public static class ActionTriggerFactory
    {
        /// <summary>
        /// Create an ActionTrigger instance that corresponds to the given type
        /// </summary>
        public static ActionTrigger CreateTrigger(ActionTriggerType type, string xmlSettings)
        {
            switch (type)
            {
                case ActionTriggerType.OrderDownloaded: return new OrderDownloadedTrigger(xmlSettings);
                case ActionTriggerType.DownloadFinished: return new DownloadFinishedTrigger(xmlSettings);
                case ActionTriggerType.ShipmentProcessed: return new ShipmentProcessedTrigger(xmlSettings);
                case ActionTriggerType.ShipmentVoided: return new ShipmentVoidedTrigger(xmlSettings);
                case ActionTriggerType.FilterContentChanged: return new FilterContentTrigger(xmlSettings);
                case ActionTriggerType.Scheduled: return new ScheduledTrigger(xmlSettings);
                case ActionTriggerType.UserInitiated: return new UserInitiatedTrigger(xmlSettings);
                case ActionTriggerType.None: return new EmptyTrigger();
            }

            throw new InvalidOperationException("Factory does not handle trigger type: " + type);
        }

    }
}
