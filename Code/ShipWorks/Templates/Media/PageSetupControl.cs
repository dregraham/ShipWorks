using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// UserControl for editing paper dimensions and margins for a template
    /// </summary>
    public partial class PageSetupControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PageSetupControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The selected paper width
        /// </summary>
        public double PaperWidth
        {
            get { return paperDimensions.PaperWidth; }
            set { paperDimensions.PaperWidth = value; }
        }

        /// <summary>
        /// The selected paper height
        /// </summary>
        public double PaperHeight
        {
            get { return paperDimensions.PaperHeight; }
            set { paperDimensions.PaperHeight = value; }
        }

        /// <summary>
        /// The left margin of the page
        /// </summary>
        public double MarginLeft
        {
            get { return (double) marginLeft.Value; }
            set { marginLeft.Value = (decimal) value; }
        }

        /// <summary>
        /// The right margin of the page
        /// </summary>
        public double MarginRight
        {
            get { return (double) marginRight.Value; }
            set { marginRight.Value = (decimal) value; }
        }

        /// <summary>
        /// The top margin of the page
        /// </summary>
        public double MarginTop
        {
            get { return (double) marginTop.Value; }
            set { marginTop.Value = (decimal) value; }
        }

        /// <summary>
        /// The bottom margin of the page
        /// </summary>
        public double MarginBottom
        {
            get { return (double) marginBottom.Value; }
            set { marginBottom.Value = (decimal) value; }
        }
    }
}
