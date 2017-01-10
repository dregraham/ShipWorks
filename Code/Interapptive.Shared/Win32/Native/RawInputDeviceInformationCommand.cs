using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Enumeration containing the DeviceInformationCommand type
    /// </summary>
    public enum RawInputDeviceInformationCommand
    {
        /// <summary>
        /// pData points to a string that contains the device name. For this uiCommand only, the value in pcbSize is the character count (not the byte count).
        /// </summary>
        DeviceName = 0x20000007,

        /// <summary>
        /// pData points to an RID_DEVICE_INFO structure.
        /// </summary>
        DeviceInfo = 0x2000000b,

        /// <summary>
        /// pData points to the previously parsed data.
        /// </summary>
        PreviouslyParsedData = 0x20000005
    }
}
