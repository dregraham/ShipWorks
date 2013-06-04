using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Saving
{
    /// <summary>
    /// Thrown when a save job can't do anything because there was no output returned from template processing
    /// </summary>
    class SavingNoTemplateOutputException : SaveException
    {
        public SavingNoTemplateOutputException()
        {

        }

        public SavingNoTemplateOutputException(string message)
            : base(message)
        {

        }

        public SavingNoTemplateOutputException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
