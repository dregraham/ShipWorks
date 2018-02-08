using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Represents the Download Settings Control 
    /// </summary>
    public interface IDownloadSettingsControl
    {
        /// <summary>
        /// The controls Location
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// The controls Width
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// The controls Anchor 
        /// </summary>
        AnchorStyles Anchor { get; set; }

        /// <summary>
        /// Save the donwload settings
        /// </summary>
        void Save();
        
        /// <summary>
        /// Load the store into the control
        /// </summary>
        void LoadStore(StoreEntity store);
    }
}