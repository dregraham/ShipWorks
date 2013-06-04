using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    public class UpsOpenAccountException : Exception
    {
        public UpsOpenAccountException()
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOpenAccountException(CodeDescriptionType[] notifications)
            : base(GenerateMessage(notifications.Select(n => n.Description ?? n.Code)))
        {
        }

        public UpsOpenAccountException(string message)
            : base(message)
        {

        }

        public UpsOpenAccountException(string message, UpsOpenAccountErrorCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public UpsOpenAccountException(string message, Exception innerEx)
            : base(message, innerEx)
        {

        }

        public UpsOpenAccountException(string message, UpsOpenAccountErrorCode errorCode, Exception innerEx)
            : base(message, innerEx)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Generate the error message to display based on the messages returned
        /// </summary>
        private static string GenerateMessage(IEnumerable<string> errors)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("UPS returned the following error{0}:", errors.Any() ? "s" : "");

            foreach (string error in errors)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }
                sb.AppendFormat("    {0}", error);
            }

            return sb.ToString();
        }

        /// <summary>
        /// UPS Open Account error code
        /// </summary>
        public UpsOpenAccountErrorCode ErrorCode { get; set; }
    }
}
