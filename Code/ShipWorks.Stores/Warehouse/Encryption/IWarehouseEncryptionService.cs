using System.Threading.Tasks;

namespace ShipWorks.Stores.Warehouse.Encryption
{
    /// <summary>
    /// Service for encrypting in a way that the ShipWorks Warehouse app can decrypt
    /// </summary>
    public interface IWarehouseEncryptionService
    {
        /// <summary>
        /// Encrypt the given text locally, using a key from kms
        /// </summary>
        Task<string> Encrypt(string plainText);
    }
}