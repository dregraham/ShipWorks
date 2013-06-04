using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Exception that is thrown when there is a problem opening the Windows Firewall
    /// </summary>
    public class WindowsFirewallException : Exception
    {
        int code;
        string message;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowsFirewallException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowsFirewallException(Win32Exception win32Ex)
            : base(null, win32Ex)
        {
            this.code = win32Ex.NativeErrorCode;
            this.message = win32Ex.Message;
        }

        /// <summary>
        /// Determines the message automatically from the given code
        /// </summary>
        public WindowsFirewallException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        /// <summary>
        /// Indicates the error code that occurred
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return code;
            }
        }

        /// <summary>
        /// Message to display to the user
        /// </summary>
        public override string Message
        {
            get
            {
                return "The firewall could not be opened.\n\n" + message;
            }
        }
    }
}
