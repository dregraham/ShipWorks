using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Exception specific to downloading from prostores
    /// </summary>
    public class ProStoresApiException : ProStoresException
    {
        string type;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresApiException(string message, string type)
            : base(message)
        {
            this.type = type;
        }

        /// <summary>
        /// The message
        /// </summary>
        public override string Message
        {
            get
            {
                if (base.Message != null && base.Message.Length > 0)
                {
                    string message = base.Message;

                    if (!message.EndsWith("."))
                    {
                        message += ".";
                    }

                    // For some reason ProStores leaves a placeholder in some of their messages, like:
                    // Invoice "{0}" is not ready for shipping.
                    message = message.Replace("\"{0}\"", "");

                    return message;
                }

                return Type;
            }
        }

        /// <summary>
        /// Type of exception as returned by ProStores
        /// </summary>
        public string Type
        {
            get { return type; }
        }
    }
}
