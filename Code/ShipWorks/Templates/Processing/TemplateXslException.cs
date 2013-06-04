using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Exception thrown due to xsl processing
    /// </summary>
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
        /// The source XSL of the error.  If the error came from the same top-level tempalte that was being compiled\executed, then this is null.  If the error
        /// came from an imported template, this will be the full name of the template.  If an error came from an external file, this will be the full URI to the file.
        /// </summary>
        public override string Source
        {
            get { return errorSource; }
            set { }
        }
    }
}
