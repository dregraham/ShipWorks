using System;
using System.Runtime.Serialization;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Thrown when template processing is canceled by the user
    /// </summary>
    [Serializable]
    public class TemplateCancelException : TemplateException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateCancelException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected TemplateCancelException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }

        /// <summary>
        /// Exception message.
        /// </summary>
        public override string Message => "Template processing was canceled.";
    }
}
