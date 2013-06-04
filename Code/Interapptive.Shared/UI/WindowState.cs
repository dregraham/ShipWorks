using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Represents the state of a single window
    /// </summary>
    public class WindowState
    {
        Dictionary<string, int> splitterDistances = new Dictionary<string, int>();

        /// <summary>
        /// The name of the window. Could be the window Text, or just any name to save the state as.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The Min\Max\Restored state of the window
        /// </summary>
        public FormWindowState FormState
        {
            get;
            set;
        }

        /// <summary>
        /// The bounds of the window
        /// </summary>
        public Rectangle Bounds
        {
            get;
            set;
        }

        /// <summary>
        /// Maps a splitter name to the distance if the splitter
        /// </summary>
        public Dictionary<string, int> SplitterDistances
        {
            get { return splitterDistances; }
        }
    }
}
