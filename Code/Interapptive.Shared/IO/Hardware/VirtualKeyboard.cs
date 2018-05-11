using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.IO.Hardware
{
    /// <summary>
    /// Send key strokes programmatically
    /// </summary>
    /// <remarks>
    /// Much code from https://stackoverflow.com/questions/20482338/simulate-keyboard-input-in-c-sharp
    /// </remarks>
    [Component]
    public class VirtualKeyboard : IVirtualKeyboard
    {
        /// <summary>
        /// Send specified key as keyboard input 
        /// </summary>
        public void Send(VirtualKeys key)
        {
            string keyToSend;
            switch (key)
            {
                case VirtualKeys.Tab:
                    keyToSend = "{TAB}";
                    break;
                case VirtualKeys.Return:
                    keyToSend = "{ENTER}";
                    break;
                case VirtualKeys.Escape:
                    keyToSend = "{ESC}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("key", key, "Unknown key sent to virtual keyboard");
            }

            SendKeys.Send(keyToSend);
        }
    }
}