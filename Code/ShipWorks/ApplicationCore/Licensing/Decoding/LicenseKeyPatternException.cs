using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing.Decoding
{
    /// <summary>
    /// Thrown if an invalid license key pattern is detected
    /// </summary>
    public class LicenseKeyPatternException : Exception
    {
        public LicenseKeyPatternException()
        {

        }

        public LicenseKeyPatternException(string message)
            : base(message)
        {

        }

        public LicenseKeyPatternException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
