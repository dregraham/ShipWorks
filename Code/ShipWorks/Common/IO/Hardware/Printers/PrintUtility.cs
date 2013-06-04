using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using ShipWorks.UI;
using System.Drawing;
using Interapptive.Shared.UI;
using System.ComponentModel;
using log4net;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Utility class for printing documents
    /// </summary>
    public static class PrintUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PrintUtility));

        /// <summary>
        /// Wrapper around PrinterSettings.InstalledPrinters that throws a PrintingException with an informative message on failure
        /// </summary>
        public static List<string> InstalledPrinters
        {
            get
            {
                try
                {
                    return PrinterSettings.InstalledPrinters.Cast<string>().ToList();
                }
                catch (Win32Exception ex)
                {
                    log.Warn("Failed calling PrinterSettings.InstalledPrinters", ex);

                    string message = ex.Message;

                    if (message.Contains("RPC server"))
                    {
                        message = "There was an error finding your installed printers.  This can be caused by the 'Print Spooler' service being stopped or a corrupted printer driver.";
                    }

                    throw new PrintingException(message, ex);
                }
            }
        }

        /// <summary>
        /// Print the given content in the specified orientation.  The user is prompted for the printer settings to use.
        /// </summary>
        public static void PrintText(IWin32Window owner, string name, string content, bool portrait)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.UseEXDialog = true;

            PrintDocument printDocument = new PrintDocument();

            printDialog.AllowPrintToFile = false;
            printDialog.Document = printDocument;

            printDocument.DocumentName = name;
            printDocument.DefaultPageSettings.Landscape = !portrait;

            // Show the print dialog first
            if (printDialog.ShowDialog(owner) != DialogResult.OK)
            {
                return;
            }

            StringBuilder stillToPrint = new StringBuilder(content);

            printDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
            {
                PrintPageText(stillToPrint, e);
            };

            DoPrint(owner, printDocument);
        }

        /// <summary>
        /// Print the next page of content
        /// </summary>
        private static void PrintPageText(StringBuilder content, PrintPageEventArgs e)
        {
            using (Font font = new Font("Times New Roman", 10))
            {
                int charsFitted;
                int linesFilled;

                StringFormat stringFormat = new StringFormat(StringFormatFlags.LineLimit);
                stringFormat.Trimming = StringTrimming.Word;

                // Measure
                e.Graphics.MeasureString(content.ToString(), font, e.MarginBounds.Size, stringFormat, out charsFitted, out linesFilled);

                // Extract the content of this page
                string pageContent = content.ToString().Substring(0, charsFitted);

                // Remove that much content from what's to be printed
                content.Remove(0, charsFitted);

                e.Graphics.DrawString(
                    pageContent,
                    font,
                    Brushes.Black,
                    e.MarginBounds,
                    stringFormat);

                // If more lines exist, print another page.
                e.HasMorePages = content.Length > 0;
            }
        }

        /// <summary>
        /// Print the given image in the specified orientation.  The user is prompted for the printer settings to use.
        /// </summary>
        public static bool PrintImage(IWin32Window owner, string name, Image image, bool portrait, bool ignoreMargins)
        {
            List<Image> images = new List<Image> { image };
            return PrintImages(owner, name, images, portrait, ignoreMargins);
        }

        /// <summary>
        /// Print the given images in the specified orientation.  The user is prompted for the printer settings to use.
        /// </summary>
        public static bool PrintImages(IWin32Window owner, string name, List<Image> images, bool portrait, bool ignoreMargins)
        {
            using (PrintDialog printDialog = new PrintDialog { UseEXDialog = true, AllowPrintToFile = false })
            {
                using (PrintDocument printDocument = new PrintDocument())
                {
                    printDialog.Document = printDocument;

                    printDocument.DocumentName = name;
                    printDocument.DefaultPageSettings.Landscape = !portrait;

                    // Show the print dialog first
                    if (printDialog.ShowDialog(owner) != DialogResult.OK)
                    {
                        return false;
                    }

                    int currentImageIndex = 0;

                    printDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                    {
                        Image currentImage = images[currentImageIndex];
                        Rectangle bounds = ignoreMargins ? e.PageBounds : e.MarginBounds;

                        ScaleImage(bounds, currentImage, e);

                        // Prepare to advance to the next page and see if there are any more pages to print
                        currentImageIndex++;
                        e.HasMorePages = currentImageIndex < images.Count;
                    };

                    DoPrint(owner, printDocument);

                    return true;
                }
            }
        }

        /// <summary>
        /// Scales the specified image to fit within the given rectangle.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="image">The image.</param>
        /// <param name="e">The <see cref="PrintPageEventArgs" /> instance containing the event data.</param>
        private static void ScaleImage(Rectangle bounds, Image image, PrintPageEventArgs e)
        {
            // If the image fits in our space, just draw it at the upper-left corner without changing it's size
            if (bounds.Width > image.Width && bounds.Height > image.Height)
            {
                e.Graphics.DrawImage(image, bounds.Location);
            }
            // Otherwise we scale it
            else
            {
                double scale = 1.0;

                if (image.Width > bounds.Width)
                {
                    scale = bounds.Width/(double) image.Width;
                }

                if (image.Height > bounds.Height)
                {
                    scale = Math.Min(scale, bounds.Height/(double) image.Height);
                }

                Rectangle scaledBounds = new Rectangle(bounds.Left, bounds.Top, (int) (image.Width*scale), (int) (image.Height*scale));

                e.Graphics.DrawImage(image, scaledBounds);
            }
        }

        /// <summary>
        /// Do the actual printing of the document, handling errors
        /// </summary>
        private static void DoPrint(IWin32Window owner, PrintDocument printDocument)
        {
            try
            {
                printDocument.Print();
            }
            catch (InvalidPrinterException ex)
            {
                MessageHelper.ShowError(owner, "There was a problem printing the document: " + ex.Message);
            }
            catch (Win32Exception ex)
            {
                string message = ex.Message;

                if (ex.NativeErrorCode == 1722)
                {
                    message = "The printer could not be found.";
                }

                MessageHelper.ShowError(owner, "There was a problem printing the document: " + message);
            }
        }
    }
}
