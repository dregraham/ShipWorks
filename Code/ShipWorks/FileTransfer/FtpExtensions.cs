using System;
using System.Collections.Generic;
using Rebex.IO;
using Rebex.Net;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Extension methods for IFtp
    /// </summary>
    public static class FtpExtensions
    {
        /// <summary>
        /// Wrap the PutFiles method since both Ftp and Sftp support it, but use different enums
        /// </summary>
        public static void PutFiles(this IFtp ftp, string localPath, string remoteDirectoryPath, TraversalMode transferOptions, ActionOnExistingFiles existingFileMode)
        {
            Ftp typedFtp = ftp as Ftp;
            if (typedFtp != null)
            {
                typedFtp.Upload(localPath, remoteDirectoryPath, transferOptions, TransferMethod.Copy, existingFileMode);
                return;
            }

            Sftp typedSftp = ftp as Sftp;
            if (typedSftp != null)
            {
                typedSftp.Upload(localPath, remoteDirectoryPath, transferOptions, TransferMethod.Copy, existingFileMode);
                return;
            }

            throw new InvalidOperationException("PutFiles can only work on Ftp and Sftp instances of IFtp");
        }
    }
}
