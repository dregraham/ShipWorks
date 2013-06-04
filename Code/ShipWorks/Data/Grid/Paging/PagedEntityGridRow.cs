using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using ShipWorks.Properties;
using System.Windows.Forms;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Diagnostics;
using ShipWorks.Data.Utility;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Data.Grid.DetailView;

namespace ShipWorks.Data.Grid.Paging
{
    public partial class PagedEntityGrid
    {
        /// <summary>
        /// Customized GridRow for use with the EntityGrid
        /// </summary>
        public class PagedEntityGridRow : EntityGridRow
        {
            // Logger
            static readonly ILog log = LogManager.GetLogger(typeof(PagedEntityGridRow));

            // Global data for showing loading info
            static TextFormattingInformation loadingFormat;
            static Image loadingImage = Resources.indiciator_green;

            // Tracks the rows are currently loading
            static List<PagedEntityGridRow> loadingRows = new List<PagedEntityGridRow>();
            static bool isAnimating = false;

            // Indicates if this row is currently in the loading state
            PagedDataState dataState = PagedDataState.None;

            // The entity that this row represents.  Only valid once the datastate is Loaded
            long? entityID;
            EntityBase2 entity;

            // When we go from loaded to not loaded, we copy the entity data so it can continue to be displayed until the row get's actually
            // removed.  This prevents flickering
            EntityBase2 ghostEntity;
            bool drawing = false;

            /// <summary>
            /// Static contsructor
            /// </summary>
            static PagedEntityGridRow()
            {
                loadingFormat = new TextFormattingInformation();
                loadingFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
                loadingFormat.TextFormatFlags = TextFormatFlags.VerticalCenter;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public PagedEntityGridRow()
            {

            }

            #region Animation Control

            /// <summary>
            /// Start or stop animating the loading for the given row
            /// </summary>
            private static void Animate(bool animate, PagedEntityGridRow row)
            {
                // Start or stop animating this particular row
                if (animate)
                {
                    if (!loadingRows.Contains(row))
                    {
                        loadingRows.Add(row);
                    }
                }
                else 
                {
                    if (loadingRows.Contains(row))
                    {
                        loadingRows.Remove(row);
                    }
                }

                UpdateAnimationState();
            }

            /// <summary>
            /// Update the state of the animation
            /// </summary>
            private static void UpdateAnimationState()
            {
                // We need to start
                if (loadingRows.Count > 0 && !isAnimating)
                {
                    // log.Debug("Start loading animation.");

                    isAnimating = true;
                    ImageAnimator.Animate(loadingImage, new EventHandler(OnFrameChanged));
                }

                // We stopped animating
                if (loadingRows.Count == 0 && isAnimating)
                {
                    // log.Debug("Stop loading animation.");

                    isAnimating = false;
                    ImageAnimator.StopAnimate(loadingImage, new EventHandler(OnFrameChanged));
                }
            }

            /// <summary>
            /// The next frame is ready to be drawn
            /// </summary>
            private static void OnFrameChanged(object o, EventArgs e)
            {
                if (Program.MainForm.InvokeRequired)
                {
                    Program.MainForm.BeginInvoke(new EventHandler(OnFrameChanged), o, e);
                    return;
                }

                if (!isAnimating)
                {
                    return;
                }

                ImageAnimator.UpdateFrames(loadingImage);

                // Some rows may no longer be valid and attached to a grid
                List<PagedEntityGridRow> rowsToRemove = new List<PagedEntityGridRow>();

                // Mark each loading row as needing redrawn
                foreach (PagedEntityGridRow row in loadingRows)
                {
                    if (row.Grid == null)
                    {
                        rowsToRemove.Add(row);
                    }
                    else
                    {
                        row.RedrawNeeded();
                    }
                }

                // Get rid of all the necessary rows
                foreach (PagedEntityGridRow row in rowsToRemove)
                {
                    loadingRows.Remove(row);
                }

                // If we removed any rows, the animation may need stopped
                if (rowsToRemove.Count > 0)
                {
                    UpdateAnimationState();
                }
            }

            #endregion

            #region Selection and Performance

            /// <summary>
            /// Hack to prevent redraw during Rows.Clear.
            /// </summary>
            protected override void OnEnter()
            {
                if (EntityGrid.IsClearing)
                {
                    return;
                }

                base.OnEnter();
            }

            /// <summary>
            /// Hack to prevent redraw during Rows.Clear.
            /// </summary>
            protected override void OnLeave()
            {
                if (EntityGrid.IsClearing)
                {
                    return;
                }

                base.OnLeave();
            }

            /// <summary>
            /// User is about to make a potentially large block selection.
            /// </summary>
            public override void SelectBlock(FocusableGridElement startElement, FocusableGridElement toElement)
            {
                GridRow rowStart = startElement as GridRow;
                GridRow rowEnd = toElement as GridRow;
                if ((rowStart != null) && (rowEnd != null))
                {
                    if (Math.Abs(rowStart.Index - rowEnd.Index) > 2000)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                    }
                }

                base.SelectBlock(startElement, toElement);
            }

            #endregion

            /// <summary>
            /// Resets the row back to its initial state, except the previous entity data can still be displayed visually
            /// until its reloaded, to prevent flicker.
            /// </summary>
            public void Reset()
            {
                DataState = PagedDataState.Reset;
            }

            /// <summary>
            /// Complete clears the row back to its initial state.
            /// </summary>
            public void Clear()
            {
                DataState = PagedDataState.None;
            }

            /// <summary>
            /// The EntityGrid we are hosted in
            /// </summary>
            public PagedEntityGrid EntityGrid
            {
                get
                {
                    return (PagedEntityGrid) Grid.SandGrid;
                }
            }

