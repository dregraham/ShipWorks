using System;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Actions.Triggers
{
    public class ScheduledTrigger : ActionTrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTrigger"/> class.
        /// </summary>
        public ScheduledTrigger()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTrigger"/> class.
        /// </summary>
        /// <param name="xmlSettings"></param>
        public ScheduledTrigger(string xmlSettings)
            : base(xmlSettings)
        {
            //TODO: move initialization once ActionSchedule work is done
            if (StartDateTimeInUtc.Year == 1)
            {
                // Initialize the start date to the top of the next hour since it wasn't included in the settings
                StartDateTimeInUtc = DateTime.UtcNow.AddMinutes(60 - DateTime.UtcNow.Minute);
            }
            else if (StartDateTimeInUtc.Kind == DateTimeKind.Unspecified)
            {
                StartDateTimeInUtc = DateTime.SpecifyKind(StartDateTimeInUtc, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Overridden to provide the trigger type
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.Scheduled; }
        }

        /// <summary>
        /// Creates the editor that is used to edit the condition.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ActionTriggerEditor CreateEditor()
        {
            return new ScheduledTriggerEditor(this);
        }

        /// <summary>
        /// The type of entity that causes the trigger to fire
        /// </summary>
        public override EntityType? TriggeringEntityType
        {
            // This doesn't map to any ShipWorks database entity at this time
            get { return null; }
        }


        /// <summary>
        /// Gets or sets the schedule for this trigger.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public ActionSchedule Schedule
        {
            get { return schedule ?? (schedule = new TempActionScheduleBridge()); }
            set { schedule = value; }
        }

        ActionSchedule schedule;

        /// <summary>
        /// TODO: remove this and TempActionScheduleBridge when ActionSchedule work is done
        /// </summary>
        public DateTime StartDateTimeInUtc
        {
            get { return Schedule.StartTime; }
            set { Schedule.StartTime = value; }
        }

        class TempActionScheduleBridge : ActionSchedule
        {
            public override Scheduling.ActionScheduleType ScheduleType
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
