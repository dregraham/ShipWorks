using System;

namespace ShipWorks.Stores.Warehouse.Encryption
{
    /// <summary>
    /// Exception for warehouse encryption operations
    /// </summary>
    public class WarehouseEncryptionException : Exception
    {
        /// <summary>
        /// Constructor with message
        /// </summary>
        public WarehouseEncryptionException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        public WarehouseEncryptionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}