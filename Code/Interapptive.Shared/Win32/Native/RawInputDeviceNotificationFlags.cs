using System;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Defines the types of raw input device notifications we are interested in
    /// </summary>
    [Flags]
    public enum RawInputDeviceNotificationFlags
    {
        /// <summary>
        /// Default settings
        /// </summary>
        DEFAULT = 0x00000000,

        /// <summary>
        /// Remove raw input device listener
        /// </summary>
        REMOVE = 0x00000001,

        /// <summary>
        /// Listen to all raw input
        /// </summary>
        INPUTSINK = 0x00000100,

        /// <summary>
        /// Notify when devices have changed
        /// </summary>
        DEVNOTIFY = 0x00002000,
    }
}
