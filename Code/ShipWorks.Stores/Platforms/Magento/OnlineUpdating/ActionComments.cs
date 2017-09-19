using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// Action and comments for uploading
    /// </summary>
    public class ActionComments
    {
        /// <summary>
        /// The code the user has selected
        /// </summary>
        public MagentoUploadCommand Action { get; set; }

        /// <summary>
        /// The user entered comments
        /// </summary>
        public string Comments { get; set; }
    }
}