using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// The status of reading a scale
    /// </summary>
    public enum ScaleReadStatus
    {
        /// <summary>
        /// The scale was successfully read
        /// </summary>
        Success,

        /// <summary>
        /// No scale was found to read
        /// </summary>
        NotFound,

        /// <summary>
        /// A scale was found, but there was a problem reading it
        /// </summary>
        ReadError
    }
}
