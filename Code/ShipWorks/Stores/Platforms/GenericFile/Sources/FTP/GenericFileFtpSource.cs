extern alias rebex2015;

using System;
using System.Collections.Generic;
using System.Linq;
using rebex2015::Rebex.IO;
using rebex2015::Rebex.Net;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using log4net;
using ShipWorks.FileTransfer;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.FTP
{
    /// <summary>
    /// Implementation of a file source for FTP
    /// </summary>
    public class GenericFileFtpSource : GenericFileSource
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileFtpSource));

        GenericFileStoreEntity store;

        // The current/active FTP connection
        IFtp ftp = null;
        List<string> fileList = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileFtpSource(GenericFileStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
        }

        /// <summary>
        /// Dispose of this instance
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ftp != null)
                {
                    try
                    {
                        ftp.Disconnect();
                        ftp.Dispose();
                        ftp = null;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to disconnect and dispose of FTP", ex);
                    }
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Logon to FTP and load the list of files that we are going to be processing
        /// </summary>
        private void LoadFileList()
        {
            if (ftp != null)
            {
                throw new InvalidOperationException("Shouldn't be here if FTP was already initialized.");
            }

            try
            {
                ftp = FtpUtility.LogonToFtp(FtpAccountManager.GetAccount(store.FtpAccountID.Value));
                ftp.ChangeDirectory(store.FtpFolder);

                List<FileSystemItem> fileItems = ftp.GetList().Where(item => item.IsFile).ToList();

                // We have to make sure they are sorted in ascending UniqueID order. Not sure if they always will be or not, but our download algorithm depends on it,
                // so we do it.
                fileList = fileItems
                    .OrderBy(i => i.LastWriteTime)
                    .Select(i => i.Name)
                    .ToList();

                log.InfoFormat("Found {0} files in folder to consider", fileItems.Count);
            }
            catch (FileTransferException ex)
            {
                throw new GenericFileLoadException("There was an error logging in to the FTP server:\n\n" + ex.Message, ex);
            }
            catch (NetworkSessionException ex)
            {
                throw new GenericFileLoadException("There was an error reading files from the FTP server:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the next file on the disk based on configuration
        /// </summary>
        public override GenericFileInstance GetNextFile()
        {
            if (fileList == null)
            {
                LoadFileList();
            }

            if (fileList.Count == 0)
            {
                return null;
            }

            try
            {
                // In case of subject pattern matching, we may have to keep looping till we find one that's ok
                while (fileList.Count > 0)
                {
                    string filename = fileList[0];
                    fileList.RemoveAt(0);

                    // See if the subject must match a given pattern
                    if ((!string.IsNullOrWhiteSpace(store.NamePatternMatch) && !PathUtility.IsDosWildcardMatch(filename, store.NamePatternMatch)) ||
                        (!string.IsNullOrWhiteSpace(store.NamePatternSkip) && PathUtility.IsDosWildcardMatch(filename, store.NamePatternSkip)))
                    {
                        continue;
                    }
                    else
                    {
                        return new GenericFileFtpInstance(ftp, store.FtpFolder, filename);
                    }
                }

                // Looped through them all - none of the subjects must have matched the rules
                return null;
            }
            catch (ArgumentException ex)
            {
                throw new GenericFileLoadException("There was an filtering incoming email by Subject:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Handle a succesful import of the given file
        /// </summary>
        public override void HandleSuccess(GenericFileInstance file)
        {
            GenericFileSuccessAction action = (GenericFileSuccessAction) store.SuccessAction;

            switch (action)
            {
                case GenericFileSuccessAction.Delete:
                    DeleteFile((GenericFileFtpInstance) file, "a successful import");
                    break;

                case GenericFileSuccessAction.Move:
                    MoveFile((GenericFileFtpInstance) file, store.SuccessMoveFolder, "a successful import");
                    break;

                default:
                    throw new InvalidOperationException("Unknown success action value: " + action);
            }
        }

        /// <summary>
        /// Handle an error of reading the given file
        /// </summary>
        public override void HandleError(GenericFileInstance file, GenericFileStoreException ex)
        {
            GenericFileErrorAction action = (GenericFileErrorAction) store.ErrorAction;

            switch (action)
            {
                case GenericFileErrorAction.Stop:
                    throw new GenericFileStoreException(string.Format("There was an error reading file '{0}':\n\n{1}", file.Name, ex.Message), ex);

                case GenericFileErrorAction.Move:
                    MoveFile((GenericFileFtpInstance) file, store.ErrorMoveFolder, "an error occurred");
                    break;

                default:
                    throw new InvalidOperationException("Unknown error action value: " + action);
            }
        }

        /// <summary>
        /// Move the given file instance to the specified target folder
        /// </summary>
        private void MoveFile(GenericFileFtpInstance file, string targetFolder, string uiDetail)
        {
            log.InfoFormat("Moving file '{0}' to '{1}'", file.Name, targetFolder);

            try
            {
                string newName = string.Format("{0} [{1}]{2}", 
                    Path.GetFileNameWithoutExtension(file.Name), 
                    DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss"), 
                    Path.GetExtension(file.Name));

                ftp.Rename(file.Path + "/" + file.Name, targetFolder + "/" + newName);
            }
            catch (NetworkSessionException ex)
            {
                throw new GenericFileStoreException(string.Format("Could not move the file '{0}' after {1}.\n\nError: {2}", file.Name, uiDetail, ex.Message), ex);
            }
        }

        /// <summary>
        /// Delete the given file
        /// </summary>
        private void DeleteFile(GenericFileFtpInstance file, string uiDetail)
        {
            log.InfoFormat("Deleting file '{0}'", file.Name);

            try
            {
                ftp.DeleteFile(file.Path + "/" + file.Name);
            }
            catch (NetworkSessionException ex)
            {
                throw new GenericFileStoreException(string.Format("Could not move the file '{0}' after {1}.\n\nError: {2}", file.Name, uiDetail, ex.Message), ex);
            }
        }
    }
}
