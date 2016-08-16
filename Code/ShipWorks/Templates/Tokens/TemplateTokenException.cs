using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Exception thrown when there is invalid token XSL
    /// </summary>
    [Serializable]
    public class TemplateTokenException : TemplateException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateTokenException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateTokenException(string message, Exception innerEx) :
            base(message, innerEx)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected TemplateTokenException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }
    }
}
