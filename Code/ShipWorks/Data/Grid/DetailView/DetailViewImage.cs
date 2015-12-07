using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Templates.Processing;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.Html;
using System.Windows.Forms;
using ShipWorks.Common.Threading;
using log4net;
using Interapptive.Shared.Utility;
using System.Threading;
using System.Diagnostics;
using ShipWorks.Templates;
using ShipWorks.UI.Controls.Html.Core;
using Divelements.SandGrid.Rendering;
using Interapptive.Shared;
using ShipWorks.UI;

namespace ShipWorks.Data.Grid.DetailView
{
    /// <summary>
    /// Internal class to the renderer that is responsible for generating and caching the result bitmap.
    /// </summary>
    public class DetailViewImage : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DetailViewImage));

        static TextFormattingInformation textFormat;

        long entityID;
        long templateID;

        // The template and TemplateXsl object that was used to generate the htmlDocument
        TemplateEntity template;
        TemplateXsl templateXsl;

        // The background color and width the last time we generated the html output
        Color backColor = Color.Black;
        Size size;

        // The cached bitmap that we have generated
        Bitmap bitmap;
        int idealBitmapHeight = 0;

        // The html control used to do the rendering
        HtmlControl htmlControl;

        // So we know if our bitmap content matches what has recently been prepared
        long htmlRevision = 0;
        long bitmapRevision = 0;

        // Indicates if we should be trying to draw our rendered bitamp
        bool canDrawBitmap = false;

        // Indicates if dispose has been called
        volatile bool disposed = false;

        // Thread sync
        object processingLock = new object();

        // What the data sync version was last time we processed.
        long lastDataProviderSyncVersion = 0;

        /// <summary>
        /// Static constructor
        /// </summary>
        static DetailViewImage()
        {
            // Create the text used to draw messages
            textFormat = new TextFormattingInformation();
            textFormat.StringFormat = new StringFormat();
            textFormat.StringFormat.LineAlignment = StringAlignment.Center;
            textFormat.TextFormatFlags = TextFormatFlags.VerticalCenter;
        }

        /// <summary>
        /// The generator
        /// </summary>
        public DetailViewImage(long entityID, long templateID)
        {
            this.entityID = entityID;
            this.templateID = templateID;

            this.template = TemplateManager.Tree.GetTemplate(templateID);

            htmlControl = new HtmlControl();
            htmlControl.Left = 0;
            htmlControl.Top = 0;
            htmlControl.AllowActivation = false;
            htmlControl.Parent = new Control();

            Debug.Assert(!Program.MainForm.InvokeRequired, "Must be on the UI thread so the HtmlControl is on the main UI thread.");
        }

        /// <summary>
        /// Draw the prepared image using the specified graphics and the given bounds.  The ideal height of the bitmap
        /// is returned via idealHeight. This is the height that would need to be given in the bounds to not have any clipping.
        /// The value is only valid if the function returns true.
        /// </summary>
        public int Draw(Graphics g, Font font, Rectangle bounds, Color foreColor, Color backColor)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired, "Must be on the UI thread.");

            if (disposed)
            {
                return 0;
            }

            if (canDrawBitmap)
            {
                bool tookLock = false;

                try
                {
                    // If we are not in the middle of preparing the html, then we can update the bitmap
                    // to reflect the latest html state.
                    tookLock = Monitor.TryEnter(processingLock);

                    if (tookLock)
                    {
                        UpdateBitmap(bounds.Size, backColor);
                    }
                }
                finally
                {
                    if (tookLock)
                    {
                        Monitor.Exit(processingLock);
                    }
                }
            }

            // If we have a bitmap to draw, draw it
            if (bitmap != null)
            {
                if (bounds.Width > 0 && bounds.Height > 0)
                {
                    g.DrawImageUnscaledAndClipped(bitmap, bounds);
                }

                return idealBitmapHeight;
            }
            else
            {
                string message = (template != null) ? "Preparing details view..." : "No template is selected for displaying details.";

                return DrawMessage(
                   g,
                   message,
                   font,
                   bounds,
                   foreColor,
                   backColor);
            }
        }

        /// <summary>
        /// Indicates if ExecuteTemplateProcessing is required to be called given the template and state of the content.
        /// </summary>
        public bool CheckForTemplateProcessingNeeded()
        {
            Debug.Assert(!Program.MainForm.InvokeRequired, "Must be on the UI thread.");

            bool tookLock = false;

            try
            {
                // If we are not in the middle of preparing the html, then we can update the bitmap
                // to reflect the latest html state.
                tookLock = Monitor.TryEnter(processingLock);

                if (tookLock)
                {
                    template = TemplateManager.Tree.GetTemplate(templateID);

                    // If there is no template, there is no need for processing and no need for drawing the bitmap
                    // since we will just be drawing a message
                    if (template == null)
                    {
                        canDrawBitmap = false;
                        DisposeBitmap();

                        return false;
                    }

                    // If the data sync version has changed, then we need to redraw, b\c the data affecting the display of the template may have changed.  If it hasn't, its not a big deal, b\c all
                    // the data should still be in cache.
                    if (lastDataProviderSyncVersion != DataProvider.GetLastSqlSyncVersion())
                    {
                        return true;
                    }

                    // If the current TemplateXsl is different from what we last processed with, we need reprocessed
                    if (templateXsl != TemplateXslProvider.FromTemplate(template))
                    {
                        return true;
                    }

                    // No need to reprocess
                    return false;
                }
                else
                {
                    // If its processing now, we'll come back through here once its done (thanks to the redraw posted by the renderer)
                    // and we can just check again then.
                    return false;
                }
            }
            finally
            {
                if (tookLock)
                {
                    Monitor.Exit(processingLock);
                }
            }
        }

        /// <summary>
        /// Process the template and load the HTML control so that's ready for future rendering
        /// </summary>
        public void ExecuteTemplateProcessing()
        {
            lock (processingLock)
            {
                // We have been disposed, no need to do anytihng
                if (htmlControl == null)
                {
                    return;
                }

                Debug.Assert(htmlControl.InvokeRequired, "This only works if its on a thread that is not the UI thread due to the async nature of html loading.");

                // Set the current xsl object in use
                templateXsl = TemplateXslProvider.FromTemplate(template);

                // The background color will have to be reset when rendered
                backColor = Color.Black;

                try
                {
                    // Process the template
                    IList<TemplateResult> results = TemplateProcessor.ProcessTemplate(template, new List<long> { entityID });

                    // Load the HtmlDocument
                    htmlControl.Html = TemplateResultFormatter.FormatHtml(results, TemplateResultUsage.ShipWorksDisplay, TemplateResultFormatSettings.FromTemplate(template));
                    htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

                    idealBitmapHeight = htmlControl.DetermineIdealRenderedBitmapHeight();
                }
                catch (TemplateXslException ex)
                {
                    htmlControl.Html = TemplateResultFormatter.FormatError(ex, TemplateOutputFormat.Html);
                }

                // Once we've prepared once, we are ready to draw at any time.  It might be stale content, but it can be drawn.
                canDrawBitmap = true;

                // Force a bitmap recreation
                Interlocked.Increment(ref htmlRevision);

                // Mark when we processed last - we automatically reprocess after a timespan
                lastDataProviderSyncVersion = DataProvider.GetLastSqlSyncVersion();

                // If we were disposed while working, then it's our job to cleanup the html control
                if (disposed)
                {
                    htmlControl.Invoke((MethodInvoker) htmlControl.Dispose);
                    htmlControl = null;
                }
            }
        }

        /// <summary>
        /// Update our rendered bitmap
        /// </summary>
        private void UpdateBitmap(Size size, Color backColor)
        {
            // This is the minimum size i found the "RenderToBitmap" method to still work on Vista64.  Other OS' 1x1 was fine.  Not sure...
            size.Height = Math.Max(size.Height, 5);
            size.Width = Math.Max(size.Width, 5);

            // See if the bitmap we have is valid
            if (this.size == size &&
                this.backColor == backColor &&
                this.bitmapRevision == Interlocked.Read(ref htmlRevision) &&
                bitmap != null)
            {
                return;
            }

            DisposeBitmap();

            // If the width is off, we need to re-render
            if (this.size != size)
            {
                // Update the width we wil be using
                this.size = size;

                // Set the width
                htmlControl.Height = size.Height + 10;
                htmlControl.Width = size.Width;

                // Auto height
                idealBitmapHeight = htmlControl.DetermineIdealRenderedBitmapHeight();
            }

            // If color is off - or if we have recently re-rendered
            if (this.backColor != backColor || bitmapRevision != htmlRevision)
            {
                // Update our cached value
                this.backColor = backColor;

                // Update the html content
                htmlControl.HtmlDocument.Body.Style.SetBackgroundColor(ColorTranslator.ToHtml(backColor));
            }

            // Render the result
            bitmap = htmlControl.RenderToBitmap(new Rectangle(0, 0, size.Width, size.Height), backColor);

            // Since we are in the lock, we dont need Interlocked
            bitmapRevision = htmlRevision;
        }

        /// <summary>
        /// Draw a message in the row
        /// </summary>
        [NDependIgnoreTooManyParams]
        private static int DrawMessage(Graphics g, string message, Font font, Rectangle bounds, Color foreColor, Color backColor)
        {
            if (bounds.Height > 0 && bounds.Width > 0)
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, bounds);
                }

                if (foreColor.GetBrightness() < .2)
                {
                    foreColor = Color.Gray;
                }

                using (SolidBrush brush = new SolidBrush(foreColor))
                {
                    bounds.X += 15;
                    bounds.Width -= 15;

                    IndependentText.DrawText(g, message, font, bounds, textFormat, foreColor);
                }
            }

            // Always return the ideal height
            return DetailViewSettings.SingleRowHeight - 1;
        }

        /// <summary>
        /// Dispose the bitmap we have cached, if any
        /// </summary>
        private void DisposeBitmap()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }

        /// <summary>
        /// Dispose the contents of the object
        /// </summary>
        public void Dispose()
        {
            disposed = true;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired, "Must be on the UI thread.");

            if (disposing)
            {
                bool tookLock = false;

                try
                {
                    tookLock = Monitor.TryEnter(processingLock);

                    // If we have the lock we can dispose the html control now
                    if (tookLock)
                    {
                        if (htmlControl != null)
                        {
                            htmlControl.Dispose();
                            htmlControl = null;
                        }
                    }
                }
                finally
                {
                    if (tookLock)
                    {
                        Monitor.Exit(processingLock);
                    }
                }

                DisposeBitmap();
            }

            canDrawBitmap = false;
        }
    }
}
