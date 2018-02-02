using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Represents the state of a single window
    /// </summary>
    public class WindowState
    {
        /// <summary>
        /// The name of the window. Could be the window Text, or just any name to save the state as.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Min\Max\Restored state of the window
        /// </summary>
        public FormWindowState FormState { get; set; }

        /// <summary>
        /// The bounds of the window
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// The bounds of the window
        /// </summary>
        public Rect BoundsWpf { get; set; }

        /// <summary>
        /// Maps a splitter name to the distance if the splitter
        /// </summary>
        public IDictionary<string, int> SplitterDistances { get; set; } = new Dictionary<string, int>();
    }
}
