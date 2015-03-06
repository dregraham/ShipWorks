using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.Paging;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.UI.Controls.SandGrid;
using Divelements.SandGrid.Rendering;
using ShipWorks.Data.Grid;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data;
using ShipWorks.Properties;
using System.Windows.Forms;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Customized grid row for displaying action errors
    /// </summary>
    public class ActionErrorGridRow : PagedEntityGrid.PagedEntityGridRow
    {
        // Indicates if the details for each row should be drawn.
        static bool showDetails = false;

        /// <summary>
        /// Indicates if the details for each row should be drawn.  This can be static since the ActionError window is only
        /// open once at a time.
        /// </summary>
        public static bool ShowDetails
        {
            get { return showDetails; }
            set { showDetails = value; }
        }

        /// <summary>
        /// Draw the foreground of the row
        /// </summary>
        protected override int DrawRowExtraFooterContent(RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            if (!showDetails)
            {
                return 0;
            }

            int totalHeight = 0;

            ActionQueueEntity queue = Entity as ActionQueueEntity;
            if (queue != null)
            {
                // Add in enough room for a row for each attribute
                totalHeight += (queue.Steps.Count * DetailViewSettings.SingleRowHeight);
                int nextTop = bounds.Top - 1;

                // Determine what the background color should be
                Color foreColor;
                Color backColor;
                SandGridUtility.GetGridRowColors(this, context, out foreColor, out backColor);

                foreach (ActionQueueStepEntity step in queue.Steps.ToList())
                {
                    Rectangle stepBounds = new Rectangle(
                        bounds.Left,
                        nextTop,
                        bounds.Width - 1,
                        DetailViewSettings.SingleRowHeight);

                    int errorHeight = 0;

                    // Only draw if this is within our allowed row bounds
                    if (stepBounds.IntersectsWith(bounds))
                    {
                        // We need to draw the row separator line.  EDIT - i think it looks better without
                        // context.Graphics.DrawLine(context.GridLinePen, stepBounds.Left, stepBounds.Top, stepBounds.Right, stepBounds.Top);

                        errorHeight = DrawStepContent(step, context, stepBounds, columns, textFormats);
                    }

                    totalHeight += errorHeight;
                    nextTop += DetailViewSettings.SingleRowHeight + errorHeight;
                }
            }

            return totalHeight;
        }

        /// <summary>
        /// Draw the step within the given bounds
        /// </summary>
        private int DrawStepContent(ActionQueueStepEntity step, RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            int errorHeight = 0;
            Padding errorMargin = new Padding(55, 1, 10, 4);

            // Figure out how much extra our error height is going to be
            if (step.StepStatus == (int) ActionQueueStepStatus.Error)
            {
                using (TextFormattingInformation tfi = TextFormattingInformation.CreateFormattingInformation(false, true, StringAlignment.Near, StringAlignment.Near, true))
                {
                    Size errorSize = IndependentText.MeasureText(context.Graphics, step.AttemptError, context.Font, bounds.Width - (errorMargin.Left + errorMargin.Right), tfi);
                    errorHeight = errorSize.Height + errorMargin.Bottom;
                }
            }

            // Draw over all the vertical lines
            Rectangle contentCounts = bounds;
            contentCounts.Y += 1;
            contentCounts.Height -= 1;
            contentCounts.Height += errorHeight;

            bounds.Y += 1;
            bounds.Height -= 1;

            int left = bounds.Left + 20;

            bounds.X = left;
            bounds.Width -= left;

            // Draw the icon
            context.Graphics.DrawImage(GetStatusImage(GetStepStatus(step)), bounds.Left, bounds.Top + 1, 16, 16);

            bounds.X += 20;
            bounds.Width -= 20;

            string stepName = string.Format("{0}. {1}: {2}", step.StepIndex + 1, step.StepName, GetStatusText(GetStepStatus(step)));

            // draw the step name
            IndependentText.DrawText(context.Graphics, stepName, Font, bounds, textFormats[0], Color.DimGray);

            // Draw the error text
            if (step.StepStatus == (int) ActionQueueStepStatus.Error)
            {
                using (TextFormattingInformation tfi = TextFormattingInformation.CreateFormattingInformation(false, true, StringAlignment.Near, StringAlignment.Near, true))
                {
                    Rectangle textBounds = new Rectangle(errorMargin.Left, bounds.Bottom, bounds.Width - (errorMargin.Left + errorMargin.Right), errorHeight);
                    IndependentText.DrawText(context.Graphics, step.AttemptError, context.Font, textBounds, tfi, Color.Red);
                }
            }

            return errorHeight;
        }

        /// <summary>
        /// Get the effective status of the step
        /// </summary>
        private ActionQueueStepStatus GetStepStatus(ActionQueueStepEntity step)
        {
            ActionQueueStepStatus status = (ActionQueueStepStatus) step.StepStatus;

            if (status == ActionQueueStepStatus.Pending)
            {
                // If there aren't any errors that suspended the flow, then the flow must be done
                if (!step.ActionQueue.Steps.Any(s => s.StepStatus == (int) ActionQueueStepStatus.Error && s.FlowError == (int) ActionTaskFlowOption.Suspend))
                {
                    // Treat it as skipped for UI purposes, as it's never going to run
                    status = ActionQueueStepStatus.Skipped;
                }
            }

            return status;
        }

        /// <summary>
        /// Get a text string that describes the status of the step
        /// </summary>
        private string GetStatusText(ActionQueueStepStatus status)
        {
            switch (status)
            {
                case ActionQueueStepStatus.Success:
                    return "Success";

                case ActionQueueStepStatus.Error:
                    return "Error";

                case ActionQueueStepStatus.Pending:
                    return "Pending";

                case ActionQueueStepStatus.Skipped:
                    return "Skipped";

                case ActionQueueStepStatus.Postponed:
                    throw new InvalidOperationException("Should not have a postponed step in the error retry window.");

                default:
                    throw new InvalidOperationException("Unhandled step status");
            }
        }

        /// <summary>
        /// Get the image that will represent the step's status
        /// </summary>
        private Image GetStatusImage(ActionQueueStepStatus status)
        {
            switch (status)
            {
                case ActionQueueStepStatus.Success:
                    return Resources.check16;

                case ActionQueueStepStatus.Error:
                    return Resources.error16;

                case ActionQueueStepStatus.Pending:
                    return Resources.step_pause;

                case ActionQueueStepStatus.Skipped:
                    return Resources.nav_redo_yellow;

                case ActionQueueStepStatus.Postponed:
                    throw new InvalidOperationException("Should not have a postponed step in the error retry window.");

                default:
                    throw new InvalidOperationException("Unhandled step status");
            }
        }
    }
}
