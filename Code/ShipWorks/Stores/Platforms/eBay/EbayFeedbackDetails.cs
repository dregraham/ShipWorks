using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Details for leaving feedback
    /// </summary>
    public class EbayFeedbackDetails
    {
        /// <summary>
        /// Type of feedback being left
        /// </summary>
        public CommentTypeCodeType FeedbackType { get; set; }

        /// <summary>
        /// Feedback contents
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// If the user selected a single item to leave feedback for, this is the key
        /// </summary>
        public long SelectedItemID { get; set; }
    }
}