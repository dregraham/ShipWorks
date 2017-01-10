using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using log4net;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Message filter used by scanner service
    /// </summary>
    public class ScannerMessageFilter : IScannerMessageFilter
    {
        private readonly IUser32Input user32Input;
        private readonly IScannerIdentifier scannerIdentifier;
        private readonly IScanBuffer scanBuffer;
        private readonly ILog log;

        private readonly HashSet<VirtualKeys> pressedKeys = new HashSet<VirtualKeys>();
        private bool shouldBlock;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerMessageFilter(IScannerIdentifier scannerIdentifier, IUser32Input user32Input,
            IScanBuffer scanBuffer, Func<Type, ILog> getLogger)
        {
            this.scannerIdentifier = scannerIdentifier;
            this.user32Input = user32Input;
            this.scanBuffer = scanBuffer;
            log = getLogger(GetType());
        }

        /// <summary>
        /// Filter messages for the scanner
        /// </summary>
        public bool PreFilterMessage(ref Message message)
        {
            switch ((WindowsMessage) message.Msg)
            {
                case WindowsMessage.INPUT_DEVICE_CHANGE:
                    HandleDeviceChange(message.LParam, message.WParam);
                    return false;
                case WindowsMessage.INPUT:
                    return HandleInput(message.LParam);
                case WindowsMessage.KEYFIRST:
                case WindowsMessage.KEYUP:
                case WindowsMessage.CHAR:
                case WindowsMessage.DEADCHAR:
                case WindowsMessage.SYSKEYDOWN:
                case WindowsMessage.SYSKEYUP:
                case WindowsMessage.SYSCHAR:
                case WindowsMessage.SYSDEADCHAR:
                case WindowsMessage.KEYLAST:
                    return shouldBlock && pressedKeys.Contains((VirtualKeys) message.WParam);
            }

            return false;
        }

        /// <summary>
        /// Handle raw input message
        /// </summary>
        private bool HandleInput(IntPtr deviceHandle)
        {
            if (!scannerIdentifier.IsScanner(deviceHandle))
            {
                return false;
            }

            GenericResult<RawInput> result = user32Input.GetRawInputData(deviceHandle, RawInputCommand.Input);
            if (!result.Success)
            {
                log.Error(result.Message);
                return false;
            }

            shouldBlock = ProcessRawInputData(deviceHandle, result.Value);
            return true;
        }

        /// <summary>
        /// Process the raw input data from a scanner
        /// </summary>
        private bool ProcessRawInputData(IntPtr deviceHandle, RawInput input)
        {
            if (input.Data.Keyboard.Message == WindowsMessage.KEYFIRST)
            {
                pressedKeys.Add(input.Data.Keyboard.VirtualKey);
                string text = GetCharacter(input.Data.Keyboard.VirtualKey);
                scanBuffer.Append(deviceHandle, text);
            }
            else
            {
                pressedKeys.Remove(input.Data.Keyboard.VirtualKey);
            }

            return pressedKeys.Any();
        }

        /// <summary>
        /// Handle a device change
        /// </summary>
        private void HandleDeviceChange(IntPtr deviceHandle, IntPtr changeType)
        {
            if (changeType.ToInt32() == 1)
            {
                scannerIdentifier.HandleDeviceAdded(deviceHandle);
            }

            scannerIdentifier.HandleDeviceRemoved(deviceHandle);
        }

        /// <summary>
        /// Get a character for the given key
        /// </summary>
        private string GetCharacter(VirtualKeys virtualKey)
        {
            return user32Input.GetCharactersFromKeys(virtualKey, pressedKeys.Contains(VirtualKeys.Shift), false);
        }
    }
}
