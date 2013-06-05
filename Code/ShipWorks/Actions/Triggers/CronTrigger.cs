using System;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Triggers
{
    public class CronTrigger : ActionTrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CronTrigger"/> class.
        /// </summary>
        public CronTrigger()
            : base(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CronTrigger"/> class.
        /// </summary>
        /// <param name="xmlSettings"></param>
        public CronTrigger(string xmlSettings)
            : base(xmlSettings)
        { }

        /// <summary>
        /// Overridden to provide the trigger type
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.Cron; }
        }

        /// <summary>
        /// Creates the editor that is used to edit the condition.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ActionTriggerEditor CreateEditor()
        {
            return new CronTriggerEditor(this);
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
        /// Gets or sets the start date time in UTC.
        /// </summary>
        /// <value>The start date time in UTC.</value>
        public DateTime StartDateTimeInUtc { get; set; }
    }
}
