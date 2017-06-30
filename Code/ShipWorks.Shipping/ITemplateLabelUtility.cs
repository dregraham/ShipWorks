using System.Drawing;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for generation of template labels
    /// </summary>
    public interface ITemplateLabelUtility
    {
        /// <summary>
        /// Loads the image from disk
        /// </summary>
        Image LoadImageFromResouceDirectory(string fullFilenameAndPath);
    }
}