            /// <summary>
            /// Indicates if this row is currently in the loading state
            /// </summary>
            public PagedDataState DataState
            {
                get
                {
                    return dataState;
                }
                set
                {
                    // Setting the header does not change our state - so we have to do this before the check of if dataState is actually changing.
                    // So if we are being cleared or reset, make sure to clear the header
                    if (value == PagedDataState.None || value == PagedDataState.Reset)
                    {
                        entityID = null;
                    }

                    if (dataState != value)
                    {
                        // If we are going from None to Reset there's really nothing to do
                        if (dataState == PagedDataState.None && value == PagedDataState.Reset)
                        {
                            return;
                        }

                        bool neededAnimate = dataState == PagedDataState.Loading || dataState == PagedDataState.Removing;
                        bool needsAnimate = value == PagedDataState.Loading || value == PagedDataState.Removing;

                        if (dataState == PagedDataState.Loaded && value != PagedDataState.None)
                        {
                            ghostEntity = EntityUtility.CloneEntity(entity);
                        }

                        if (value != PagedDataState.Loaded)
                        {
                            entityID = null;
                            entity = null;

                            if (value == PagedDataState.None)
                            {
                                ghostEntity = null;
                            }
                        }

                        bool wasReset = dataState == PagedDataState.Reset;

                        this.dataState = value;

                        if (neededAnimate || needsAnimate)
                        {
                            Animate(needsAnimate, this);
                        }

                        // Don't need to redraw if just going from Reset to None
                        if (wasReset && dataState == PagedDataState.None)
                        {
                            return;
                        }

                        RedrawNeeded();
                    }
                }
            }

            /// <summary>
            /// The entity in the database this row represents, along with any extra data configured to be retrieved by the grid.  
            /// Only valid if the row is in the Loaded state.
            /// </summary>
            public override EntityBase2 Entity
            {
                get
                {
                    if (DataState == PagedDataState.Loaded)
                    {
                        return entity;
                    }
                    else if (drawing)
                    {
                        return ghostEntity;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Get the ID of the entity represented by the row.
            /// </summary>
            public override long? EntityID
            {
                get
                {
                    EntityBase2 entity = Entity;
                    if (entity != null)
                    {
                        return (long) entity.Fields.PrimaryKeyFields[0].CurrentValue;
                    }

                    if (entityID != null)
                    {
                        return entityID;
                    }

                    return null;
                }
            }

            /// <summary>
            /// Load the given entity data into the row.
            /// </summary>
            public void LoadRowEntity(EntityBase2 entity)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this.entity = entity;
                this.entityID = (long) entity.Fields.PrimaryKeyFields[0].CurrentValue;

                this.DataState = PagedDataState.Loaded;
            }

            /// <summary>
            /// Load the given entity header into the row
            /// </summary>
            public void LoadRowEntityID(long entityID)
            {
                if (this.entityID != null)
                {
                    throw new InvalidOperationException("The header has already been initialized. There is a logic problem somewhere.");
                }

                this.entityID = entityID;
            }

            /// <summary>
            /// We don't ever have to provide a cell value.  Its basically just used for sorting, and since we sort in the db, it goes unused.
            /// </summary>
            public override object GetCellValue(GridColumn baseColumn)
            {
                return null;
            }

            /// <summary>
            /// Draw the background of the row
            /// </summary>
            protected override void DrawRowBackground(RenderingContext context)
            {
                // If our load had been deferred, now we need to make sure we are being loaded
                if (DataState == PagedDataState.None || DataState == PagedDataState.Reset || DataState == PagedDataState.LoadDeferred)
                {
                    EntityGrid.PromoteRowToLoading(this);
                }

                base.DrawRowBackground(context);
            }

            /// <summary>
            /// Overridden for drawing foreground
            /// </summary>
            protected override void DrawRowForeground(RenderingContext context, Rectangle bounds, GridColumn[] columns, TextFormattingInformation[] textFormats)
            {
                drawing = true;

                // If we are not loaded, don't draw any data.
                if (Entity == null)
                {
                    columns = new GridColumn[0];
                }

                base.DrawRowForeground(context, bounds, columns, textFormats);

                drawing = false;

                // Draw the loading look
                if (DataState == PagedDataState.Loading || DataState == PagedDataState.Removing)
                {
                    Point pos = EntityGrid.PointToGrid(new Point(EntityGrid.ShowRowHeaders ? 23 : 3, 0));

                    string text = (DataState == PagedDataState.Loading) ? "Loading..." : "Removing...";

                    Color foreColor;
                    Color backColor;
                    SandGridUtility.GetGridRowColors(this, context, out foreColor, out backColor);

                    Size size = IndependentText.MeasureText(context.Graphics, text, context.Font, loadingFormat);
                    using (SolidBrush brush = new SolidBrush(backColor))
                    {
                        Rectangle fillRect = new Rectangle(pos.X, bounds.Top + 1, size.Width + + 21 + 10, DetailViewSettings.SingleRowHeight - 2);
                        if (backColor.A < 255)
                        {
                            context.Graphics.FillRectangle(Brushes.White, fillRect);
                        }

                        context.Graphics.FillRectangle(brush, fillRect);
                    }

                    context.Graphics.DrawImage(loadingImage, pos.X, bounds.Top, 16, 16);
                    IndependentText.DrawText(context.Graphics, text, context.Font, new Rectangle(pos.X + 21, bounds.Top, bounds.Width, DetailViewSettings.SingleRowHeight - 2), loadingFormat, foreColor);
                }
            }
        }
    }
}
