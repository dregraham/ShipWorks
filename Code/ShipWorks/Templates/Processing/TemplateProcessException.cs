using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Exception thrown when there is an error during template processing.
    /// </summary>
    class TemplateProcessException : TemplateException
    {
        public TemplateProcessException()
        {

        }

        public TemplateProcessException(string message)
            : base(message)
        {

        }

        public TemplateProcessException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
