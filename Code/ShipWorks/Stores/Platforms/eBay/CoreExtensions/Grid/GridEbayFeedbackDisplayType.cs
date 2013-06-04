using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.Properties;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Data.Grid.Columns.DisplayTypes;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid
{
    /// <summary>
    /// Provides the feedback image and text for the eBay feedback columns
    /// </summary>
    public class GridEbayFeedbackDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridEbayFeedbackDisplayType()
        {

        }

        /// <summary>
        /// Returns Feedback text for display in the grid
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            GridEbayFeedbackData feedbackData = (GridEbayFeedbackData) value;

            if (feedbackData != null)
            {
                return feedbackData.GetCommentValue();
            }

            return "";
        }

        /// <summary>
        /// Get the image for the note column
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            GridEbayFeedbackData feedbackData = (GridEbayFeedbackData) value;

            if (feedbackData != null)
            {
                return feedbackData.GetColumnDisplayImage();
            }

            return null;
        }
    }
}
