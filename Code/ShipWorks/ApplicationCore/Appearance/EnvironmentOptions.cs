using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Represents the available options for environment settings
    /// </summary>
    public class EnvironmentOptions
    {
        bool windows;
        bool menus;
        bool columns;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnvironmentOptions(bool windows, bool menus, bool columns)
        {
            this.windows = windows;
            this.menus = menus;
            this.columns = columns;
        }

        /// <summary>
        /// Window locations and shortcuts from the docking panels and ribbon
        /// </summary>
        public bool Windows
        {
            get { return windows; }
        }

        /// <summary>
        /// Grid context menu settings
        /// </summary>
        public bool Menus
        {
            get { return menus; }
        }

        /// <summary>
        /// Grid column settings
        /// </summary>
        public bool Columns
        {
            get { return columns; }
        }
    }
}
