using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Media;
using System.Runtime.InteropServices;
using Interapptive.Shared;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Represents the effective paper settings of a template
    /// </summary>
    public class PrintJobPageSettings
    {
        double pageHeight = 11;
        double pageWidth = 8.5;

        double marginTop = 1;
        double marginBottom = 1;
        double marginLeft = 1;
        double marginRight = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintJobPageSettings(TemplateEntity template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            if (template.Type == (int) TemplateType.Label)
            {
                throw new InvalidOperationException("This class is only intended to manage the non-label sheet settings of a template.");
            }

            pageHeight = template.PageHeight;
            pageWidth = template.PageWidth;

            marginTop = template.PageMarginTop;
            marginBottom = template.PageMarginBottom;
            marginLeft = template.PageMarginLeft;
            marginRight = template.PageMarginRight;
        }

        /// <summary>
        /// Contructor for manually creating the page settings values instead of pulling then from a template
        /// </summary>
        [NDependIgnoreTooManyParams]
        public PrintJobPageSettings(double pageHeight, double pageWidth, double marginTop, double marginRight, double marginBottom, double marginLeft)
        {
            this.pageHeight = pageHeight;
            this.pageWidth = pageWidth;

            this.marginTop = marginTop;
            this.marginBottom = marginBottom;
            this.marginLeft = marginLeft;
            this.marginRight = marginRight;
        }

        /// <summary>
        /// Height of the page to print
        /// </summary>
        public double PageHeight
        {
            get { return pageHeight; }
            set { pageHeight = value; }
        }

        /// <summary>
        /// Width of the page to print
        /// </summary>
        public double PageWidth
        {
            get { return pageWidth; }
            set { pageWidth = value; }
        }

        /// <summary>
        /// The top margin of the printed page.
        /// </summary>
        public double MarginTop
        {
            get { return marginTop; }
            set { marginTop = value; }
        }

        /// <summary>
        /// The bottom margin of the printed page.
        /// </summary>
        public double MarginBottom
        {
            get { return marginBottom; }
            set { marginBottom = value; }
        }

        /// <summary>
        /// The left margin of the printed page.
        /// </summary>
        public double MarginLeft
        {
            get { return marginLeft; }
            set { marginLeft = value; }
        }

        /// <summary>
        /// The right margin of the printed page.
        /// </summary>
        public double MarginRight
        {
            get { return marginRight; }
            set { marginRight = value; }
        }
    }
}
