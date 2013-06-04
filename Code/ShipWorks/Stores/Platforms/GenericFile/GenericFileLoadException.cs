using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// Exception thrown when a generic file import fails b\c it can't load the file at all
    /// </summary>
    public class GenericFileLoadException : GenericFileStoreException
    {
        public GenericFileLoadException()
        {

        }

        public GenericFileLoadException(string message)
            : base(message)
        {

        }

        public GenericFileLoadException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
