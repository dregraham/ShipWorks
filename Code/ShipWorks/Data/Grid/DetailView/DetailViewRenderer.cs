using System;
using System.Collections.Generic;
using System.Linq;
using Divelements.SandGrid;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Common.Threading;
using System.Diagnostics;
using System.Threading;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.UI.Controls.SandGrid;
using Interapptive.Shared.Collections;

namespace ShipWorks.Data.Grid.DetailView
{
    /// <summary>
    /// Responsible for rendering the details view of a GridRow based on grid settings
    /// </summary>
    public class DetailViewRenderer : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DetailViewRenderer));

        // Cache of rendering results
        LruCache<string, DetailViewImage> imageCache = new LruCache<string, DetailViewImage>(150);

        // Tracks items that are waiting to be rendered
        LinkedList<PendingItem> pendingProcessingList = new LinkedList<PendingItem>();
        ManualResetEvent pendingExistsEvent = new ManualResetEvent(false);

        // Long instead of bool for use with Interlocked
        long threadSafeIsDisposed = 0;

        // This is non-null when there is data to be rendered, and keeps the connection from being messed with while we are fetching data.
        ApplicationBusyToken busyToken;

        // Maximum number of items that can be pending at time
        const int maxPendingItems = 50;

        class PendingItem
        {
            public DetailViewImage DetailsViewImage;
            public GridRow GridRow;
        }

        /// <summary>
        /// Standard constructor
        /// </summary>
        public DetailViewRenderer()
        {
            imageCache.CacheItemRemoved += new LruCacheItemRemovedEventHandler<string, DetailViewImage>(OnCachedImageRemoved);

            Thread thread = new Thread(ExceptionMonitor.WrapThread(BackgroundPrepareContent));
            thread.IsBackground = true;
            thread.Name = "DetailViewRenderer - BackgroundProcessImages";
            thread.Start();
        }

        /// <summary>
        /// Draw the details using the given settings and row information.  The desired detail view height is returned
        /// </summary>
        public int Draw(long? entityID, DetailViewSettings settings, GridRow gridRow, RenderingContext context, Rectangle rowBounds)
        {
            // If normal mode, then nothing to do
            if (settings == null || settings.DetailViewMode == DetailViewMode.Normal)
            {
                return 0;
            }

            // The bounds the details will be drawn to. The - 1 is to make room for the the grid row line.
            Rectangle detailBounds = new Rectangle(rowBounds.Location, new Size(rowBounds.Width - 1, rowBounds.Height - 1));

            // For Normal + Detail, we don't get the whole height - we have to make room for the real row output
            if (settings.DetailViewMode == DetailViewMode.NormalWithDetail)
            {
                // This is for the separator line
                detailBounds.Y += 1;
                detailBounds.Height -= 1;
            }

            // Determine what the background color should be
            Color foreColor;
            Color backColor;
            SandGridUtility.GetGridRowColors(gridRow, context, out foreColor, out backColor);

            // The grid
            SandGrid grid = (SandGrid) gridRow.Grid.SandGrid;

            // Draw a line at the top
            if (settings.DetailViewMode == DetailViewMode.NormalWithDetail)
            {
                // The line has to be within our allowed bounds.  This would not be the case when auto-sizing and the auto-size determined there
                // was zero content.  In that case there would be no detail area - and thus no spot to draw this line.
                if (rowBounds.Contains(rowBounds.Left, detailBounds.Top - 1))
                {
                    // We already adjusted down for the seperator line - thats what the -1's are for.
                    context.Graphics.DrawLine(context.GridLinePen, detailBounds.Left, detailBounds.Top - 1, detailBounds.Right, detailBounds.Top - 1);
                }
            }

            int idealHeight = 0;

            if (entityID != null)
            {
                // Get the content to be renderered
                DetailViewImage detailImage = GetImage(gridRow, entityID.Value, settings.TemplateID);

                // Draw the image
                idealHeight = detailImage.Draw(context.Graphics, grid.Font, detailBounds, foreColor, backColor);
            }

            // If DetailsRows is zero, then its auto-height
            if (settings.DetailRows == 0)
            {
                // Special case for no detail content
                if (idealHeight == 0)
                {
                    return (settings.DetailViewMode == DetailViewMode.DetailOnly) ? DetailViewSettings.SingleRowHeight : 0;
                }
                else
                {
                    // The ideal height is just for the detail content.  First we have to make room for the standard grid line.
                    idealHeight += 1;

                    // We don't add another 1 pixel for the bottom line when doing Normal+Detail... what will end up happening is the bottom line
                    // will overwrite the last row. Shouldn't be a problem - and keeps things sized correctly between modes.

                    // If an auto, return how high Draw says it should be.  But cap it to a reasonable level.
                    return Math.Min(100 * DetailViewSettings.SingleRowHeight, idealHeight);
                }
            }
            else
            {
                return DetailViewSettings.SingleRowHeight * settings.DetailRows;
            }
        }

        /// <summary>
        /// Get the rendering content to use for drawing the given entity details onto the specified row with the given template.
        /// </summary>
        private DetailViewImage GetImage(GridRow gridRow, long entityID, long templateID)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired);

            // Cache based on the entity and the template
            string cacheKey = string.Format("{0}_{1}", entityID, templateID);

            // Get the cache or create a new entry
            DetailViewImage detailsImage = imageCache[cacheKey];
            if (detailsImage == null)
            {
                detailsImage = new DetailViewImage(entityID, templateID);
                imageCache[cacheKey] = detailsImage;
            }

            lock (pendingProcessingList)
            {
                bool alreadyPending = pendingProcessingList.Any(p => p.DetailsViewImage == detailsImage);

                // If a prepare is not already pending, but needs to be, do that now
                if (!alreadyPending && detailsImage.CheckForTemplateProcessingNeeded())
                {
                    // If we are in a connection sensitive scope, we can't be fetching data in the background, b\c the connection
                    // could be changed out from under us.
                    if (!ConnectionSensitiveScope.IsActive)
                    {
                        // Add the item to the list of content that needs prepared
                        AppendPendingItem(
                            new PendingItem
                            {
                                DetailsViewImage = detailsImage,
                                GridRow = gridRow
                            });
                    }
                }
            }

            return detailsImage;
        }

        /// <summary>
        /// Append an item to the pending list.  Assumes the list is already locked\syncronized in some way for multithreaded safety.
        /// </summary>
        private void AppendPendingItem(PendingItem item)
        {
            // If there's already too many pending, remove the oldest one
            if (pendingProcessingList.Count >= maxPendingItems)
            {
                pendingProcessingList.RemoveFirst();
            }

            pendingProcessingList.AddLast(item);

            bool alreadySet = pendingExistsEvent.WaitOne(0, false);
            if (!alreadySet)
            {
                Debug.Assert(busyToken == null);
                busyToken = ApplicationBusyManager.OperationStarting("preparing details view");

                pendingExistsEvent.Set();
            }
        }

        /// <summary>
        /// Background thread for preparing the content of the detail views
        /// </summary>
        private void BackgroundPrepareContent()
        {
            while (!ThreadSafeIsDisposed)
            {
                // Wait until there is content to process
                pendingExistsEvent.WaitOne();

                // The next content we will process
                PendingItem pendingItem = null;

                do
                {
                    lock (pendingProcessingList)
                    {
                        // If there are any pending, pop one off the tiop
                        if (pendingProcessingList.Count > 0)
                        {
                            pendingItem = pendingProcessingList.First.Value;
                        }
                        // Otherwise we're done
                        else
                        {
                            pendingItem = null;
                            pendingExistsEvent.Reset();

                            // Let the operation manager know we are done.  Could be null if going through this path due to the event being set during dispose
                            if (busyToken != null)
                            {
                                ApplicationBusyManager.OperationComplete(busyToken);
                                busyToken = null;
                            }
                        }
                    }

                    // If we popped content to prepare
                    if (pendingItem != null)
                    {
                        GridRow gridRow = pendingItem.GridRow;

                        InnerGrid grid = gridRow.Grid;

                        // If the row is still in the grid and visible
                        if (grid != null)
                        {
                            int vScroll = grid.SandGrid.VScrollOffset;
                            Rectangle visibleBounds = new Rectangle(gridRow.Bounds.Left, grid.SandGrid.ScrollableViewportBounds.Top + vScroll, gridRow.Bounds.Width, grid.SandGrid.ScrollableViewportBounds.Height);

                            // See if its visibile in the grid
                            if (gridRow.Bounds.IntersectsWith(visibleBounds))
                            {
                                pendingItem.DetailsViewImage.ExecuteTemplateProcessing();

                                // We could have been disposed while working... don't continue if that's the case.
                                if (!ThreadSafeIsDisposed)
                                {
                                    // We need to tell the row to redraw now that the content is prepared
                                    Program.MainForm.BeginInvoke((MethodInvoker) delegate
                                    {
                                        if (gridRow.Grid != null)
                                        {
                                            // A little hack to force the row to redraw
                                            gridRow.ContentsUnknown = !gridRow.ContentsUnknown;
                                        }
                                    });
                                }
                            }
                            else
                            {
                                // GridRow no longer in scroll
                            }
                        }
                        else
                        {
                            // GridRow went away
                        }

                        // Now that its all done, we can remove it from the list
                        lock (pendingProcessingList)
                        {
                            pendingProcessingList.Remove(pendingItem);
                        }
                    }

                }
                while (pendingItem != null);
            }

            pendingExistsEvent.Close();
        }

        /// <summary>
        /// Called when a cached object has been removed from the cache
        /// </summary>
        void OnCachedImageRemoved(object sender, LruCacheItemRemovedEventArgs<string, DetailViewImage> e)
        {
            DetailViewImage detailsContent = e.Item;
            detailsContent.Dispose();
        }

        /// <summary>
        /// A thread-safe version of IsDisposed
        /// </summary>
        protected bool ThreadSafeIsDisposed
        {
            get { return Interlocked.Read(ref threadSafeIsDisposed) > 0; }
        }

        /// <summary>
        /// Dispose the contents of the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // We will get a remove notification for each item in the cache
                imageCache.Clear();

                lock (pendingProcessingList)
                {
                    pendingProcessingList.Clear();
                }

                // We need to do this to release the background thread
                pendingExistsEvent.Set();
            }

            Interlocked.Increment(ref threadSafeIsDisposed);
        }
    }
}
