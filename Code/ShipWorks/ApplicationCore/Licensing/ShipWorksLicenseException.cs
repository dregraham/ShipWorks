using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Exception thrown by licensing subsystem
    /// </summary>
    public class ShipWorksLicenseException : Exception
    {
        public ShipWorksLicenseException()
        {

        }

        public ShipWorksLicenseException(string message)
            : base(message)
        {

        }

        public ShipWorksLicenseException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
