using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Metrics;
using ShipWorks.Stores.Communication;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// Base class for generic file downloaders
    /// </summary>
    public abstract class GenericFileDownloaderBase : OrderElementFactoryDownloaderBase
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileDownloaderBase));

        int fileCount = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericFileDownloaderBase(GenericFileStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// The store being downloaded
        /// </summary>
        protected GenericFileStoreEntity GenericStore
        {
            get { return (GenericFileStoreEntity) Store; }
        }

        /// <summary>
        /// Can be used by derived classes to perform one-time download initialization
        /// </summary>
        protected virtual void InitializeDownload()
        {

        }

        /// <summary>
        /// Import data from the given file.  Must throw GenericFileStoreException (or derived) on error.  Return false to indicate cancled beofre completion.
        /// </summary>
        protected abstract bool ImportFile(GenericFileInstance file);

        /// <summary>
        /// Import from the XML file
        /// </summary>
        /// <param name="trackedDurationEvent"></param>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                InitializeDownload();

                GenericFileSourceType sourceType = GenericFileSourceTypeManager.GetFileSourceType((GenericFileSourceTypeCode) GenericStore.FileSource);

                // Prepare the file source
                using (GenericFileSource fileSource = sourceType.CreateFileSource(GenericStore))
                {
                    Progress.Detail = "Checking for files...";

                    // keep going until none are left
                    while (true)
                    {
                        if (!ImportNextFile(fileSource))
                        {
                            if (fileCount == 0)
                            {
                                Progress.Detail = "No files to import.";
                                Progress.PercentComplete = 100;
                            }
                            else
                            {
                                Progress.Detail = "Done";
                            }

                            return;
                        }

                        // Check if it has been cancelled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }
                    }
                }
            }
            catch (GenericFileStoreException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Import the next file from the source
        /// </summary>
        private bool ImportNextFile(GenericFileSource fileSource)
        {
            GenericFileInstance file = fileSource.GetNextFile();

            // No more files
            if (file == null)
            {
                return false;
            }

            // Increment that we've seen another file
            fileCount++;

            GenericFileStoreException importError = null;

            try
            {
                // Return of false means canceled before completed
                if (!ImportFile(file))
                {
                    // We still return true to caller b\c there ARE more (or could be).. the caller will check the Cancelled flag.
                    return false;
                }
            }
            catch (GenericFileStoreException ex)
            {
                log.Error(string.Format("Failed to load import source '{0}'", file.Name), ex);

                importError = ex;
            }

            // Determine whether to run the success or error action
            if (importError == null)
            {
                // Let the file source do whatever success action is configured.  This has to be outside of the try\catch, b\c if the HandleSuccess throws
                // for some reason - that should NOT be handled by the error handler, but bubble all the way up immediately.
                fileSource.HandleSuccess(file);
            }
            else
            {
                // Let the file source do whatever error action is configured
                fileSource.HandleError(file, importError);
            }

            return true;
        }
    }
}
