using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Handleable exception thrown by Generic store and derivatives
    /// </summary>
    public class GenericStoreException : Exception
    {
        public GenericStoreException()
        {

        }

        public GenericStoreException(string message)
            : base(message)
        {

        }

        public GenericStoreException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
