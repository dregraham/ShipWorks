using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Factory for Buy.com FTP client
    /// </summary>
    public interface IFtpClientFactory
    {
        /// <summary>
        /// Login to an FTP client for the given store
        /// </summary>
        Task<IFtpClient> LoginAsync(IBuyDotComStoreEntity buyDotComStoreEntity);
    }
}