namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Details for uploading a comment to NetworkSolutions
    /// </summary>
    public class NetworkSolutionsCommentDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsCommentDetails(long code, string comments)
        {
            Code = code;
            Comments = comments;
        }

        /// <summary>
        /// Code that should be uploaded
        /// </summary>
        public long Code { get; }

        /// <summary>
        /// Comment to send with the upload
        /// </summary>
        public string Comments { get; }
    }
}