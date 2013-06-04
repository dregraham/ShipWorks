using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// what to do when an error occurrs during a generic file import
    /// </summary>
    public enum GenericFileErrorAction
    {
        /// <summary>
        /// Stop importing, leave the file, and display the error
        /// </summary>
        Stop = 0,
        
        /// <summary>
        /// Move the file and keep importing
        /// </summary>
        Move = 1,

        /// <summary>
        /// Mark the email message as read (Email only)
        /// </summary>
        MarkAsRead = 2
    }
}
