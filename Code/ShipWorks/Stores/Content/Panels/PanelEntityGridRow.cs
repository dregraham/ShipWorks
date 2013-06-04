using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Paging;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using Divelements.SandGrid;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Grid.DetailView;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Specialized grid row for display in the panels
    /// </summary>
    public class PanelEntityGridRow : PagedEntityGrid.PagedEntityGridRow
    {
        /// <summary>
        /// Draw the extra header for the row
        /// </summary>
        protected override int DrawRowExtraHeaderContent(RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
        {
            int totalHeight = 0;

            PanelEntityGrid panelGrid = (PanelEntityGrid) EntityGrid;
            GroupingContext groupingContext = panelGrid.GroupingContext;

            if (panelGrid.GroupingContext != null)
            {
                if (Entity != null)
                {
                    long? previousRowGroupID = null;

                    // If we are not the first item in the grid, see what order the previous item goes with
                    if (IndexInGrid > 0)
                    {
                        PanelEntityGridRow previousRow = (PanelEntityGridRow) Grid.Rows[IndexInGrid - 1];
                        EntityBase2 previousEntity = previousRow.Entity;

                        if (previousEntity == null)
                        {
                            previousEntity = EntityGrid.EntityGateway.GetEntityFromRow(IndexInGrid - 1, null);
                        }

                        if (previousEntity != null)
                        {
                            previousRowGroupID = groupingContext.GetEntityGroupID(previousEntity);
                        }
                    }

                    long thisGroupID = groupingContext.GetEntityGroupID(Entity);

                    // If the order of the previous item is different than us, then draw a header
                    if (previousRowGroupID != groupingContext.GetEntityGroupID(Entity))
                    {
                        totalHeight = DetailViewSettings.SingleRowHeight;

                        EntityBase2 groupEntity = DataProvider.GetEntity(thisGroupID);

                        if (groupEntity != null)
                        {
                            using (Font bolded = new Font(Font, FontStyle.Bold))
                            {
                                string headerText = groupingContext.GetGroupHeader(groupEntity);
                                int headerTextWidth = IndependentText.MeasureText(context.Graphics, headerText, bolded, textFormats[0]).Width;

                                Rectangle headerBounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, totalHeight);
                                Rectangle headerTextBounds = new Rectangle(headerBounds.X + 3, headerBounds.Y, headerBounds.Width, headerBounds.Height);

                                // Fill in the background
                                context.Graphics.FillRectangle(SystemBrushes.Window, headerBounds);

                                // Draw the devider line that goes next to the text
                                context.Graphics.DrawLine(Pens.Silver, headerTextBounds.Left + headerTextWidth + 5, bounds.Top + headerBounds.Height / 2, bounds.Right, bounds.Top + headerBounds.Height / 2);

                                // Draw the text
                                IndependentText.DrawText(context.Graphics, headerText, bolded, headerTextBounds, textFormats[0], Color.Black);
                            }
                        }
                    }
                }
            }

            return totalHeight;
        }
    }
}
