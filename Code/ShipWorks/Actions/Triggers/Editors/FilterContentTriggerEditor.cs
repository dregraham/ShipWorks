using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;
using Interapptive.Shared.Utility;
using System.Text.RegularExpressions;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Editor for the filter content changed trigger
    /// </summary>
    public partial class FilterContentTriggerEditor : ActionTriggerEditor
    {
        FilterContentTrigger trigger;
        ContextMenuStrip filterTargetMenu;
        ContextMenuStrip directionMenu;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterContentTriggerEditor(FilterContentTrigger trigger)
        {
            InitializeComponent();

            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            this.trigger = trigger;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            filterTarget.Text = GetTargetText(trigger.FilterTarget);

            // load the filter
            filterComboBox.LoadLayouts(trigger.FilterTarget);
            filterComboBox.SelectedFilterNodeID = trigger.FilterNodeID;

            // Load the filter target context menu
            filterTargetMenu = new ContextMenuStrip();
            foreach (FilterTarget target in Enum.GetValues(typeof(FilterTarget)))
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(GetTargetText(target));
                menuItem.Click += new EventHandler(OnChangeFilterTarget);
                menuItem.Tag = target;

                filterTargetMenu.Items.Add(menuItem);
            }

            // Load the direction menu
            directionMenu = new ContextMenuStrip();
            directionMenu.Items.Add("enters", null, OnChangeDirection).Tag = FilterContentChangeDirection.Entering;
            directionMenu.Items.Add("leaves", null, OnChangeDirection).Tag = FilterContentChangeDirection.Leaving;
            labelDirection.Text = directionMenu.Items.Cast<ToolStripMenuItem>().Single(i => (FilterContentChangeDirection) i.Tag == trigger.Direction).Text;

            UpdateControlLayout();
        }

        /// <summary>
        /// Update the dynamic layout of the controls based on size
        /// </summary>
        private void UpdateControlLayout()
        {
            bool vowel = Regex.Match(filterTarget.Text, "^(a|e|i|o|u)", RegexOptions.IgnoreCase).Success;

            labelWhen.Text = string.Format("When a{0}", vowel ? "n" : "");

            filterTarget.Left = labelWhen.Right - 3;
            labelDirection.Left = filterTarget.Right - 3;
            filterComboBox.Left = labelDirection.Right - 1;
        }

        /// <summary>
        /// We can't just use Enum.GetDescription since we wan't to show it as singular, and the enum descriptions
        /// are plural.
        /// </summary>
        private string GetTargetText(FilterTarget target)
        {
            string plural = EnumHelper.GetDescription(target);
            string singular = plural.Remove(plural.Length - 1);

            return singular;
        }

        /// <summary>
        /// FilterTarget link has been clicked
        /// </summary>
        private void OnClickFilterTarget(object sender, EventArgs e)
        {
            filterTargetMenu.Show(filterTarget.Parent.PointToScreen(new Point(filterTarget.Left, filterTarget.Bottom)));
        }

        /// <summary>
        /// Changing the selected filter target
        /// </summary>
        void OnChangeFilterTarget(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            FilterTarget target = (FilterTarget) menuItem.Tag;

            if (target != trigger.FilterTarget)
            {
                filterTarget.Text = GetTargetText(target);
                trigger.FilterTarget = target;

                filterComboBox.LoadLayouts(trigger.FilterTarget);
                filterComboBox.SelectedFilterNode = null;

                UpdateControlLayout();
            }
        }

        /// <summary>
        /// The selected filter node has changed
        /// </summary>
        private void OnFilterNodeChanged(object sender, EventArgs e)
        {
            trigger.FilterNodeID = filterComboBox.SelectedFilterNode != null ? filterComboBox.SelectedFilterNode.FilterNodeID : 0;
        }

        /// <summary>
        /// Clicking the direction link
        /// </summary>
        private void OnClickLabelDirection(object sender, EventArgs e)
        {
            directionMenu.Show(labelDirection.Parent.PointToScreen(new Point(labelDirection.Left, labelDirection.Bottom)));
        }

        /// <summary>
        /// Changing the selected direction for entering\leaving the filter
        /// </summary>
        private void OnChangeDirection(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;

            labelDirection.Text = menuItem.Text;
            trigger.Direction = (FilterContentChangeDirection) menuItem.Tag;
        }
    }
}
