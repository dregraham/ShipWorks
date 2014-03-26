using System;
using System.Collections.Generic;
using System.Text;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Options for use with the WindowStateSaver class
    /// </summary>
    [Flags]
    public enum WindowStateSaverOptions
    {
        // Remember the size
        Size = 0x0001,

        // Remember the position
        Position = 0x0002,

        // Remember size and position
        FullState = 0x0003,

        // If remembering size, if there is no history, maximize the size to the current window
        InitialMaximize = 0x004
    }
}
