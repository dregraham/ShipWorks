using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// Exception that is thrown when there is a problem migrating configuration from ShipWorks 2x
    /// </summary>
    public class ConfigurationMigrationException : Exception
    {
        int code;
        string message;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigurationMigrationException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationMigrationException(Win32Exception win32Ex)
            : base(null, win32Ex)
        {
            this.code = win32Ex.NativeErrorCode;
            this.message = win32Ex.Message;
        }

        /// <summary>
        /// Determines the message automatically from the given code
        /// </summary>
        public ConfigurationMigrationException(int code)
        {
            this.code = code;
            this.message = LookupMessage(code);
        }

        /// <summary>
        /// Lookup the message to use for the given code
        /// </summary>
        private static string LookupMessage(int code)
        {
            return string.Format("Error code: {0}.  Please see the log for details.", code);
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
                return "The ShipWorks 2 configuration could not be upgrated.\n\n" + message;
            }
        }
    }
}
