using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// Base for custom exceptions for the GenericFile store
    /// </summary>
    public class GenericFileStoreException : Exception
    {
        public GenericFileStoreException()
        {

        }

        public GenericFileStoreException(string message) :
            base(message)
        {

        }

        public GenericFileStoreException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
