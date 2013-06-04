using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Exception thrown when ShipWorks has apparently not been installed correctly.
    /// </summary>
    public class InstallationException : Exception
    {
        public InstallationException()
        {

        }

        public InstallationException(string message)
            : base(message)
        {

        }

        public InstallationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
