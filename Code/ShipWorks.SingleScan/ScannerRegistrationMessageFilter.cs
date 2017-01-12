using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Message filter used to identify the scanner
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ScannerRegistrationMessageFilter : IScannerMessageFilter
    {
        private readonly IUser32Input user32Input;
        private readonly IScanBuffer scanBuffer;
        private readonly ILog log;

        private readonly HashSet<VirtualKeys> pressedKeys = new HashSet<VirtualKeys>();
        private bool shouldBlock;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerRegistrationMessageFilter(IUser32Input user32Input, IScanBuffer scanBuffer, Func<Type, ILog> getLogger)
        {
            this.user32Input = user32Input;
            this.scanBuffer = scanBuffer;
            log = getLogger(GetType());
        }

        /// <summary>
        /// Filter messages for the scanner
        /// </summary>
        /// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
        public bool PreFilterMessage(ref Message message)
        {
            switch ((WindowsMessage) message.Msg)
            {
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
        /// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
        private bool HandleInput(IntPtr messageHandle)
        {
            GenericResult<RawInput> result = user32Input.GetRawInputData(messageHandle, RawInputCommand.Input);
            if (!result.Success)
            {
                log.Error(result.Message);
                return false;
            }

            shouldBlock = ProcessRawInputData(result.Value.Header.DeviceHandle, result.Value);
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
        /// Get a character for the given key
        /// </summary>
        private string GetCharacter(VirtualKeys virtualKey)
        {
            return user32Input.GetCharactersFromKeys(virtualKey, pressedKeys.Contains(VirtualKeys.Shift), false);
        }
    }
}
