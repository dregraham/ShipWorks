namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Enum for Getting Raw Input Device Info
    /// </summary>

    public enum RawInputDeviceInformationCommand
    {
        /// <summary>
        /// pData points to a string that contains the device name. For this uiCommand only, the value in pcbSize is the character count (not the byte count).
        /// </summary>
        RIDI_DEVICENAME = 0x20000007,
        /// <summary>
        /// pData points to an RID_DEVICE_INFO structure.
        /// </summary>
        RIDI_DEVICEINFO = 0x2000000b,
        /// <summary>
        /// pData points to the previously parsed data.
        /// </summary>
        RIDI_PREPARSEDDATA = 0x20000005
    }
}
