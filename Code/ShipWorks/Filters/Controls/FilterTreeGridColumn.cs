using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ShipWorks.Filters;
using ShipWorks.Properties;
using log4net;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Provides customized drawing for rows in the FilterTree
    /// </summary>
    class FilterTreeGridColumn : SandGridTreeColumn
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FilterTreeGridColumn));

        static List<FilterTreeGridRow> animatingRows = new List<FilterTreeGridRow>();
        static Image loadingImage = Resources.arrows_blue;
        static volatile bool isAnimating = false;
        static bool pendingFrameUpdate = false;

        #region Animation Control

        /// <summary>
        /// Start or stop animating the loading for the given row
        /// </summary>
        private static void Animate(bool animate, FilterTreeGridRow row)
        {
            lock (animatingRows)
            {
                // Start or stop animating this particular row
                if (animate)
                {
                    animatingRows.Add(row);
                }
                else
                {
                    animatingRows.Remove(row);
                }
            }

            UpdateAnimationState();
        }

        /// <summary>
        /// Update the state of the animation
        /// </summary>
        private static void UpdateAnimationState()
        {
            int animatingRowCount;

            lock (animatingRows)
            {
                animatingRowCount = animatingRows.Count;
            }

            // We need to start
            if (animatingRowCount > 0 && !isAnimating)
            {
                log.Debug("Start filter calculation animation.");

                isAnimating = true;
                ImageAnimator.Animate(loadingImage, new EventHandler(OnFrameChanged));
            }

            // We stopped animating
            if (animatingRowCount == 0 && isAnimating)
            {
                log.Debug("Stop filter calculation animation.");

                isAnimating = false;
                ImageAnimator.StopAnimate(loadingImage, new EventHandler(OnFrameChanged));
            }
        }

        /// <summary>
        /// The next frame is ready to be drawn
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void OnFrameChanged(object o, EventArgs e)
        {
            if (!isAnimating)
            {
                return;
            }

            if (Program.MainForm.InvokeRequired)
            {
                if (pendingFrameUpdate)
                {
                    return;
                }

                pendingFrameUpdate = true;

                Program.MainForm.BeginInvoke(new EventHandler(OnFrameChanged), o, e);
                return;
            }

            ImageAnimator.UpdateFrames(loadingImage);

            // Some rows may no longer be valid and attached to a grid
            List<FilterTreeGridRow> rowsToRemove = new List<FilterTreeGridRow>();

            lock (animatingRows)
            {
                List<SandGridBase> gridsToInvalidate = new List<SandGridBase>();

                // Mark each loading row as needing redrawn
                foreach (FilterTreeGridRow row in animatingRows)
                {
                    if (row.Grid == null)
                    {
                        rowsToRemove.Add(row);
                    }
                    else
                    {
                        if (!gridsToInvalidate.Contains(row.Grid.SandGrid))
                        {
                            gridsToInvalidate.Add(row.Grid.SandGrid);
                        }
                    }
                }

                foreach (SandGrid grid in gridsToInvalidate)
                {
                    grid.Invalidate();
                }

                // Get rid of all the necessary rows
                foreach (FilterTreeGridRow row in rowsToRemove)
                {
                    animatingRows.Remove(row);
                }
            }

            // If we removed any rows, the animation may need stopped
            if (rowsToRemove.Count > 0)
            {
                UpdateAnimationState();
            }

            pendingFrameUpdate = false;
        }

        #endregion

        /// <summary>
        /// Draw the cell, adding the filter count to it
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreTooManyParams]
        protected override void DrawCell(RenderingContext context, GridRow row, object value, Font cellFont, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color cellForeColor)
        {
            FilterTreeGridRow filterTreeRow = (FilterTreeGridRow) row;

            textFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            textFormat.StringFormat.LineAlignment = StringAlignment.Center;
            textFormat.StringFormat.Trimming = StringTrimming.None;

            base.DrawCell(context, row, value, cellFont, image, bounds, selected, textFormat, cellForeColor);

            // How big the text is drawn by default
            Size size = IndependentText.MeasureText(context.Graphics, value.ToString(), cellFont, textFormat);
            int imageAndTextWidth = size.Width + image.Width + Grid.ImageTextSeparation + 3;

            Rectangle countBounds = bounds;
            countBounds.Inflate(-4, 0);
            countBounds.X += imageAndTextWidth;

            // Get the count
            FilterCount count = filterTreeRow.FilterCount;

            // Prepare animation
            bool needAnimate = !(count == null || count.Status == FilterCountStatus.Ready);
            Animate(needAnimate, filterTreeRow);

            // If we have a count at all
            if (count != null)
            {
                Color countColor = Color.Blue;

                SandGridTree sandTree = row.Grid.SandGrid as SandGridTree;
                if (sandTree != null && sandTree.SelectFoldersOnly && !filterTreeRow.IsFolder)
                {
                    countColor = Color.CornflowerBlue;
                }

                // Adjust font for disabled nodes
                if (filterTreeRow.IsFilterDisabled())
                {
                    using (DisabledFilterFont disabledFont = new DisabledFilterFont(cellFont))
                    {
                        countColor = disabledFont.TextColor;
                        cellFont = disabledFont.Font;   
                    }
                }

                // Allow the row a chance to update its style based on the state of the filter node. We want
                // the styling the filter name to reflect whether it is enabled/disabled.
                filterTreeRow.UpdateStyle();

                // If its ready, we can draw a number.
                if (count.Status == FilterCountStatus.Ready)
                {
                    IndependentText.DrawText(context.Graphics, string.Format("({0:#,##0})", count.Count), cellFont, countBounds, textFormat, countColor);
                }
                // Otherwise, we draw the animation
                else
                {
                    Size parenSize = IndependentText.MeasureText(context.Graphics, "(", cellFont, textFormat);
                    IndependentText.DrawText(context.Graphics, "(", cellFont, countBounds, textFormat, countColor);
                    countBounds.Offset(parenSize.Width, 0);

                    context.Graphics.DrawImage(loadingImage, countBounds.Left, countBounds.Top + (countBounds.Height / 2) - 5, 10, 10);
                    countBounds.Offset(10, 0);

                    IndependentText.DrawText(context.Graphics, ")", cellFont, countBounds, textFormat, countColor);
                }
            }
        }
    }
}
