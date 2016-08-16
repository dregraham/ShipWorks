using System;
using System.Runtime.Serialization;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Exception thrown due to XSL processing
    /// </summary>
    [Serializable]
    public class TemplateXslException : TemplateException
    {
        int lineNumber;
        int linePosition;
        string errorSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXslException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXslException(string message, int lineNumber, int linePosition, string errorSource, Exception innerEx) :
            base(message, innerEx)
        {
            this.lineNumber = lineNumber;
            this.linePosition = linePosition;
            this.errorSource = errorSource;
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected TemplateXslException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }

        /// <summary>
        /// Custom message processing
        /// </summary>
        public override string Message
        {
            get
            {
                string message = base.Message;

                // Our InnerException already is the Exception.  It's message was extracted to create the base.Message.
                if (message.Contains("InnerException") && InnerException != null && InnerException.InnerException != null)
                {
                    message = message.Replace("See InnerException for a complete description of the error.", "\n\n" + InnerException.InnerException.Message);
                }

                return message;
            }
        }

        /// <summary>
        /// The line number indicating where the error occurred.
        /// </summary>
        public int LineNumber
        {
            get { return lineNumber; }
        }

        /// <summary>
        /// The line position indicating where the error occurred
        /// </summary>
        public int LinePosition
        {
            get { return linePosition; }
        }

        /// <summary>
        /// The source XSL of the error.  If the error came from the same top-level template that was being compiled\executed, then this is null.  If the error
        /// came from an imported template, this will be the full name of the template.  If an error came from an external file, this will be the full URI to the file.
        /// </summary>
        public override string Source
        {
            get { return errorSource; }
            set { }
        }
    }
}
