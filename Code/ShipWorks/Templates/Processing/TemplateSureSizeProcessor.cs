using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.UI.Controls.Html;
using ShipWorks.UI.Controls.Html.Core;
using log4net;
using System.Diagnostics;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Responsible for processing SureSize funtionality on an HtmlControl
    /// </summary>
    public static class TemplateSureSizeProcessor
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateSureSizeProcessor));

        /// <summary>
        /// Process the given HtmlControl content for SureSize
        /// </summary>
        public static void Process(HtmlControl htmlControl)
        {
            if (htmlControl == null)
            {
                throw new ArgumentNullException("htmlControl");
            }

            Stopwatch sw = Stopwatch.StartNew();

            HtmlApi.IHTMLDocument3 document = (HtmlApi.IHTMLDocument3) htmlControl.HtmlDocument;
            HtmlApi.IHTMLElementCollection elements = document.getElementsByTagName("div");

            // First do any that are defined in the templates
            foreach (HtmlApi.IHTMLElement element in elements)
            {
                if (string.Compare(element.ID, "suresize", StringComparison.OrdinalIgnoreCase) == 0)
                {
                     ProcessSureSizeElement(element);
                }
            }

            // Do all labels
            foreach (HtmlApi.IHTMLElement element in elements)
            {
                if (element.ID == "swlabel")
                {
                    ProcessSureSizeElement(element);
                }
            }

            log.DebugFormat("SureSize Loop: " + sw.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// Do SureSize on the given element
        /// </summary>
        private static void ProcessSureSizeElement(HtmlApi.IHTMLElement element)
        {
            // Get the style element
            HtmlApi.IHTMLStyle mainStyle = element.Style;
            HtmlApi.IHTMLStyle3 zoomStyle = (HtmlApi.IHTMLStyle3) mainStyle;

            // Start off assuming 100% will work
            zoomStyle.SetZoom("100%");

            // These are required for GetClientWidth and GetClientHeight to work.  Has to be done, even if its already set.
            mainStyle.SetWidth("100%");
            mainStyle.SetHeight("100%");

            // This is required fo thet GetScrollXXX to work
            mainStyle.SetOverflow("hidden");
           
            HtmlApi.IHTMLElement2 element2 = (HtmlApi.IHTMLElement2) element;
            double xRatio = (double) element2.GetClientWidth() / (double) element2.GetScrollWidth();
            double yRatio = (double) element2.GetClientHeight() / (double) element2.GetScrollHeight();

            // See if we need to zoom
            if (xRatio < 1 || yRatio < 1)
            {
                double currentZoom = 100;

                // See what zoom would for sure get us there
                double idealZoom = Math.Min(xRatio, yRatio) * 100.0;

                int iteration = 0;
                int maxIterations = 5;

                // We incrementally get to the desired zoom level.   We could go directly to one we know
                // works, but its possible to overshoot, expecially in the case where a div just has text.                      
                while ((xRatio < 1 || yRatio < 1) && (++iteration <= maxIterations))
                {
                    currentZoom = idealZoom + (currentZoom - idealZoom) / 2.0;

                    // Update the zoom level of this label
                    zoomStyle.SetZoom(string.Format("{0}%", (int) Math.Floor(currentZoom)));

                    // Recalculate the ratios
                    xRatio = (double) element2.GetClientWidth() / (double) element2.GetScrollWidth();
                    yRatio = (double) element2.GetClientHeight() / (double) element2.GetScrollHeight();
                }

                // If we stil didnt reach a good zoom level, use the ideal
                if (xRatio < 1 || yRatio < 1)
                {
                    zoomStyle.SetZoom(string.Format("{0}%", (int) Math.Floor(idealZoom)));
                }
            }
        }
    }
}
