using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// ThreeDCart specific exception
    /// </summary>
    [Serializable]
    public class ThreeDCartException : Exception
    {
        private readonly XmlNode errorNode;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartException(string message) 
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The base exception error message</param>
        /// <param name="errorNode">An xml node with error information to report to the user</param>
        public ThreeDCartException(string message, XmlNode errorNode)
            : base(message)
        {
            this.errorNode = errorNode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartException(string message, Exception inner)
            : base(message, inner)
        {

        }

        protected ThreeDCartException(SerializationInfo info, StreamingContext context): base(info, context)
        {
            if (info != null)
            {
                errorNode = info.GetValue("ErrorNode", typeof(XmlNode)) as XmlNode;
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info != null)
            {
                info.AddValue("ErrorNode", errorNode);
            }
        }

        /// <summary>
        /// The user displayable exception message
        /// </summary>
        public override string Message
        {
            get
            {
                string errorMessage = string.Empty;

                if (errorNode != null)
                {
                    // See if there is a discription node
                    XmlNode messageNode = errorNode.SelectSingleNode("//Description");
                    if (messageNode != null && !string.IsNullOrWhiteSpace(messageNode.InnerText))
                    {
                        errorMessage = messageNode.InnerText;
                    }
                    else if (errorNode.SelectSingleNode("//Error") != null)
                    {
                        // There was no description node, but there is an error node.  Use it's text
                        messageNode = errorNode.SelectSingleNode("//Error");
                        errorMessage = messageNode != null ? messageNode.InnerText : string.Empty;
                    }

                    // Parse out any user friendly info for the user to see
                    if (errorMessage.Contains("Technical description"))
                    {
                        errorMessage = errorMessage.Substring(0, errorMessage.IndexOf("---", StringComparison.OrdinalIgnoreCase)).Trim() + ".";
                        errorMessage = errorMessage.Replace("Technical description: ", string.Empty);
                    }
                }

                return string.Format("{0}{1}{2}{3}", base.Message, Environment.NewLine, Environment.NewLine, errorMessage);
            }
        }
    }
}
