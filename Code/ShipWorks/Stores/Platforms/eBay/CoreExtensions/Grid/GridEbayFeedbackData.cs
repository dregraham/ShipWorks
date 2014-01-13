using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Properties;
using System.Drawing;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid
{
    /// <summary>
    /// Feedback data container for use by grid display columns
    /// </summary>
    public class GridEbayFeedbackData
    {
        GridEbayFeedbackDirection direction;
        EbayFeedbackType? feedbackType;
        string comments;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridEbayFeedbackData(GridEbayFeedbackDirection direction, EbayFeedbackType? type, string comments)
        {
            this.direction = direction;
            this.feedbackType = type;
            this.comments = comments;
        }

        /// <summary>
        /// Indiciates who feedback was left for or received by the buyer
        /// </summary>
        public GridEbayFeedbackDirection Direction
        {
            get { return direction; }
        }

        /// <summary>
        /// Type of feedback
        /// </summary>
        public EbayFeedbackType? FeedbackType
        {
            get { return feedbackType; }
        }

        /// <summary>
        /// Comments associated with the feedback
        /// </summary>
        public string Comments
        {
            get { return comments; }
        }

        /// <summary>
        /// Gets the comment to display in the grid based on the feedback data
        /// </summary>
        public string GetCommentValue()
        {
            return comments;
        }

        /// <summary>
        /// Get the image to display in a grid column based on the feedback data
        /// </summary>
        public Image GetColumnDisplayImage()
        {
            if (feedbackType == null)
            {
                return null;
            }

            switch (feedbackType)
            {
                case EbayFeedbackType.Positive: return Resources.add16_2;
                case EbayFeedbackType.Negative: return Resources.forbidden1;
                case EbayFeedbackType.Neutral: return Resources.shape_circle;
            }

            return Resources.blank16;
        }
    }
}
