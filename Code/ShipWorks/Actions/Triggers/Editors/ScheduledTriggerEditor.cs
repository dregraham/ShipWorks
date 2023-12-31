﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Scheduling;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Editor for scheduled triggers
    /// </summary>
    public partial class ScheduledTriggerEditor : ActionTriggerEditor
    {
        private readonly ScheduledTrigger trigger;

        // The menus for choosing what its inputs will be
        ContextMenuStrip actionScheduleTypesMenu;

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
            DateTime localStartTime = trigger.Schedule.StartDateTimeInUtc.ToLocalTime();
            
            startDate.Value = localStartTime.Date;
            startTime.Value = localStartTime;

            if (trigger.Schedule.EndsOnType == ActionEndsOnType.SpecificDateTime)
            {
                endsNeverRadioButton.Checked = false;
                endsOnRadioButton.Checked = true;
                endsOnDate.Enabled = true;
                endsOnTime.Enabled = true;

                if (trigger.Schedule.EndDateTimeInUtc.HasValue)
                {
                    DateTime localEndTime = trigger.Schedule.EndDateTimeInUtc.Value.ToLocalTime();

                    endsOnDate.Value = localEndTime;
                    endsOnTime.Value = localEndTime;
                }
            }
            else
            {
                endsNeverRadioButton.Checked = true;
                endsOnRadioButton.Checked = false;
                endsOnDate.Enabled = false;
                endsOnTime.Enabled = false;
            }

            UpdateScheduledTriggerOptions();
        }

        /// <summary>
        /// Update the menu options for the selected ScheduledTrigger
        /// </summary>
        private void UpdateScheduledTriggerOptions()
        {
            actionScheduleTypesMenu = new ContextMenuStrip();

            foreach (ActionScheduleType target in Enum.GetValues(typeof(ActionScheduleType)))
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(EnumHelper.GetDescription(target));
                menuItem.Click += new EventHandler(OnChangeInputSource);
                menuItem.Tag = target;

                actionScheduleTypesMenu.Items.Add(menuItem);
            }

            SelectInputSource(trigger.Schedule.ScheduleType);
        }

        /// <summary>
        /// Select the given input source as the current one for the task
        /// </summary>
        private void SelectInputSource(ActionScheduleType actionScheduleType)
        {
            // Update the displayed text
            actionScheduleTypeLink.Text = EnumHelper.GetDescription(actionScheduleType);

            bool isRecurringAction = actionScheduleType != ActionScheduleType.OneTime;

            if (isRecurringAction)
            {
                UserControl actionScheduleEditor = trigger.Schedule.CreateEditor();
                actionScheduleEditor.Location = new Point(5, 10);

                DisposeRecurringSettingsGroupControls();
                recurringSettingsGroup.Controls.Add(actionScheduleEditor);

                recurringSettingsGroup.Height = actionScheduleEditor.Bottom + 5;
                this.Height = recurringSettingsGroup.Bottom + 5;
            }
            else
            {
                this.Height = panel1.Top;
                DisposeRecurringSettingsGroupControls();
            }

            // Hide or display the 'ends' UI if the trigger type is one-time
            endsLabel.Visible = isRecurringAction;
            panel1.Visible = isRecurringAction;

            recurringSettingsGroup.Visible = recurringSettingsGroup.Controls.Count > 0;
        }

        /// <summary>
        /// Remove each control from recurringSettingsGroup, then Dispose() each one.
        /// </summary>
        private void DisposeRecurringSettingsGroupControls()
        {
            foreach (UserControl oldEditor in recurringSettingsGroup.Controls)
            {
                recurringSettingsGroup.Controls.Remove(oldEditor);
                oldEditor.Dispose();
            }
        }

        /// <summary>
        /// Clicking the link to choose the data source
        /// </summary>
        private void OnClickInputSourceLink(object sender, EventArgs e)
        {
            actionScheduleTypesMenu.Show(actionScheduleTypeLink.Parent.PointToScreen(new Point(actionScheduleTypeLink.Left, actionScheduleTypeLink.Bottom)));
        }

        /// <summary>
        /// User has selected a different data source
        /// </summary>
        private void OnChangeInputSource(object sender, EventArgs e)
        {
            ActionScheduleType actionScheduleType = (ActionScheduleType) ((ToolStripMenuItem) sender).Tag;
            if (trigger.Schedule.ScheduleType != actionScheduleType)
            {
                var oldSchedule = trigger.Schedule;
                trigger.Schedule = ActionScheduleFactory.CreateActionSchedule(actionScheduleType);
                trigger.Schedule.StartDateTimeInUtc = oldSchedule.StartDateTimeInUtc;
                trigger.Schedule.EndDateTimeInUtc = oldSchedule.EndDateTimeInUtc;
                trigger.Schedule.EndsOnType = oldSchedule.EndsOnType;
                SelectInputSource(actionScheduleType);
            }
        }

        /// <summary>
        /// Updates the ends on controls based on radio selected
        /// </summary>
        void OnEndsRadioButtonsCheckedChanged(object sender, EventArgs e)
        {
            if (endsOnRadioButton.Checked)
            {
                trigger.Schedule.EndsOnType = ActionEndsOnType.SpecificDateTime;
                endsOnDate.Enabled = true;
                endsOnTime.Enabled = true;
            }
            else
            {
                trigger.Schedule.EndsOnType = ActionEndsOnType.Never;
                endsOnDate.Enabled = false;
                endsOnTime.Enabled = false;
            }
        }

        /// <summary>
        /// Called when [start date time changed].
        /// </summary>
        private void OnStartDateTimeChanged(object sender, EventArgs e)
        {
            // Separate controls are used to capture date/time - combine them to obtain the date and
            // use the UTC date since this getting assigned back to the trigger
            trigger.Schedule.StartDateTimeInUtc = startDate.Value.Add(startTime.Value.TimeOfDay).ToUniversalTime();

            // Deduct the seconds, so the trigger is set at the top of the minute (i.e. 12:41:00 AM instead of 12:41:43 AM)
            trigger.Schedule.StartDateTimeInUtc = trigger.Schedule.StartDateTimeInUtc.AddSeconds(-trigger.Schedule.StartDateTimeInUtc.Second);
        }

        /// <summary>
        /// Called when end date/time changed.
        /// </summary>
        private void OnEndDateTimeChanged(object sender, EventArgs e)
        {
            // Separate controls are used to capture ends on date/time - combine them to obtain the date and
            // use the UTC date since this getting assigned back to the trigger
            DateTime endDateTime = endsOnDate.Value.Date.Add(endsOnTime.Value.TimeOfDay).ToUniversalTime();

            // Add a second, so the trigger can run at the last second if needed.
            trigger.Schedule.EndDateTimeInUtc = endDateTime.AddSeconds(1);
        }
    }
}
