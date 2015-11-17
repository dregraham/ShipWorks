using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using System.Web;
using ShipWorks.Templates.Media;
using System.Diagnostics;
using ShipWorks.UI.Controls.Html;
using System.IO;
using Interapptive.Shared;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Formats template results into various outputs
    /// </summary>
    public static class TemplateResultFormatter
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateResultFormatter));

        #region Resources

        static string htmlErrorFormat = @"
            <html>

            <head>
	            <style>
                    span {{font-family: Tahoma, Arial; font-size: 10pt; }}
                    .major {{color: #FF0000; font-weight: bold;}}
                    .minor {{color: #777777; }}
	            </style>
            </head>

            <body>
                <ZOOMDIV>
                    <table style='margin: 0 15% 0 5%;'><tr><td style='height: 100%; vertical-align: middle;'>
	                    <span class='major'>{0}</span><br />
                        {1}
                        <span class='minor'>{2}</span> 
                        <span class='minor'>{3}</span>
                    </td></tr></table>
                </div>
            </body>

            </html>
            ";

        const string previewPageBreak = "<br><br><div style='text-align: center; width: 100%; font: normal 8pt verdana; border-top: 1px dotted #888888; padding-top: 3px; color: #888888;'>Page Break</div><br>";
        const string printPageBreak = "<div style='page-break-after:always; font-size:0%;' >&nbsp;</div>";

        #endregion

        /// <summary>
        /// Static constructor
        /// </summary>
        static TemplateResultFormatter()
        {
            htmlErrorFormat = htmlErrorFormat.Replace("<ZOOMDIV>", HtmlControl.ZoomDivStartTag);
        }

        /// <summary>
        /// Format the given result into HTML suitable for using as print output. The first result use
        /// is that given by resultIndex, and when the function resturns resultIndex is set to the next result that would be used.
        /// </summary>
        public static string FormatHtml(IList<TemplateResult> results, TemplateResultUsage usage, TemplateResultFormatSettings settings)
        {
            if (settings.TemplateType == TemplateType.Thermal)
            {
                return TemplateHelper.ThermalTemplateDisplayHtml;
            }

            // Due to the selected context having no input (like if you selected an Order, and used a template that's context was Shipment, and there
            // were no shipments for the order) there are no results to process.  This is an error.  We don't want blank template content being
            // emailed or printed or whatever.
            if (results.Count == 0 && settings.TemplateType != TemplateType.Report)
            {
                return FormatError(new TemplateException(TemplateHelper.NoResultsErrorMessage), TemplateOutputFormat.Html);
            }

            if (settings.TemplateType == TemplateType.Label)
            {
                return TemplateLabelMaker.FormatLabelHtml(results, usage, settings);
            }
            else
            {
                return FormatStandardHtml(results, usage, settings);
            }
        }

        /// <summary>
        /// Format the given error using the specified output format.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static string FormatError(TemplateException ex, TemplateOutputFormat format)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            int lineNumber = 0;
            int linePosition = 0;

            TemplateXslException xslEx = ex as TemplateXslException;
            if (xslEx != null)
            {
                lineNumber = xslEx.LineNumber;
                linePosition = xslEx.LinePosition;
            }

            switch (format)
            {
                case TemplateOutputFormat.Html:
                    {
                        string line = "";
                        string position = "";
                        string source = "";

                        if (ex.Source != null)
                        {
                            source = string.Format("<br /><span class='minor'>Source: {0}</span><br />", ex.Source);
                        }

                        if (lineNumber > 0 || linePosition > 0)
                        {
                            line = string.Format("Line: {0} ,", lineNumber);
                            position = string.Format("Position: {0}", linePosition);
                        }

                        return string.Format(htmlErrorFormat, ex.Message.Replace("\r", "").Replace("\n", "<br />"), source, line, position);
                    }

                case TemplateOutputFormat.Xml:
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<ShipWorks>");
                        sb.Append("<Error>");
                        sb.AppendFormat("<Message>{0}</Message>", ex.Message);

                        if (ex.Source != null)
                        {
                            sb.AppendFormat("<Source>{0}</Source>", ex.Source);
                        }

                        if (lineNumber > 0 || linePosition > 0)
                        {
                            sb.AppendFormat("<Line>{0}</Line><Position>{1}</Position>", lineNumber, linePosition);
                        }

                        sb.Append("</Error>");
                        sb.Append("</ShipWorks>");

                        return sb.ToString();
                    }

                case TemplateOutputFormat.Text:
                    {
                        string message = ex.Message;

                        if (ex.Source != null)
                        {
                            message += string.Format("\n\nSource: {0}", ex.Source);
                        }

                        if (lineNumber > 0 || linePosition > 0)
                        {
                            message += string.Format(" (Line {0}, Position {1})", lineNumber, linePosition);
                        }

                        return message;
                    }
            }

            throw new InvalidOperationException(string.Format("Invalid TemplateOutputFormat {0}", format));
        }

        /// <summary>
        /// Format the given results as a single html output, using the specified page break as the break between result pages.  The first result use
        /// is that given by resultIndex, and when the function resturns resultIndex is set to the next result that would be used.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static string FormatStandardHtml(IList<TemplateResult> results, TemplateResultUsage usage, TemplateResultFormatSettings settings)
        {
            int startIndex = settings.NextResultIndex;

            // Determine the page break to use
            string pageBreak = (usage == TemplateResultUsage.ShipWorksDisplay || usage == TemplateResultUsage.Copy) ? previewPageBreak : printPageBreak;

            // By default, our only limit will be the memory limit.  If showing as some type of preview, it will limit the result count as well
            int maxResultsToUse = int.MaxValue;
            
            switch (usage)
            {
                // For previewing in IE or shipworks, limit to the preview max
                case TemplateResultUsage.PrintPreview: 
                case TemplateResultUsage.ShipWorksDisplay:
                case TemplateResultUsage.Copy:
                    maxResultsToUse = TemplateHelper.MaxResultsForPreview;
                    break;

                // For saving and emailing we want one file per item, so limit to 1
                case TemplateResultUsage.Save:
                case TemplateResultUsage.Email:
                    maxResultsToUse = 1;
                    break;
            }

            using (StringWriter writer = new StringWriter())
            {
                if (settings.OutputFormat == TemplateOutputFormat.Html)
                {
                    TemplateHtmlDocument startHtmlDocument = new TemplateHtmlDocument(results[startIndex].ReadResult(), usage);

                    writer.Write(startHtmlDocument.HtmlStartTag);
                    writer.Write(startHtmlDocument.HeadCompleteTag);
                    writer.Write(startHtmlDocument.BodyStartTag);
                }
                else
                {
                    writer.Write("<html><body>");
                }

                // This is to make it easier for zooming when interacting in a preview.
                if (usage == TemplateResultUsage.ShipWorksDisplay || usage == TemplateResultUsage.Copy)
                {
                    writer.Write(HtmlControl.ZoomDivStartTag);
                }

                // Output all the body content
                while (settings.NextResultIndex < results.Count &&
                       settings.NextResultIndex - startIndex < maxResultsToUse)
                {
                    // If this is not the first one, we need a page break
                    if (startIndex != settings.NextResultIndex)
                    {
                        writer.Write(pageBreak);
                    }

                    string xslResult = results[settings.NextResultIndex].ReadResult();
                    writer.Write(EncodeForHtml(xslResult, settings.OutputFormat));

                    settings.NextResultIndex++;

                    // If we've exceeded the max space we are supposed to use, then stop using more results for now
                    if (writer.GetStringBuilder().Length >= TemplateHelper.MaxMemoryForHtml)
                    {
                        break;
                    }
                }

                if (usage == TemplateResultUsage.ShipWorksDisplay || usage == TemplateResultUsage.Copy)
                {
                    // Close the zoom div.
                    writer.Write("</div>");

                    // See if its cutoff
                    if (settings.NextResultIndex != results.Count)
                    {
                        // This div id is set to not showup on actual print outs
                        writer.Write("<div id='sw_html_cutoff'>");
                        writer.Write(pageBreak);

                        writer.Write(string.Format("Only the the first {0} results are shown.", settings.NextResultIndex - startIndex));

                        // Close the cutoff display
                        writer.Write("</div>");
                    }
                }

                // Close the html
                writer.Write("</body></html>");

                return writer.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Encode the specified content so its ready to be used as html content.  This does nothing if format is Html.
        /// </summary>
        public static string EncodeForHtml(string content, TemplateOutputFormat format)
        {
            if (format == TemplateOutputFormat.Html)
            {
                // Since we are only grabbing the innerHtml, usage doesnt really matter
                TemplateHtmlDocument htmlDocument = new TemplateHtmlDocument(content, TemplateResultUsage.ShipWorksDisplay);
                return htmlDocument.BodyInnerHtml;
            }
            else
            {
                // XML and text needs escaped so the < > are visible, and not interpreted as tags
                content = HttpUtility.HtmlEncode(content);

                // Wrap text in a <pre> tag for output
                content = string.Format("<pre style=\"font: 10pt 'Courier New';\">{0}</pre>", content);

                return content;
            }
        }
    }
}
