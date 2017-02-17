using System;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Enumeration containing flags for raw keyboard input.
    /// </summary>
    [CLSCompliant(false)]
    [Flags]
    public enum RawKeyboardFlags : ushort
    {
        KeyMake = 0,

        KeyBreak = 1,

        KeyE0 = 2,

        KeyE1 = 4,

        TerminalServerSetLED = 8,

        TerminalServerShadow = 0x10,

        TerminalServerVKPACKET = 0x20
    }
}