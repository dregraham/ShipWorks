using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Container of information about the physical file path of a database
    /// </summary>
    public class DatabaseFileInfo
    {
        /// <summary>
        /// The name of the datbase
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// The full path of the MDF file
        /// </summary>
        public string DataFile { get; set; }

        /// <summary>
        /// The full path of the LDF file
        /// </summary>
        public string LogFile { get; set; }
    }
}
