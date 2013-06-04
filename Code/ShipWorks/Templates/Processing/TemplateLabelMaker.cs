using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.Html;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Media;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Responsible for generating html label sheets from template results
    /// </summary>
    public static class TemplateLabelMaker
    {
        /// <summary>
        /// Format the html that can be used by the preview control for the given results for the sheet and format
        /// </summary>
        public static string FormatLabelHtml(IList<TemplateResult> results, TemplateResultUsage usage, TemplateResultFormatSettings settings)
        {
            if (settings.LabelSheet == null)
            {
                throw new TemplateException("The label sheet used by the template has been deleted.");
            }

            if (usage == TemplateResultUsage.ShipWorksDisplay)
            {
                return FormatShipWorksDisplayHtml(results, settings); 
            }
            else
            {
                return FormatOutputHtml(results, usage, settings);
            }
        }

        /// <summary>
        /// Format labels for use as display within shipworks
        /// </summary>
        private static string FormatShipWorksDisplayHtml(IList<TemplateResult> results, TemplateResultFormatSettings settings)
        {
            // Create a new blank html page.
            StringBuilder htmlBuilder = new StringBuilder();
            TemplateHtmlDocument htmlDocument = new TemplateHtmlDocument(results[0].ReadResult(), TemplateResultUsage.ShipWorksDisplay);

            // Add the <html> and <head> sections.
            htmlBuilder.Append(htmlDocument.HtmlStartTag);
            htmlBuilder.Append(htmlDocument.HeadCompleteTag);

            // <body>
            htmlBuilder.Append("<body bgcolor='#CCDEFA' style='margin-top:0; margin-left:0; margin-right:0; margin-bottom:0; '> \n");

            // This is to make it easier for zooming when interacting in a preview.
            htmlBuilder.Append(HtmlControl.ZoomDivStartTag);

            // Add the margins for the preview
            htmlBuilder.Append(@"
                   <table style='width: 100%; height: 100%; padding-top:.1in; padding-left:.1in; padding-right:.1in; padding-bottom:.1in;' cellSpacing=0 cellPadding=0 border=0>
                      <tr>
                        <td vAlign=center align=middle>
            ");

            List<string[]> pages = GenerateLabelSheetPages(results, settings, TemplateHelper.MaxResultsForPreview);

            // Add in all the pages
            for (int i = 0; i < pages.Count; i++)
            {
                AddPreviewPage(htmlBuilder, settings.LabelSheet, pages[i], i);

                if (i != pages.Count - 1)
                {
                    htmlBuilder.Append("<br><br>");
                }
            }

            // Close off the margins
            htmlBuilder.Append(@"
                         </td>
                      </tr>
                   </table>
                ");

            // Close the zoom div.
            htmlBuilder.Append("</div>");

            // See if its cutoff
            if (settings.NextResultIndex != results.Count)
            {
                AppendResultsTruncatedMessage(htmlBuilder, pages.Count);
            }

            // </body></html>
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");

            return htmlBuilder.ToString();
        }

        /// <summary>
        /// Format the html that can be used for outputing the label result
        /// </summary>
        private static string FormatOutputHtml(IList<TemplateResult> results, TemplateResultUsage usage, TemplateResultFormatSettings settings)
        {
            // Create a new blank html page.
            StringBuilder htmlBuilder = new StringBuilder();

            TemplateHtmlDocument htmlDocument = new TemplateHtmlDocument(results[0].ReadResult(), usage);
            LabelSheetEntity labelSheet = settings.LabelSheet;

            StringBuilder style = new StringBuilder();
            style.Append("   body {margin:0; padding:0; background-color:white; }\n");
            style.Append("   div.label\n   {\n");
            style.Append("      border-style: none;\n");
            style.Append("      padding:0; margin:0; position:absolute; overflow:hidden;\n");
            style.Append("      width: " + labelSheet.LabelWidth + "in;\n");
            style.Append("      height: " + labelSheet.LabelHeight + "in;\n");
            style.Append("   }\n");

            // Add the extra styling
            htmlDocument.ExtraCss = style.ToString();

            // Add the <html> and <head> sections.
            htmlBuilder.Append(htmlDocument.HtmlStartTag);
            htmlBuilder.Append(htmlDocument.HeadCompleteTag);

            // <body>
            htmlBuilder.Append("<body> \n");

            // By default we use all the results up to the memory limit
            int maxResultsToUse = int.MaxValue;
            
            // For the preview window we keep it more reasonable
            if (usage == TemplateResultUsage.PrintPreview)
            {
                maxResultsToUse = TemplateHelper.MaxResultsForPreview;
            }

            // Generate the pages that will be used to create the content
            List<string[]> pages = GenerateLabelSheetPages(results, settings, maxResultsToUse);

            // This div doesn't get seen at all - but its necessary to make our print template for preview work.  We need it so that
            // the document flows normally all the way to the end.  The entirely absolute positioned contents were not working right.  We
            // were getting onlayoutcomplete events raised with overflow=false for pages that were not the last page.
            if (usage == TemplateResultUsage.PrintPreview)
            {
                htmlBuilder.AppendFormat("<div style='height: {0}in;'>&nbsp;</div>", pages.Count * labelSheet.PaperSizeHeight);
            }

            // Add in all the pages
            for (int i = 0; i < pages.Count; i++)
            {
                AddPrintPage(htmlBuilder, labelSheet, pages[i], i);
            }

            // </body></html>
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");

            return htmlBuilder.ToString();
        }

        /// <summary>
        /// Generate the list of pages with contents of each cell.  The first cell to have content is listed, and is 1 based, going from left-to-right, up-to-down.
        /// </summary>
        private static List<string[]> GenerateLabelSheetPages(
            IList<TemplateResult> results, 
            TemplateResultFormatSettings settings, 
            int maxLabelCells)
        {
            List<string[]> pages = new List<string[]>();

            LabelSheetEntity labelSheet = settings.LabelSheet;

            // Ensure row\column values are in range
            int startRow = Math.Min(Math.Max(settings.LabelPosition.Row, 1), labelSheet.Rows);
            int startColumn = Math.Min(Math.Max(settings.LabelPosition.Column, 1), labelSheet.Columns);

            // Cell to start in (1 based)
            int startCell = ((startRow - 1) * labelSheet.Columns) + startColumn;

            // Reset positioning for next time
            settings.LabelPosition = new LabelPosition(1, 1);

            // Number of cells per page
            int cellsPerPage = labelSheet.Rows * labelSheet.Columns;

            int startIndex = settings.NextResultIndex;
            long totalMemoryUsed = 0;

            // Build the pages
            while (settings.NextResultIndex < results.Count && 
                   settings.NextResultIndex - startIndex < maxLabelCells &&
                   totalMemoryUsed < TemplateHelper.MaxMemoryForHtml)
            {
                string[] pageCells = new string[cellsPerPage];

                // Fill in each cell on the page
                for (int cell = 1; cell <= cellsPerPage; cell++)
                {
                    // If we are passed the initial starting cell and there are more
                    // labels left, fill in the label content
                    if (cell >= startCell && settings.NextResultIndex < results.Count)
                    {
                        string xslResult = results[settings.NextResultIndex].ReadResult();
                        pageCells[cell - 1] = TemplateResultFormatter.EncodeForHtml(xslResult, settings.OutputFormat);

                        settings.NextResultIndex++;
                        totalMemoryUsed += pageCells[cell - 1].Length;
                    }
                    else
                    {
                        pageCells[cell - 1] = null;
                    }
                }

                // Add this page to our pages list
                pages.Add(pageCells);

                // Ensure the starting cell for all pages after the first is the first cell
                startCell = 1;
            }

            return pages;
        }

        /// <summary>
        /// The heart of the class, creates the tables with the labels cells in it
        /// </summary>
        private static void AddPreviewPage(StringBuilder htmlBuilder, LabelSheetEntity labelSheet, string[] labels, int page)
        {
            int previousCells = labelSheet.Columns * labelSheet.Rows * page;

            // Add the main table
            htmlBuilder.AppendFormat(@"
                     <table cellSpacing=0 cellPadding=0 bgColor='#ffffff'  
                            style='table-layout: fixed; 
                                   border: 0px #000022 dotted; 
                                   border-collapse: separate; 
                                   width: {0}in; 
                                   height: {1}in; 
                                  '  
                     >
                ", labelSheet.PaperSizeWidth, labelSheet.PaperSizeHeight);

            // Go through each row and colum in the table
            for (int row = 1; row <= labelSheet.Rows; row++)
            {
                // Add a new row
                htmlBuilder.AppendLine("    <tr>");

                // Go through each colum in the row
                for (int col = 1; col <= labelSheet.Columns; col++)
                {
                    int currentCell = previousCells + ((row - 1) * labelSheet.Columns) + col;

                    // Get the padding (this creates spacing between labels)
                    double leftPad = (col == 1) ? labelSheet.MarginLeft : labelSheet.HorizontalSpacing;
                    double topPad = (row == 1) ? labelSheet.MarginTop : labelSheet.VerticalSpacing;

                    double horizontal = labelSheet.MarginLeft + (labelSheet.LabelWidth * labelSheet.Columns) + (labelSheet.HorizontalSpacing * (labelSheet.Columns - 1));
                    double vertical = labelSheet.MarginTop + (labelSheet.LabelHeight * labelSheet.Rows) + (labelSheet.VerticalSpacing * (labelSheet.Rows - 1));

                    double rightPad = (col == labelSheet.Columns) ? labelSheet.PaperSizeWidth - horizontal : 0;
                    double bottomPad = (row == labelSheet.Rows) ? labelSheet.PaperSizeHeight - vertical : 0;

                    // Insert the TD tag
                    htmlBuilder.AppendFormat(string.Format(@"
                        <td border=0 
                            style='border-style: none; 
                                   padding-left: {0}in; 
                                   padding-top: {1}in; 
                                   padding-right: {2}in; 
                                   padding-bottom: {3}in; 
                                   width: {4}in; 
                                   height: {5}in; 
                                  '
                        >",
                        leftPad, topPad, rightPad, bottomPad, (labelSheet.LabelWidth + leftPad + rightPad), (labelSheet.LabelHeight + topPad + bottomPad)
                    ));

                    // Insert the inner label div, that will show the label border.
                    htmlBuilder.AppendFormat("           <div id=cell{0} vAlign=Top border=0 style='border: 1px #BBBBDD dotted; width:100%; height:100%; overflow:hidden;' >",
                        currentCell);
                    htmlBuilder.AppendLine();

                    int cellIndex = ((row - 1) * labelSheet.Columns) + col - 1;
                    string cellContent = labels[cellIndex];

                    // We use another DIV that will actual control the zoom for the shrink to fit.  This makes it so the zoom does not 
                    // apply to the outer DIV, the one with the border, and thus does not screw up the border.
                    htmlBuilder.AppendFormat("           <div {0} >", cellContent != null ? "id='swlabel'" : "" );
                    htmlBuilder.AppendLine();

                    // Add the label contents
                    htmlBuilder.Append(cellContent ?? "&nbsp;");

                    // Close off that inner zoom div
                    htmlBuilder.AppendLine("                   </div>");

                    // Close the label div
                    htmlBuilder.AppendLine("          </div>");

                    // Close the TD tab
                    htmlBuilder.AppendLine("           </td>");
                }

                // Close the row
                htmlBuilder.AppendLine("    </tr>");
            }

            // Close the table
            htmlBuilder.AppendLine("    </table>");
        }

        /// <summary>
        /// Creates a page that will be used for the actual print job.
        /// </summary>
        private static void AddPrintPage(StringBuilder htmlBuilder, LabelSheetEntity labelSheet, string[] labels, int page)
        {
            // Go through each row and colum in the table
            for (int row = 1; row <= labelSheet.Rows; row++)
            {
                // Go through each colum in the row
                for (int col = 1; col <= labelSheet.Columns; col++)
                {
                    int cellIndex = ((row - 1) * labelSheet.Columns) + col - 1;
                    string cellContent = labels[cellIndex];

                    if (cellContent != null)
                    {
                        // Get the position of this label
                        double left = labelSheet.MarginLeft + (col - 1) * (labelSheet.HorizontalSpacing + labelSheet.LabelWidth);
                        double top = labelSheet.MarginTop + (row - 1) * (labelSheet.VerticalSpacing + labelSheet.LabelHeight);

                        // Offset by how many pages we have already printed
                        top += page * labelSheet.PaperSizeHeight;

                        // Insert the main div tag
                        htmlBuilder.Append("       <div class='label' vAlign=Top style='left: " + left + "in; top: " + top + "in;'>\n");

                        // We use another DIV that will actual control the zoom for the shrink to fit.  This makes it so the zoom does not 
                        // apply to the outer DIV, the one with the border, and thus does not screw up the border.
                        htmlBuilder.Append("           <div id='swlabel'>\n");

                        // Add the label contents
                        htmlBuilder.Append(cellContent);

                        // Close off that inner zoom div
                        htmlBuilder.Append("           </div>\n");

                        // Close the label div
                        htmlBuilder.Append("       </div>\n");
                    }
                }
            }
        }

        /// <summary>
        /// Append the message that lets the user know that some of the results were truncated
        /// </summary>
        private static void AppendResultsTruncatedMessage(StringBuilder htmlBuilder, int pagesCreated)
        {
            // This div id is set to not showup on actual print outs
            htmlBuilder.Append("<div id='sw_html_cutoff' style='color: white;'><br />");

            htmlBuilder.Append(string.Format("Only the the first {0} pages are shown.", pagesCreated));

            // Close the cutoff display
            htmlBuilder.Append("<br /><br /></div>");
        }
    }
}
