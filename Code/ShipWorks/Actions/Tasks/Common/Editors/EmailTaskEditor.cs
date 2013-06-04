using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Task settings editor for the EmailTask
    /// </summary>
    public partial class EmailTaskEditor : TemplateBasedTaskEditor
    {
        EmailTask task;
        ContextMenuStrip delayTimeMenu;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailTaskEditor(EmailTask task) : base(task)
        {
            InitializeComponent();

            this.task = task;
        }

        /// <summary>
        /// Update the editor basd on the given current trigger
        /// </summary>
        public override void NotifyTaskInputChanged(ActionTrigger trigger, EntityType? inputType)
        {
            base.NotifyTaskInputChanged(trigger, inputType);

            bool shipDateSupported = inputType != null && inputType.Value == EntityType.ShipmentEntity;

            if (!shipDateSupported && task.DelayType == EmailDelayType.ShipDate)
            {
                task.DelayType = EmailDelayType.TimeDays;
            }

            UpdateDelayTimeMenu(shipDateSupported);
            UpdateDelayTimeUI();
        }

        /// <summary>
        /// Update the UI that controls if delay time.
        /// </summary>
        private void UpdateDelayTimeUI()
        {
            delayDelivery.Checked = task.DelayDelivery;

            delayTimeLink.Enabled = task.DelayDelivery;
            labelAt.Enabled = task.DelayDelivery;
            delayTimeOfDay.Enabled = task.DelayDelivery;

            if (task.DelayType == EmailDelayType.ShipDate)
            {
                delayTimeLink.Text = "on the shipment ship date";
            }
            else if (task.DelayType == EmailDelayType.DayOfWeek)
            {
                delayTimeLink.Text = string.Format("on the upcoming {0}", (DayOfWeek) task.DelayQuantity);
            }
            else
            {
                string unit = GetTimeUnitText(task.DelayType, task.DelayQuantity);

                delayTimeLink.Text = string.Format("after {0} {1}", task.DelayQuantity, unit);
            }

            delayTimeOfDay.Visible = task.DelayType != EmailDelayType.TimeMinutes && task.DelayType != EmailDelayType.TimeHours;
            delayTimeOfDay.Value = new DateTime(2000, 1, 1, task.DelayTimeOfDay.Hours, task.DelayTimeOfDay.Minutes, 0);

            labelAt.Visible = delayTimeOfDay.Visible;
            labelAt.Left = delayTimeLink.Right - 2;

            delayTimeOfDay.Left = labelAt.Right;
        }

        /// <summary>
        /// Get the time unit text basd on the delay type and qauntity
        /// </summary>
        private string GetTimeUnitText(EmailDelayType delayType, int quantity)
        {
            string value;

            switch (delayType)
            {
                case EmailDelayType.TimeDays:
                    value = "day";
                    break;

                case EmailDelayType.TimeHours:
                    value = "hour";
                    break;

                case EmailDelayType.TimeMinutes:
                    value = "minute";
                    break;

                case EmailDelayType.TimeWeeks:
                    value = "week";
                    break;

                default:
                    throw new InvalidOperationException("Invalid delayType");
            }

            if (quantity > 1)
            {
                value += "s";
            }

            return value;
        }

        /// <summary>
        /// Update the menu that lets you pick how long to delay
        /// </summary>
        private void UpdateDelayTimeMenu(bool shipDateSupported)
        {
            delayTimeMenu = new ContextMenuStrip();

            if (shipDateSupported)
            {
                delayTimeMenu.Items.Add(new ToolStripMenuItem("On the shipment ship date", null, OnChangeDelayTime) { Tag = null });
                delayTimeMenu.Items.Add(new ToolStripSeparator());
            }

            PopulateTimeMenu(delayTimeMenu.Items.Add("Minutes"), 30, "minutes", EmailDelayType.TimeMinutes);
            PopulateTimeMenu(delayTimeMenu.Items.Add("Hours"), 24, "hours", EmailDelayType.TimeHours);
            PopulateTimeMenu(delayTimeMenu.Items.Add("Days"), 30, "days", EmailDelayType.TimeDays);
            PopulateTimeMenu(delayTimeMenu.Items.Add("Weeks"), 4, "weeks", EmailDelayType.TimeWeeks);

            delayTimeMenu.Items.Add(new ToolStripSeparator());
            PopulateDayOfWeekMenu(delayTimeMenu.Items.Add("On the upcoming"));
        }

        /// <summary>
        /// Populate the time submenu
        /// </summary>
        private void PopulateTimeMenu(ToolStripItem parent, int quantity, string type, EmailDelayType unit)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) parent;

            for (int i = 1; i <= quantity; i++)
            {
                object tag = new object[] { i, unit };

                menuItem.DropDownItems.Add(new ToolStripMenuItem(
                    string.Format("After {0} {1}", i, i == 1 ? type.TrimEnd('s') : type), null,
                    OnChangeDelayTime) { Tag = tag });
            }
        }

        /// <summary>
        /// Populate the menu for selecting an upcoming day of the week
        /// </summary>
        private void PopulateDayOfWeekMenu(ToolStripItem parent)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) parent;

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().OrderBy(d => (int) d))
            {
                menuItem.DropDownItems.Add(new ToolStripMenuItem(
                    day.ToString(),
                    null,
                    OnChangeDelayTime) { Tag = day });
            }
        }

        /// <summary>
        /// Clicking the link to choose the delay time
        /// </summary>
        private void OnClickLinkDelayTime(object sender, EventArgs e)
        {
            delayTimeMenu.Show(delayTimeLink.Parent.PointToScreen(new Point(delayTimeLink.Left, delayTimeLink.Bottom)));
        }

        /// <summary>
        /// Changing whether delivery delay is enabled
        /// </summary>
        private void OnChangeDelayDelivery(object sender, EventArgs e)
        {
            task.DelayDelivery = delayDelivery.Checked;

            UpdateDelayTimeUI();
        }

        /// <summary>
        /// Change the delay time used
        /// </summary>
        private void OnChangeDelayTime(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;

            if (menuItem.Tag == null)
            {
                task.DelayType = EmailDelayType.ShipDate;
            }
            else if (menuItem.Tag is object[])
            {
                task.DelayType = (EmailDelayType) ((object[]) menuItem.Tag)[1];
                task.DelayQuantity = (int) ((object[]) menuItem.Tag)[0];
            }
            else
            {
                task.DelayType = EmailDelayType.DayOfWeek;
                task.DelayQuantity = (int) (DayOfWeek) menuItem.Tag;
            }

            UpdateDelayTimeUI();
        }

        /// <summary>
        /// Changing the time that the delay goes to on the configured day
        /// </summary>
        private void OnChangeDelayTimeOfDay(object sender, EventArgs e)
        {
            task.DelayTimeOfDay = delayTimeOfDay.Value.TimeOfDay;

            UpdateDelayTimeUI();
        }
    }
}
