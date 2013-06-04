using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// The status of executing tasks in the background
    /// </summary>
    public enum BackgroundResultStatus
    {
        /// <summary>
        /// All tasks completed successfully
        /// </summary>
        Success,

        /// <summary>
        /// Tasks completed successfully, but there were some issues
        /// </summary>
        Warning,

        /// <summary>
        /// Some tasks completed with errors
        /// </summary>
        Error
    }
}
