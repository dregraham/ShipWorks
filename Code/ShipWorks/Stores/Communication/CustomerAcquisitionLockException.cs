using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Thrown when a CustomerAcquisitionLock cannot be obtained
    /// </summary>
    class CustomerAcquisitionLockException : Exception
    {
        public CustomerAcquisitionLockException()
        {

        }

        public CustomerAcquisitionLockException(string message)
            : base(message)
        {

        }

        public CustomerAcquisitionLockException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
