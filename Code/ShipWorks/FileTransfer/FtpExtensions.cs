extern alias rebex2015;

using System;
using System.Collections.Generic;
using rebex2015::Rebex.Net;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Extension methods for IFtp
    /// </summary>
    public static class FtpExtensions
    {
        private static readonly IDictionary<FtpBatchTransferOptions, SftpBatchTransferOptions> batchTransferOptionsTranslations = 
            new Dictionary<FtpBatchTransferOptions, SftpBatchTransferOptions>
            {
                {FtpBatchTransferOptions.Default, SftpBatchTransferOptions.Default},
                {FtpBatchTransferOptions.Recursive, SftpBatchTransferOptions.Recursive},
                {FtpBatchTransferOptions.SkipLinks, SftpBatchTransferOptions.SkipLinks},
                {FtpBatchTransferOptions.ThrowExceptionOnLinks, SftpBatchTransferOptions.ThrowExceptionOnLinks},
                {FtpBatchTransferOptions.XCopy, SftpBatchTransferOptions.XCopy},
            };

        private static readonly IDictionary<FtpActionOnExistingFiles, SftpActionOnExistingFiles> actionOnExistingFilesTranslations = 
            new Dictionary<FtpActionOnExistingFiles, SftpActionOnExistingFiles>
            {
                {FtpActionOnExistingFiles.OverwriteAll, SftpActionOnExistingFiles.OverwriteAll},
                {FtpActionOnExistingFiles.OverwriteDifferentSize, SftpActionOnExistingFiles.OverwriteDifferentSize},
                {FtpActionOnExistingFiles.OverwriteOlder, SftpActionOnExistingFiles.OverwriteOlder},
                {FtpActionOnExistingFiles.ResumeIfPossible, SftpActionOnExistingFiles.ResumeIfPossible},
                {FtpActionOnExistingFiles.SkipAll, SftpActionOnExistingFiles.SkipAll},
                {FtpActionOnExistingFiles.ThrowException, SftpActionOnExistingFiles.ThrowException},
            };

        /// <summary>
        /// Wrap the PutFiles method since both Ftp and Sftp support it, but use different enums
        /// </summary>
        public static void PutFiles(this IFtp ftp, string localPath, string remoteDirectoryPath, FtpBatchTransferOptions transferOptions, FtpActionOnExistingFiles existingFileMode)
        {
            Ftp typedFtp = ftp as Ftp;
            if (typedFtp != null)
            {
                typedFtp.PutFiles(localPath, remoteDirectoryPath, transferOptions, existingFileMode);
                return;
            }

            Sftp typedSftp = ftp as Sftp;
            if (typedSftp != null)
            {
                typedSftp.PutFiles(localPath, remoteDirectoryPath, batchTransferOptionsTranslations[transferOptions], actionOnExistingFilesTranslations[existingFileMode]);
                return;
            }

            throw new InvalidOperationException("PutFiles can only work on Ftp and Sftp instances of IFtp");
        }
    }
}
