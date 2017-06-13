using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using log4net;
using System.Text.RegularExpressions;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Disk
{
    /// <summary>
    /// Implementation of a file source for local disk
    /// </summary>
    public class GenericFileDiskSource : GenericFileSource
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileDiskSource));

        GenericFileStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileDiskSource(GenericFileStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
        }

        /// <summary>
        /// Get the next file on the disk based on configuration
        /// </summary>
        public override GenericFileInstance GetNextFile()
        {
            if (!Directory.Exists(store.DiskFolder))
            {
                throw new GenericFileLoadException(string.Format("The import folder '{0}' does not exist.", store.DiskFolder));
            }

            try
            {
                string searchPattern = "*";

                // Override "all" search pattern if one was specified
                if (!string.IsNullOrWhiteSpace(store.NamePatternMatch))
                {
                    searchPattern = store.NamePatternMatch;
                }

                // Get the list of files matching the pattern
                DirectoryInfo directory = new DirectoryInfo(store.DiskFolder);

                // Get the list of files, ordered from oldest to newest
                List<FileInfo> files = directory.GetFiles(searchPattern).OrderBy(fsi => fsi.LastWriteTime > fsi.CreationTime ? fsi.LastWriteTime : fsi.CreationTime).ToList();
                int originalCount = files.Count;

                // Remove all the files that are excluded by the cant match pattern
                files = RemoveExcludedFiles(files, store.NamePatternSkip);

                log.InfoFormat("DiskSource found {0} files matching positive pattern, {1} after exclusions.", originalCount, files.Count);

                if (files.Count > 0)
                {
                    return new GenericFileDiskInstance(files[0]);
                }
                else
                {
                    return null;
                }
            }
            catch (IOException ex)
            {
                throw new GenericFileLoadException("Could not read from the import folder: " + ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new GenericFileLoadException("Could not read from the import folder: " + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new GenericFileLoadException("Could not read from the import folder: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Remove all the files excluded by the given pattern
        /// </summary>
        private List<FileInfo> RemoveExcludedFiles(List<FileInfo> files, string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return files;
            }

            return files.Where(fi => PathUtility.IsDosWildcardMatch(fi.Name, pattern)).ToList();
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
                    DeleteFile((GenericFileDiskInstance) file, "a successful import");
                    break;

                case GenericFileSuccessAction.Move:
                    MoveFile((GenericFileDiskInstance) file, store.SuccessMoveFolder, "a successful import");
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
                    MoveFile((GenericFileDiskInstance) file, store.ErrorMoveFolder, "an error occurred");
                    break;

                default:
                    throw new InvalidOperationException("Unknown error action value: " + action);
            }
        }

        /// <summary>
        /// Move the given file instance to the specified target folder
        /// </summary>
        private static void MoveFile(GenericFileDiskInstance file, string targetFolder, string uiDetail)
        {
            FileInfo fileInfo = file.FileInfo;

            log.InfoFormat("Moving file '{0}' to '{1}'", fileInfo.FullName, targetFolder);

            try
            {
                string newName = string.Format("{0} [{1}]{2}", 
                    Path.GetFileNameWithoutExtension(fileInfo.FullName), 
                    DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss"), 
                    Path.GetExtension(fileInfo.FullName));

                fileInfo.MoveTo(Path.Combine(targetFolder, newName));
            }
            catch (IOException ex)
            {
                log.Warn($"Could not move the file '{file.Name}' after {uiDetail}.\n\nError: {ex.Message}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Warn($"Could not move the file '{file.Name}' after {uiDetail}.\n\nError: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Delete the given file
        /// </summary>
        private static void DeleteFile(GenericFileDiskInstance file, string uiDetail)
        {
            log.InfoFormat("Deleting file '{0}'", file.FileInfo.FullName);

            try
            {
                file.FileInfo.Delete();
            }
            catch (IOException ex)
            {
                log.Warn($"Could not move the file '{file.Name}' after {uiDetail}.\n\nError: {ex.Message}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Warn($"Could not move the file '{file.Name}' after {uiDetail}.\n\nError: {ex.Message}", ex);
            }
        }
    }
}
