using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using log4net;
using System.Threading;
using System.Diagnostics;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Common.Threading;
using ShipWorks.Users;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Linq;
using Autofac;
using Interapptive.Shared;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Actions;
using ShipWorks.Data.Utility;
using ShipWorks.Users.Security;
using ShipWorks.Users.Audit;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Coordinates the download of orders from one or more stores
    /// </summary>
    public static class DownloadManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DownloadManager));

        // Downloading flag
        static volatile bool isDownloading = false;

        // The progress dlg currently displayed, or null if not displayed.
        static ProgressDlg progressDlg = null;

        // The busy token
        static ApplicationBusyToken busyToken;

        // The progress item container
        static ProgressProvider progressProvider;

        // The current queue of items to be downloaded and its locking features
        static List<PendingDownload> downloadQueue;
        static object downloadQueueLock = new object();

        #region class PendingDownload

        class PendingDownload
        {
            public long StoreID { get; set; }
            public ProgressItem ProgressItem { get; set; }
            public DownloadInitiatedBy DownloadInitiatedBy { get; set; }
        }

        #endregion

        /// <summary>
        /// Raised when a download is about to start
        /// </summary>
        static public event EventHandler DownloadStarting;

        /// <summary>
        /// Raised after a download completes, regardless of the outcome
        /// </summary>
        static public event DownloadCompleteEventHandler DownloadComplete;

        // Data for controlling auto-download
        static Dictionary<long, DateTime?> lastDownloadTimesCache = null;
        static object lastDownloadTimesLock = new object();
        static DateTime lastAutoDownloadCheck;

        /// <summary>
        /// Initialize the DownloadManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            lastAutoDownloadCheck = DateTime.MinValue;
            lastDownloadTimesCache = null;

            progressProvider = new ProgressProvider();
            downloadQueue = new List<PendingDownload>();
        }

        /// <summary>
        /// Indicates if a download is currently in progress
        /// </summary>
        public static bool IsDownloading(long? storeID = null)
        {
            if (storeID == null)
            {
                return isDownloading;
            }
            else
            {
                return downloadQueue.Any(d => d.StoreID == storeID);
            }
        }

        /// <summary>
        /// Indicates if the progress window is currently visible
        /// </summary>
        public static bool IsProgressVisible
        {
            get { return progressDlg != null; }
        }

        /// <summary>
        /// Show the progress window modally
        /// </summary>
        public static void ShowProgressDlg(IWin32Window parent)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired);

            using (progressDlg = CreateProgressWindow())
            {
                progressDlg.ShowDialog(parent);
            }

            progressDlg = null;
        }

        /// <summary>
        /// Initiates an auto-download for any stores that are due for an auto download.  By default we only actually do anything at most once every 15 seconds, but
        /// if you really really need this to check right now-now, you can set forceCheckNow to true
        /// </summary>
        public static void StartAutoDownloadIfNeeded(bool forceCheckNow = false)
        {
            // If we are a background process, and the UI process is open - then just let the UI do it
            if (!Program.ExecutionMode.IsUISupported && UserInterfaceExecutionMode.IsProcessRunning)
            {
                return;
            }

            // Don't check more often than every 15 seconds
            if (!forceCheckNow && lastAutoDownloadCheck + TimeSpan.FromSeconds(15) > DateTime.UtcNow)
            {
                return;
            }

            lastAutoDownloadCheck = DateTime.UtcNow;

            List<StoreEntity> readyToDownload = GetStoresForAutoDownloading();

            if (readyToDownload.Count > 0)
            {
                StartDownload(readyToDownload, DownloadInitiatedBy.ShipWorks);
            }
        }

        /// <summary>
        /// Gets the stores that are ready for automatic downloading.
        /// </summary>
        /// <returns>A List of StoreEntity instances.</returns>
        private static List<StoreEntity> GetStoresForAutoDownloading()
        {
            List<StoreEntity> readyToDownload = new List<StoreEntity>();

            bool wereTimesCached = (lastDownloadTimesCache != null);

            // Find each store that is ready for an auto-download
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                if (!ComputerDownloadPolicy.Load(store).IsThisComputerAllowed)
                {
                    continue;
                }

                // First see if its enabled in general, and if auto-downloads at all
                if (!store.Enabled || !store.AutoDownload)
                {
                    continue;
                }

                // If only when a way, make sure we are away
                if (store.AutoDownloadOnlyAway && !IdleWatcher.IsIdle)
                {
                    continue;
                }

                DateTime? lastDownload = GetLastDownloadTime(store);

                if (lastDownload == null || lastDownload + TimeSpan.FromMinutes(store.AutoDownloadMinutes) < DateTime.UtcNow)
                {
                    readyToDownload.Add(store);
                }
            }

            // We checked the ready-to-download with cached download times. If there are any that are ready to download it
            // could be that they've recently been downloaded since we cached the values.  So check again after refetching
            // the latest last download times.
            if (wereTimesCached && readyToDownload.Count > 0)
            {
                lock (lastDownloadTimesLock)
                {
                    lastDownloadTimesCache = null;
                }

                foreach (StoreEntity store in readyToDownload.ToArray())
                {
                    DateTime? lastDownload = GetLastDownloadTime(store);

                    // Its downloaded somwhere since we had the last cached time, so remove it.
                    if (lastDownload.HasValue && lastDownload + TimeSpan.FromMinutes(store.AutoDownloadMinutes) >= DateTime.UtcNow)
                    {
                        readyToDownload.Remove(store);
                    }
                }
            }

            return readyToDownload;
        }

        /// <summary>
        /// Initiate downloading of the given stores
        /// </summary>
        public static void StartDownload(ICollection<StoreEntity> stores, DownloadInitiatedBy initiatedBy)
        {
            Debug.Assert(!Program.ExecutionMode.IsUISupported || !Program.MainForm.InvokeRequired);

            foreach (StoreEntity store in stores)
            {
                AddToDownloadedQueue(store, initiatedBy);
            }
        }

        /// <summary>
        /// Add the given store to the queue of things to download.
        /// </summary>
        private static void AddToDownloadedQueue(StoreEntity store, DownloadInitiatedBy initiatedBy)
        {
            Debug.Assert(!Program.ExecutionMode.IsUISupported || !Program.MainForm.InvokeRequired);

            lock (downloadQueueLock)
            {
                log.InfoFormat("Adding store {0} to download queue.", store.StoreName);

                // If the store is already in the queue, forget it
                if (downloadQueue.Any(d => d.StoreID == store.StoreID))
                {
                    log.InfoFormat("Store {0} is already downloading.", store.StoreName);
                    return;
                }

                // We are going to be starting a new download so create a new progress set.  If the progress window
                // is open right now, it will just keep showing the old stuff.  That's basically OK, b\c the only way
                // it could be open and us starting a new download is if its an auto-download.  And I don't think we'd want
                // to just clear out what they were looking at.
                if (!isDownloading)
                {
                    progressProvider = new ProgressProvider();
                }

                // Create the progress item, and tag it with the store
                ProgressItem progressItem = new ProgressItem(store.StoreName);
                progressProvider.ProgressItems.Add(progressItem);

                // Add to the download queue
                PendingDownload download = new PendingDownload { StoreID = store.StoreID, ProgressItem = progressItem, DownloadInitiatedBy = initiatedBy };
                downloadQueue.Add(download);

                // Ensure our downloader is working.
                if (!isDownloading)
                {
                    Debug.Assert(busyToken == null);

                    // If we are in a context sensitive scope, we have to wait until next time.  If we are on the UI, we'll always get it.
                    // We only may not if we are running in the background.
                    if (!ApplicationBusyManager.TryOperationStarting("downloading", out busyToken))
                    {
                        return;
                    }

                    isDownloading = true;

                    Thread thread = new Thread(ExceptionMonitor.WrapThread(DownloadWorkerThread));
                    thread.Name = "DownloadThread";
                    thread.IsBackground = true;
                    thread.Start();

                    // Raise the starting event
                    if (DownloadStarting != null)
                    {
                        DownloadStarting(null, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Entry point function for downloading
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private static void DownloadWorkerThread()
        {
            log.InfoFormat("Download starting.");

            // Configuration affects downloading
            ConfigurationData.CheckForChangesNeeded();

            int nextIndexToDownload = 0;
            bool showDashboardError = false;

            // Keep going until there are no more stores to download for
            while (true)
            {
                StoreEntity store = null;
                ProgressItem progressItem = null;
                DownloadInitiatedBy initiatedBy;

                bool releaseQueueLock = true;
                Monitor.Enter(downloadQueueLock);
                try
                {
                    // If there are no more left in the list to download then break out of the immediate while loop.
                    if (nextIndexToDownload >= downloadQueue.Count || progressProvider.CancelRequested)
                    {
                        // Let the operation manager know we are done
                        ApplicationBusyManager.OperationComplete(busyToken);
                        busyToken = null;

                        // Clear the queue and reset our event so we stop back up at that top while loop
                        downloadQueue.Clear();

                        // No longer downloading
                        isDownloading = false;

                        log.InfoFormat("Download complete");

                        // Release the downloadQueueLock before notifying that the download is complete
                        Monitor.Exit(downloadQueueLock);
                        releaseQueueLock = false;

                        // Raise the completed event
                        if (DownloadComplete != null)
                        {
                            ProgressDlg dlg = progressDlg;

                            DownloadComplete(null, new DownloadCompleteEventArgs(showDashboardError, (dlg != null && dlg.ProgressProvider == progressProvider)));
                        }

                        // Escape, we are all done.
                        return;
                    }
                    // Pull the next store to download off the queue
                    else
                    {
                        PendingDownload pendingDownload = downloadQueue[nextIndexToDownload];

                        store = StoreManager.GetStore(pendingDownload.StoreID);
                        progressItem = pendingDownload.ProgressItem;
                        initiatedBy = pendingDownload.DownloadInitiatedBy;

                        nextIndexToDownload++;
                    }
                }
                finally
                {
                    if (releaseQueueLock)
                    {
                        Monitor.Exit(downloadQueueLock);
                    }
                }


                DownloadEntity downloadLog = null;
                StoreDownloader downloader = null;

                try
                {
                    // This item is now running
                    progressItem.Starting();

                    if (store == null)
                    {
                        throw new StoreDeletedException();
                    }

                    log.InfoFormat("Starting download for store '{0}' ({1})", store.StoreName, store.StoreID);

                    // We open a lock that will stay open for the duration of the store download,
                    // which will serve to lock out any other running instance of ShipWorks from downloading
                    // for this store.
                    using (SqlEntityLock storeLock = new SqlEntityLock(store.StoreID, "Download"))
                    {
                        // We create the log entry right when we start
                        downloadLog = CreateDownloadLog(store, initiatedBy);

                        // Create the downloader
                        downloader = StoreTypeManager.GetType(store).CreateDownloader();

                        // Verify the license
                        progressItem.Detail = "Connecting...";
                        CheckLicense(store);

                        // Do the download.  Operates as the super user.
                        using (AuditBehaviorScope auditScope = new AuditBehaviorScope(
                            AuditBehaviorUser.SuperUser,
                            new AuditReason(initiatedBy == DownloadInitiatedBy.ShipWorks ? AuditReasonType.AutomaticDownload : AuditReasonType.ManualDownload)))
                        {
                            downloader.Download(progressItem, downloadLog.DownloadID);
                        }

                        // Item is complete
                        progressItem.Completed();
                    }
                }
                catch (SqlAppResourceLockException ex)
                {
                    log.Error("Could not obtain download lock.", ex);

                    progressItem.Failed(new DownloadException("Another computer is already downloading for this store.", ex));
                }
                catch (DownloadException ex)
                {
                    log.Error("Download error", ex);

                    progressItem.Failed(ex);
                }
                catch (ShipWorksLicenseException ex)
                {
                    log.Error("License error", ex);

                    progressItem.Failed(ex);
                }
                catch (TangoException ex)
                {
                    log.Error("Tango error", ex);

                    progressItem.Failed(ex);
                }
                catch (StoreDeletedException ex)
                {
                    log.Error("Store Deleted", ex);

                    progressItem.Failed(new StoreDeletedException(ex));

                    // Can't log the download if there's no more store
                    downloadLog = null;
                }
                catch (ORMQueryExecutionException ex)
                {
                    log.Error("Query Error", ex);

                    // See if this is because the store got deleted.  Rare case, but what the hell, might as well handle it.
                    if (StoreCollection.GetCount(SqlAdapter.Default, StoreFields.StoreID == store.StoreID) == 0)
                    {
                        log.Warn("The store was deleted.");
                        progressItem.Failed(new StoreDeletedException(ex));

                        // Can't log the download if there's no more store
                        downloadLog = null;
                    }
                    else
                    {
                        // Don't know what to do with it otherwise
                        throw;
                    }
                }

                // This would only be null if the store had been deleted before we tried to log the download
                if (downloadLog != null)
                {
                    // Could be null if an error was thrown before we got that far
                    if (downloader != null)
                    {
                        downloadLog.QuantityTotal = downloader.QuantitySaved;
                        downloadLog.QuantityNew = downloader.QuantityNew;
                    }

                    // Update the download log with the result
                    if (progressItem.Status == ProgressItemStatus.Success)
                    {
                        downloadLog.Result = (int) DownloadResult.Success;
                    }
                    else if (progressItem.Status == ProgressItemStatus.Canceled)
                    {
                        downloadLog.Result = (int) DownloadResult.Cancel;
                    }
                    else
                    {
                        downloadLog.Result = (int) DownloadResult.Error;
                        downloadLog.ErrorMessage = progressItem.Error.Message;
                        showDashboardError = true;
                    }

                    // Save the updated log
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        downloadLog.Ended = DateTime.UtcNow;
                        adapter.SaveAndRefetch(downloadLog);
                    }

                    ActionDispatcher.DispatchDownloadFinished(store.StoreID, (DownloadResult) downloadLog.Result, downloadLog.QuantityNew);
                }
                else
                {
                    // If there wasn't a DownloadLog created for this download, (due to applock, or store deleted), then manually set the last download time
                    // in the cache so that we don't keep trying every 15 seconds since it hasn't been updated in the database.
                    if (store != null)
                    {
                        lock (lastDownloadTimesLock)
                        {
                            if (lastDownloadTimesCache != null)
                            {
                                lastDownloadTimesCache[store.StoreID] = DateTime.UtcNow;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check the store's license.
        /// </summary>
        private static void CheckLicense(StoreEntity store)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                LicenseService licenseService = lifetimeScope.Resolve<LicenseService>();
                ILicense license = licenseService.GetLicense(store);
                license.Refresh();

                if (license.IsDisabled)
                {
                    throw new ShipWorksLicenseException(license.DisabledReason);
                }

                if (license.IsOverChannelLimit)
                {
                    throw new ShipWorksLicenseException("Channel Limit Exceeded.");
                }
            }
        }

        /// <summary>
        /// Create the window that will be used for displaying progess
        /// </summary>
        private static ProgressDlg CreateProgressWindow()
        {
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "ShipWorks Download";
            progressDlg.Description = "ShipWorks is downloading from your online store.";

            // Implement the hiding
            progressDlg.AllowCloseWhenRunning = true;
            progressDlg.AutoCloseWhenComplete = true;

            progressDlg.ActionColumnHeaderText = "Store";
            progressDlg.CloseTextWhenRunning = "Hide";
            progressDlg.CloseTextWhenComplete = "Close";

            return progressDlg;
        }

        /// <summary>
        /// Create and save the download log at the start of the download.
        /// </summary>
        private static DownloadEntity CreateDownloadLog(StoreEntity store, DownloadInitiatedBy initiatedBy)
        {
            DownloadEntity downloadLog = new DownloadEntity();
            downloadLog.StoreID = store.StoreID;
            downloadLog.ComputerID = UserSession.Computer.ComputerID;
            downloadLog.UserID = initiatedBy == DownloadInitiatedBy.ShipWorks ? SuperUser.Instance.UserID : UserSession.User.UserID;
            downloadLog.InitiatedBy = (int) initiatedBy;
            downloadLog.Started = DateTime.UtcNow;
            downloadLog.Ended = null;
            downloadLog.Result = (int) DownloadResult.Unfinished;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(downloadLog);
            }

            return downloadLog;
        }

        /// <summary>
        /// Get the last time a download for the given store ended
        /// </summary>
        private static DateTime? GetLastDownloadTime(StoreEntity store)
        {
            lock (lastDownloadTimesLock)
            {
                if (lastDownloadTimesCache == null)
                {
                    lastDownloadTimesCache = StoreManager.GetLastDownloadTimes();
                }

                DateTime? lastDownload;
                if (lastDownloadTimesCache.TryGetValue(store.StoreID, out lastDownload))
                {
                    return lastDownload;
                }

                return null;
            }
        }
    }
}
