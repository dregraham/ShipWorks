using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for handling raw input
    /// </summary>
    [CLSCompliant(false)]
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
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RawInput pData,
            ref int pcbSize, int cbSizeHeader);

        /// <summary>
        /// Function to convert to unicode.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        /// <summary>
        /// Get characters given pressed keys and keyboard state
        /// </summary>
        public string GetCharactersFromKeys(VirtualKeys keys, bool shift, bool altGr)
        {
            StringBuilder buf = new StringBuilder(256);
            byte[] keyboardState = new byte[256];

            if (shift)
            {
                keyboardState[(int)Keys.ShiftKey] = 0xff;
            }

            ToUnicode((uint) keys, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
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
