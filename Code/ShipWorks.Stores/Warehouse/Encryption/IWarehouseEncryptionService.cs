using System.Threading.Tasks;

namespace ShipWorks.Stores.Warehouse.Encryption
{
    public interface IWarehouseEncryptionService
    {
        /// <summary>
        /// Encrypt the given text locally, using a key from kms
        /// </summary>
        Task<string> Encrypt(string plainText);
    }
}