using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// Thrown when a file that is being imported is not in a valid format
    /// </summary>
    public class GenericFileFormatException : GenericFileStoreException
    {
        public GenericFileFormatException()
        {

        }

        public GenericFileFormatException(string message)
            : base(message)
        {

        }

        public GenericFileFormatException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
