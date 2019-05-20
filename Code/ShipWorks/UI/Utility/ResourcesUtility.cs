using System.Drawing;
using ShipWorks.Properties;

namespace ShipWorks.UI.Utility
{
    /// <summary>
    /// Utility for dealing with resources
    /// </summary>
    public static class ResourcesUtility
    {
        /// <summary>
        /// Get an image from resources by its name
        /// </summary>
        public static Image GetImage(string name) =>
            (Bitmap) Resources.ResourceManager.GetObject(name, Resources.Culture);
    }
}
