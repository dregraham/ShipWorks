using System;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;

namespace ShipWorks.ApplicationCore.Crashes 
{
    /// <summary>
    /// Represents errors that occur during submission of reports.
    /// </summary>
    [Serializable]
    class CrashSubmitException : Exception
    {
        public CrashSubmitException(String message, Exception innerException)
            : base(message, innerException)
        {

        }

        public CrashSubmitException(String message)
            : base(message)
        {

        }

        public CrashSubmitException()
        {

        }
    }
}