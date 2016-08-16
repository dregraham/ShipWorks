using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.FileTransfer;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// BuyDotCom Utility Class
    /// </summary>
    public static class BuyDotComUtility
    {
        const string BuyDotComArchiveFolder = "BuyDotComArchiveFolder";

        /// <summary>
        /// Controls if messages get deleted off the server after they are downloaded
        /// </summary>
        public static bool ArchiveFileAfterDownload
        {
            get { return InterapptiveOnly.Registry.GetValue(BuyDotComArchiveFolder, true); }
            set { InterapptiveOnly.Registry.SetValue(BuyDotComArchiveFolder, value); }
        }

        /// <summary>
        /// Get any FTP account entity that is configured for the given store
        /// </summary>
        public static FtpAccountEntity GetFtpAccount(BuyDotComStoreEntity store)
        {
            return GetFtpAccount(store.FtpUsername, SecureText.Decrypt(store.FtpPassword, store.FtpUsername));
        }

        /// <summary>
        /// Get an FTP account entity for connecting to buy.com with the given username\password
        /// </summary>
        public static FtpAccountEntity GetFtpAccount(string username, string pasword)
        {
            // If the domain is ever changeable, we'll need to rework how we do license identifiers
            FtpAccountEntity account = FtpUtility.CreateDefaultAccount("trade.marketplace.buy.com",
                username,
                pasword);

            // Per buy.com, always use Active
            account.Passive = false;

            // Don't accidentially add it to the database
            account.IsNew = false;
            account.IsDirty = false;

            return account;
        }
    }
}
