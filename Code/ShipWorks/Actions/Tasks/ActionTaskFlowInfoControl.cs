using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;
using System.Diagnostics;
using ShipWorks.Filters.Controls;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Control that displays information about the flow settings for a task
    /// </summary>
    public partial class ActionTaskFlowInfoControl : UserControl
    {
        /// <summary>
        /// Raised when the flow link is clicked
        /// </summary>
        public event EventHandler FlowClicked;

        private readonly FilterControlDisplayManager filterDisplayManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskFlowInfoControl()
        {
            InitializeComponent();

            filterDisplayManager = new FilterControlDisplayManager(filterName, filterCount);
        }

        /// <summary>
        /// The flow link has been clicked
        /// </summary>
        private void OnClickFlowLink(object sender, EventArgs e)
        {
            if (FlowClicked != null)
            {
                FlowClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update the flow control info that is displayed for the given task
        /// </summary>
        public void UpdateInfoDisplay(ActionTask task, ActionTrigger trigger, IEnumerable<ActionTaskBubble> allBubbles)
        {
            ActionTaskEntity entity = task.Entity;

            bool anyDifferent = false;

            // Check the filter condition
            panelFilter.Visible = CheckFilterCondition(entity, trigger.TriggeringEntityType);
            anyDifferent |= panelFilter.Visible;

            // Success fulow
            panelSuccess.Visible = CheckFlowCondition(whenSuccess, entity.FlowSuccess, entity.FlowSuccessBubble, allBubbles);
            anyDifferent |= panelSuccess.Visible;

            // Skipped flow
            panelSkipped.Visible = task.Entity.FilterCondition && CheckFlowCondition(whenSkipped, entity.FlowSkipped, entity.FlowSkippedBubble, allBubbles);
            anyDifferent |= panelSkipped.Visible;

            // Error flow
            panelError.Visible = CheckFlowCondition(whenError, entity.FlowError, entity.FlowErrorBubble, allBubbles);
            anyDifferent |= panelError.Visible;

            // If any are different we are as tall as the last visible one
            if (anyDifferent)
            {
                Height = Controls.OfType<Control>().First(c => c.Visible).Bottom + 4;
            }
            // Otherwise we have no height at all
            else
            {
                Height = 0;
            }
        }

        /// <summary>
        /// Check the flow condition to see if its standard or not
        /// </summary>
        private bool CheckFlowCondition(Label label, long flowOption, object nextBubble, IEnumerable<ActionTaskBubble> allBubbles)
        {
            if (flowOption == (int) ActionTaskFlowOption.NextStep)
            {
                return false;
            }

            if (Enum.IsDefined(typeof(ActionTaskFlowOption), (ActionTaskFlowOption) flowOption))
            {
                label.Text = EnumHelper.GetDescription((ActionTaskFlowOption) flowOption);
            }
            else
            {
                if (nextBubble != null && allBubbles.Any(b => b == nextBubble))
                {
                    int index = allBubbles.ToList().IndexOf((ActionTaskBubble) nextBubble);

                    label.Text = string.Format("Go to step {0}: {1}", index + 1, ActionTaskManager.GetBinding(((ActionTaskBubble) nextBubble).ActionTask).FullName);
                }
                else
                {
                    // If the next task isn't found, the default is to go to the next step, which is the default condition, so we don't show it
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if the filter condition is non-standard
        /// </summary>
        private bool CheckFilterCondition(ActionTaskEntity entity, EntityType? triggeringEntity)
        {
            bool showCondition = entity.FilterCondition && triggeringEntity != null;

            if (showCondition)
            {
                panelFilter.Visible = true;

                labelFilter.Text = string.Format("Run only if the {0} is in:", ActionTaskFlowDlg.GetTriggeringEntityDescription(triggeringEntity));
                panelFilterName.Left = labelFilter.Right;

                FilterNodeEntity filterNode = FilterLayoutContext.Current.FindNode(entity.FilterConditionNodeID);

                // Its invalid if the triggering entity has changed since picking the node for another filter type
                if (filterNode != null && FilterHelper.GetEntityType((FilterTarget) filterNode.Filter.FilterTarget) != triggeringEntity)
                {
                    filterNode = null;
                }

                if (filterNode != null)
                {
                    filterPicture.Visible = true;

                    filterName.Visible = true;
                    filterName.Text = filterNode.Filter.Name;
                    
                    filterCount.Left = filterName.Right - 3;

                    filterDisplayManager.ToggleDisplay(filterNode.Filter.State == (byte)FilterState.Enabled);

                    FilterCount count = FilterContentManager.GetCount(filterNode.FilterNodeID);
                    if (count != null)
                    {
                        if (count.Status == FilterCountStatus.Ready)
                        {
                            filterCount.Visible = true;
                            filterCount.Text = string.Format("({0})", count.Count);
                        }
                        else
                        {
                            filterCount.Visible = false;
                        }
                    }
                    else
                    {
                        filterCount.Visible = false;
                    }
                }
                else
                {
                    filterPicture.Visible = false;
                    filterName.Visible = false;
                    filterCount.Visible = false;
                }
            }

            return showCondition;
        }
    }
}
