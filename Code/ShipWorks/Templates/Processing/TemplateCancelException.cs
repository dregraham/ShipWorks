using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Thrown when tempalte processing is canceled by the user
    /// </summary>
    public class TemplateCancelException : TemplateException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateCancelException()
        {

        }

        /// <summary>
        /// Exception message.
        /// </summary>
        public override string Message
        {
            get
            {
                return "Template processing was canceled.";
            }
        }
    }
}
