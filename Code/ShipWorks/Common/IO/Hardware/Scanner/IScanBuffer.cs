using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Buffer characters that make up a scan
    /// </summary>
    [Service]
    public interface IScanBuffer
    {
        /// <summary>
        /// Append input to the current scan
        /// </summary>
        void Append(IntPtr handle, string input);
    }
}