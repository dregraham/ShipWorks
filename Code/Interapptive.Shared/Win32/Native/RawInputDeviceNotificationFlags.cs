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
        Default = 0x00000000,

        /// <summary>
        /// Remove raw input device listener
        /// </summary>
        RemoveDevice = 0x00000001,

        /// <summary>
        /// Listen to all raw input
        /// </summary>
        InputSink = 0x00000100,

        /// <summary>
        /// Notify when devices have changed
        /// </summary>
        DeviceNotify = 0x00002000,
    }
}
