using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Exception that is thrown when a concurrency vioation occurrs while saving templates
    /// </summary>
    class TemplateConcurrencyException : TemplateException
    {
        public TemplateConcurrencyException()
        {

        }

        public TemplateConcurrencyException(string message)
            : base(message)
        {

        }

        public TemplateConcurrencyException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
