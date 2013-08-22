using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using log4net;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Exception when handling JSON objects
    /// </summary>
    public class JsonException : Exception
    {
        static readonly ILog log = LogManager.GetLogger(typeof(JsonException));

        public JsonException(string message)
            : base(message)
        {

        }

        public JsonException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public JToken Input 
        { 
            get; 
            set; 
        }

        public string Key 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Override ToString to get specific info about the exception.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.Message))
            {
                msg.AppendFormat("Error Message: {0}{1}{2}", Environment.NewLine, this.Message, Environment.NewLine);
            }

            if (this.InnerException != null && !string.IsNullOrWhiteSpace(this.InnerException.Message))
            {
                msg.AppendFormat("Inner Exception Error Message: {0}{1}{2}", Environment.NewLine, this.InnerException.Message, Environment.NewLine);
            }

            msg.AppendFormat("Input: {0}{1}{2}Key: {3}", Environment.NewLine,
                Input == null ? string.Empty : Input.ToString(),
                Environment.NewLine,
                string.IsNullOrWhiteSpace(Key) ? string.Empty : Key,
                Environment.NewLine);
            msg.AppendFormat("Additional Info: {0}{1}{2}", Environment.NewLine, base.ToString(), Environment.NewLine);

            string message = msg.ToString();
            log.Error(msg);

            return message;
        }
    }
}
