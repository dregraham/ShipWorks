using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ShipWorks.Templates.Media;
using ShipWorks.Data.Model.EntityClasses;
using System.Reflection;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Templates.Printing
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class BrowserPageSettings
    {
        double pageHeight = 11;
        double pageWidth = 8.5;

        double marginTop = 0;
        double marginBottom = 0;
        double marginLeft = 0;
        double marginRight = 0;

        /// <summary>
        /// Initialize using settings from the specified label settings label sheet
        /// </summary>
        internal BrowserPageSettings(PrintJobLabelSettings labelSettings, PrinterCalibration calibration)
        {
            LabelSheetEntity labelSheet = LabelSheetManager.GetLabelSheet(labelSettings.LabelSheetID);
            if (labelSheet != null)
            {
                pageHeight = labelSheet.PaperSizeHeight;
                pageWidth = labelSheet.PaperSizeWidth;

                // Portrait
                if (pageHeight > pageWidth)
                {
                    marginLeft = (double) (calibration.XOffset / 10m);
                    marginTop = (double) (-calibration.YOffset / 10m);
                }
                // Landscape
                else
                {
                    marginTop = (double) (-calibration.XOffset / 10m);
                    marginLeft = (double) (-calibration.YOffset / 10m);
                }
            }
        }

        /// <summary>
        /// Initialize using settings from the specified template page settings
        /// </summary>
        internal BrowserPageSettings(PrintJobPageSettings pageSettings)
        {
            pageHeight = pageSettings.PageHeight;
            pageWidth = pageSettings.PageWidth;

            marginTop = pageSettings.MarginTop;
            marginBottom = pageSettings.MarginBottom;
            marginLeft = pageSettings.MarginLeft;
            marginRight = pageSettings.MarginRight;
        }

        /// <summary>
        /// Initialize width and height with zero margins
        /// </summary>
        internal BrowserPageSettings(double pageWidth, double pageHeight)
        {
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
        }

        /// <summary>
        /// Height of the page to print
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double PageHeight
        {
            get { return pageHeight; }
        }

        /// <summary>
        /// Width of the page to print
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double PageWidth
        {
            get { return pageWidth; }
        }

        /// <summary>
        /// The top margin of the printed page.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double MarginTop
        {
            get { return marginTop; }
        }

        /// <summary>
        /// The bottom margin of the printed page.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double MarginBottom
        {
            get { return marginBottom; }
        }

        /// <summary>
        /// The left margin of the printed page.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double MarginLeft
        {
            get { return marginLeft; }
        }

        /// <summary>
        /// The right margin of the printed page.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double MarginRight
        {
            get { return marginRight; }
        }

        /// <summary>
        /// True if printed in portrait, landscape if false
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsPortrait
        {
            get { return (PageHeight > PageWidth); }
        }

        /// <summary>
        /// Indicates 'portrait' or 'landscape'
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Orientation
        {
            get { return IsPortrait ? "portrait" : "landscape"; }
        }
    }
}
