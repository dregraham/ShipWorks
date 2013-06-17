using System;

namespace ShipWorks.Actions.Triggers.Editors
{
    public partial class ScheduledTriggerEditor : ActionTriggerEditor
    {
        private readonly ScheduledTrigger trigger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTriggerEditor"/> class.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        public ScheduledTriggerEditor(ScheduledTrigger trigger)
        {
            InitializeComponent();

            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            this.trigger = trigger;
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            DateTime localTime = trigger.StartDateTimeInUtc.ToLocalTime();

            startDate.Value = localTime.Date;
            startTime.Value = localTime;
        }

        /// <summary>
        /// Called when [start date time changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnStartDateTimeChanged(object sender, EventArgs e)
        {
            // Separate controls are used to capture date/time - combine them to obtain the date and
            // use the UTC date since this getting assigned back to the trigger
            trigger.StartDateTimeInUtc = startDate.Value.Add(startTime.Value.TimeOfDay).ToUniversalTime();

            // Deduct the seconds, so the trigger is set at the top of the minute (i.e. 12:41:00 AM instead of 12:41:43 AM)
            trigger.StartDateTimeInUtc = trigger.StartDateTimeInUtc.AddSeconds(-trigger.StartDateTimeInUtc.Second);
        }
    }
}
