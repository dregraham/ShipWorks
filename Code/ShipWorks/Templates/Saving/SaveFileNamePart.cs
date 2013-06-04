using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Saving
{
    /// <summary>
    /// Use to distinguish between the name and folder parts of a full filename.
    /// </summary>
    public enum SaveFileNamePart
    {
        /// <summary>
        /// The full name and folder of a file.
        /// </summary>
        FullName,

        /// <summary>
        /// Just the file name part without the folder.
        /// </summary>
        Name,

        /// <summary>
        /// Just the folder without the filename
        /// </summary>
        Folder
    }
}
