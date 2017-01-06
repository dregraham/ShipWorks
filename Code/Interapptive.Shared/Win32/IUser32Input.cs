using System;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for handling raw input
    /// </summary>
    public interface IUser32Input
    {
        /// <summary>
        /// Get characters given pressed keys and keyboard state
        /// </summary>
        string GetCharactersFromKeys(VirtualKeys keys, bool shift, bool altGr);

        /// <summary>
        /// Get raw input data
        /// </summary>
        GenericResult<RawInput> GetRawInputData(IntPtr deviceHandle, RawInputCommand commandType);
    }
}
