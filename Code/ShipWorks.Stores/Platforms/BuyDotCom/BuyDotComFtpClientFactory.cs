using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Rebex.Net;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.FileTransfer;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Factory for Buy.com FTP client
    /// </summary>
    [Component]
    public class BuyDotComFtpClientFactory : IBuyDotComFtpClientFactory
    {
        private readonly Func<IFtp, IBuyDotComFtpClient> createFtpClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComFtpClientFactory(Func<IFtp, IBuyDotComFtpClient> createFtpClient)
        {
            this.createFtpClient = createFtpClient;
        }

        /// <summary>
        /// Login to an FTP client for the given store
        /// </summary>
        public async Task<IBuyDotComFtpClient> LoginAsync(IBuyDotComStoreEntity store)
        {
            try
            {
                IFtp ftp = await FtpUtility.LogonToFtpAsync(BuyDotComUtility.GetFtpAccount(store)).ConfigureAwait(false);
                return createFtpClient(ftp);
            }
            catch (FileTransferException ex)
            {
                throw new BuyDotComException(ex.Message, ex);
            }
        }
    }
}
