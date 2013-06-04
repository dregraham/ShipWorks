using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// What to do with a file after it is imported
    /// </summary>
    public enum GenericFileSuccessAction
    {
        /// <summary>
        /// Move the file to a new location
        /// </summary>
        Move = 0,

        /// <summary>
        /// Delete the original
        /// </summary>
        Delete = 1,

        /// <summary>
        /// Mark the email message as read (Email only)
        /// </summary>
        MarkAsRead = 2
    }
}
