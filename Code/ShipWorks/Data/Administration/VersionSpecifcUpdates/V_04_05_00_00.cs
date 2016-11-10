using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.Administration.VersionSpecifcUpdates
{
    /// <summary>
    /// ShipWorks update that should be applied for a specific version
    /// </summary>
    /// <remarks>
    /// If we were upgrading from this version, Regenerate scheduled actions
    /// to fix issue caused by breaking out assemblies
    /// </remarks>
    public class V_04_05_00_00 : IVersionSpecificUpdate
    {
        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(4, 5, 0, 0);

        /// <summary>
        /// Execute the update
        /// </summary>
        public void Update()
        {
            // Grab all of the actions that are enabled and schedule based
            ActionManager.InitializeForCurrentSession();
            IEnumerable<ActionEntity> actions = ActionManager.Actions.Where(a => a.Enabled && a.TriggerType == (int) ActionTriggerType.Scheduled);
            using (SqlAdapter adapter = new SqlAdapter())
            {
                foreach (ActionEntity action in actions)
                {
                    // Some trigger's state depend on the enabled state of the action
                    ScheduledTrigger scheduledTrigger = ActionManager.LoadTrigger(action) as ScheduledTrigger;

                    if (scheduledTrigger?.Schedule != null)
                    {
                        // Check to see if the action is a One Time action and in the past, if so we disable it
                        if (scheduledTrigger.Schedule.StartDateTimeInUtc < DateTime.UtcNow &&
                            scheduledTrigger.Schedule.ScheduleType == ActionScheduleType.OneTime)
                        {
                            action.Enabled = false;
                        }
                        else
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
