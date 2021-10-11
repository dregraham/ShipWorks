using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// UserControl for editing paper dimensions
    /// </summary>
    public partial class PaperDimensionsControl : UserControl
    {
        bool changingPaperDimensionValues;

        /// <summary>
        /// Constructor
        /// </summary>
        public PaperDimensionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            paperDimensions.DisplayMember = "Description";
            paperDimensions.DataSource = PaperDimensions.SupportedDimensions;
        }

        /// <summary>
        /// The selected paper width
        /// </summary>
        public double PaperWidth
        {
            get { return (double) paperWidth.Value; }
            set { paperWidth.Value = (decimal) value; }
        }

        /// <summary>
        /// The selected paper height
        /// </summary>
        public double PaperHeight
        {
            get { return (double) paperHeight.Value; }
            set { paperHeight.Value = (decimal) value; }
        }

        /// <summary>
        /// The selected paper details is changing.
        /// </summary>
        private void OnChangePaperDimensions(object sender, System.EventArgs e)
        {
            PaperDimensions dimensions = (PaperDimensions) paperDimensions.SelectedItem;

            if (!dimensions.IsCustom)
            {
                changingPaperDimensionValues = true;

                // Set the width\height of the details
                paperHeight.Value = (decimal) dimensions.Height;
                paperWidth.Value = (decimal) dimensions.Width;

                changingPaperDimensionValues = false;
            }
        }

        /// <summary>
        /// When the dimensions are changed, update the Combo to match
        /// </summary>
        private void OnChangePaperDimensionValue(object sender, System.EventArgs e)
        {
            // OnLoad needs to have been called at this point
            if (!IsHandleCreated)
            {
                CreateHandle();
            }

            if (changingPaperDimensionValues)
            {
                return;
            }

            paperDimensions.SelectedItem = PaperDimensions.FromDimensions((double) paperWidth.Value, (double) paperHeight.Value);
        }
    }
}
