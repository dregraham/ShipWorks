using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Base for all exceptions related to templates that we handle
    /// </summary>
    [Serializable]
    public class TemplateException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
