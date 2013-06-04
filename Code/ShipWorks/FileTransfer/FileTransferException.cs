using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// ShipWorks wrapper for file transfer exceptions
    /// </summary>
    public class FileTransferException : Exception
    {
        public FileTransferException()
        {

        }

        public FileTransferException(string message)
            : base(message)
        {

        }

        public FileTransferException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
