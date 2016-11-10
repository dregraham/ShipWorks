using System;
using ShipWorks.Actions;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// ShipWorks update that should be applied for a specific version
    /// </summary>
    /// <remarks>
    /// If we were upgrading from this version, Regenerate scheduled actions
    /// to fix issue caused by breaking out assemblies
    /// </remarks>
    public class V_05_04_00_00 : IVersionSpecificUpdate
    {
        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(5, 4, 0, 0);

        /// <summary>
        /// Execute the update
        /// </summary>
        public void Update()
        {
            // Grab all of the actions that are enabled and schedule based
            ActionManager.InitializeForCurrentSession();
            using (SqlAdapter adapter = new SqlAdapter())
            {
                foreach (ActionEntity action in ActionManager.Actions)
                {
                    // Some trigger's state depend on the enabled state of the action
                    ScheduledTrigger scheduledTrigger = ActionManager.LoadTrigger(action) as ScheduledTrigger;

                    if (scheduledTrigger?.Schedule != null)
                    {
                        // Check to see if the action is a One Time action and in the past, if so we disable it
                        if (scheduledTrigger.Schedule.StartDateTimeInUtc >= DateTime.UtcNow ||
                            scheduledTrigger.Schedule.ScheduleType != ActionScheduleType.OneTime)
                        {
                            scheduledTrigger.SaveExtraState(action, adapter);
                        }
                    }

                    ActionManager.SaveAction(action, adapter);
                }

                adapter.Commit();
            }
        }
    }
}
