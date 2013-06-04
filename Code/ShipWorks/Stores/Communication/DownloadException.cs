using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Exception that is thrown when something goes wrong during a store download.
    /// </summary>
    public class DownloadException : Exception
    {
        public DownloadException()
        {

        }

        public DownloadException(string message)
            : base(message)
        {

        }

        public DownloadException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
