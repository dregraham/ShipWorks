using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Exception that is thrown when a template cannot go where the user was trying to put it.
    /// </summary>
    [Serializable]
    public class TemplateInvalidLocationException : TemplateException
    {
        public TemplateInvalidLocationException()
        {

        }

        public TemplateInvalidLocationException(string message)
            : base(message)
        {

        }

        public TemplateInvalidLocationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
