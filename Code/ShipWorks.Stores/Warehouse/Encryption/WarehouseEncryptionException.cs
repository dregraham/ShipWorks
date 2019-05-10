using System;

namespace ShipWorks.Stores.Warehouse.Encryption
{
    public class WarehouseEncryptionException : Exception
    {
        public WarehouseEncryptionException(string message) : base(message)
        {
        }
        
        public WarehouseEncryptionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}