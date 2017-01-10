using System;
using System.Runtime.InteropServices;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for handling raw input
    /// </summary>
    public class User32Input : IUser32Input
    {
        /// <summary>
        /// Function to retrieve raw input data.
        /// </summary>
        /// <param name="hRawInput">Handle to the raw input.</param>
        /// <param name="uiCommand">Command to issue when retrieving data.</param>
        /// <param name="pData">Raw input data.</param>
        /// <param name="pcbSize">Number of bytes in the array.</param>
        /// <param name="cbSizeHeader">Size of the header.</param>
        /// <returns>0 if successful if pData is null, otherwise number of bytes if pData is not null.</returns>
        [DllImport("user32.dll")]
        private static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RawInput pData,
            ref int pcbSize, int cbSizeHeader);

        /// <summary>
        /// Get characters given pressed keys and keyboard state
        /// </summary>
        public string GetCharactersFromKeys(VirtualKeys keys, bool shift, bool altGr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get raw input data
        /// </summary>
        public GenericResult<RawInput> GetRawInputData(IntPtr deviceHandle, RawInputCommand commandType)
        {
            RawInput input;
            int size = Marshal.SizeOf(typeof(RawInput));

            int outSize = GetRawInputData(deviceHandle, commandType, out input, ref size,
                Marshal.SizeOf(typeof(RawInputHeader)));

            return outSize != -1 ?
                GenericResult.FromSuccess(input) :
                GenericResult.FromError<RawInput>($"Error getting raw input: {Marshal.GetLastWin32Error()}");
        }
    }
}
