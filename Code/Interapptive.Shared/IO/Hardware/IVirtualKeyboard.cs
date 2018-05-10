using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.IO.Hardware
{
    /// <summary>
    /// Send key strokes programaticly
    /// </summary>
    public interface IVirtualKeyboard
    {
        /// <summary>
        /// Send specified key as keyboard input 
        /// </summary>
        void Send(VirtualKeys key);
    }
}