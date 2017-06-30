using System.Drawing;
using System.IO;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for generation of template labels
    /// </summary>
    [Component]
    public class TemplateLabelUtilityWrapper : ITemplateLabelUtility
    {
        /// <summary>
        /// Loads the image from disk
        /// </summary>
        public Image LoadImageFromResouceDirectory(string fileNameWithoutPath)
        {
            return TemplateLabelUtility.LoadImageFromDisk(Path.Combine(DataPath.CurrentResources, fileNameWithoutPath));
        }
    }
}
