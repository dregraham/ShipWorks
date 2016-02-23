using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using log4net;
using System.Diagnostics;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Properties;
using System.Threading;
using ShipWorks.ApplicationCore.Interaction;
using Interapptive.Shared.Utility;
using System.Net;
using System.Reflection;
using System.Xml;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Users;
using System.Collections;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Platforms;
using Interapptive.Shared.Net;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.ApplicationCore.Services;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// Responsible for maintaining the display of the panel
    /// </summary>
    public static class DashboardManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DashboardManager));

        // Timer for the stopped scheduler notification threshold
        private static readonly System.Windows.Forms.Timer stoppedSchedulerNotificationTimer = new System.Windows.Forms.Timer();

        // Wait 10 minutes between when a scheduler is noticed to be not running and actually notifying the user about it
        private const int stoppedScheduleNotificationThreshold = 600000;

        // The panel that the controller uses to display the panel items
        static Panel panel;

        // List of dashboard items
        static List<DashboardItem> dashboardItems = new List<DashboardItem>();

        // The priority of types in how they should be sorted
        static List<Type> itemTypeSortOrder = new List<Type>
            {
                typeof(DashboardActionErrorItem),
                typeof(DashboardSchedulerServiceStoppedItem),
                typeof(DashboardEmailItem),
                typeof(DashboardStoreItem),
                typeof(DashboardMessageItem),
                typeof(DashboardTrialItem),
                typeof(DashboardOnlineVersionItem)
            };

        // The text to display when the dashboard is working in the background
        static string busyText = "updating dashboard";

        // Work ID for checking tango messages and checking the sw version
        static Guid checkVersionWorkID;
        static Guid tangoMessageWorkID;

        /// <summary>
        /// Initialize once per application run
        /// </summary>
        public static void InitializeForApplication(Panel dashboardPanel)
        {
            if (dashboardPanel == null)
            {
                throw new ArgumentNullException("dashboardPanel");
            }

            if (panel != null)
            {
                throw new InvalidOperationException("The DashboardManager has already been initialized.");
            }

            panel = dashboardPanel;

            panel.Visible = false;
            panel.Controls.Clear();
            panel.BackColor = Color.Transparent;
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // In case they keep ShipWorks open forever at a time, check every so often
            IdleWatcher.RegisterDatabaseDependentWork("DashboardTrialsUpdate", UpdateTrialDaysDisplay, busyText, TimeSpan.FromHours(2));

            // Check for a new version of ShipWorks
            checkVersionWorkID = IdleWatcher.RegisterDatabaseDependentWork("DashboardVersionCheck", AsyncCheckShipWorksVersion, busyText, TimeSpan.FromHours(2));

            // Check for updates from tango.  This will also be manually done at other important times, like after downloading or shipping.
            tangoMessageWorkID = IdleWatcher.RegisterDatabaseDependentWork("DashboardMessagesCheck", AsyncCheckLatestServerMessages, busyText, TimeSpan.FromHours(.5));

            UpdateLayout();

            stoppedSchedulerNotificationTimer.Interval = stoppedScheduleNotificationThreshold;
            stoppedSchedulerNotificationTimer.Tick += OnStoppedSchedulerNotificationTimerTick;
        }

        /// <summary>
        /// Some UI properties of the panel change depending on if it has content
        /// </summary>
        private static void UpdateLayout()
        {
            panel.Padding = (panel.Controls.Count > 0) ? new Padding(3, 3, 3, 2) : new Padding(0);
        }

        /// <summary>
        /// Open the panel and begin the async initialization of the contents.
        /// </summary>
        public static void OpenDashboard()
        {
            // Ensure we start our fresh
            panel.Controls.Clear();
            dashboardItems.Clear();

            UpdateLayout();
            panel.Visible = true;

            // Don't wait for idle - do these right away
            IdleWatcher.RunWorkNow(checkVersionWorkID);
            IdleWatcher.RunWorkNow(tangoMessageWorkID);
       }

        /// <summary>
        /// Clear the contents of the panel.  Does not dismiss any messages, just removes them.  Called when a user logs off.
        /// </summary>
        public static void CloseDashboard()
        {
            panel.Controls.Clear();
            panel.Visible = false;

            dashboardItems.Clear();

            UpdateLayout();
        }

        /// <summary>
        /// Indicates if the dashboard is currently in the open state
        /// </summary>
        public static bool IsDashboardOpen
        {
            get
            {
                return panel != null && panel.Visible;
            }
        }

        /// <summary>
        /// Add the given information message to the dashboard
        /// </summary>
        public static void ShowLocalMessage(string identifier, DashboardMessageImageType imageType, string primaryText, string secondaryText, params DashboardAction[] actions)
        {
            // Dismiss it in case it already exists
            DismissLocalMessage(identifier);

            // Add it back in with the new values
            DashboardLocalMessageItem messageItem = new DashboardLocalMessageItem(identifier, imageType, primaryText, secondaryText, actions);
            AddDashboardItem(messageItem);
        }

        /// <summary>
        /// Dismiss the message with the given identifier.  If the message is not present, no action is taken.
        /// </summary>
        public static void DismissLocalMessage(string identifier)
        {
            DashboardLocalMessageItem item = dashboardItems.OfType<DashboardLocalMessageItem>().SingleOrDefault(i => i.Identifier == identifier);

            if (item != null)
            {
                DismissItemAndBar(item);
            }
        }

        /// <summary>
        /// Update all dashboard items that are dependent on the current set of StoreType's
        /// </summary>
        public static void UpdateStoreDependentItems()
        {
            UpdateTrialItems();

            List<DashboardStoreItem> currentItems = new List<DashboardStoreItem>();

            // Give each store a chance to create its messages, but only the ones that are enabled
            foreach (StoreType storeType in StoreManager.GetStoreTypeInstances().Where(st => st.Store.Enabled))
            {
                var storeMessages = storeType.CreateDashboardMessages();

                if (storeMessages != null)
                {
                    currentItems.AddRange(storeMessages);

                    // Add in each message
                    foreach (DashboardStoreItem storeItem in storeMessages)
                    {
                        // See if it already exists
                        DashboardStoreItem existing = dashboardItems.OfType<DashboardStoreItem>().SingleOrDefault(i => i.Identifier == storeItem.Identifier);

                        // If it exists, just update it in place
                        if (existing != null)
                        {
                            existing.Update(storeItem);
                        }
                        else
                        {
                            AddDashboardItem(storeItem);
                        }
                    }
                }
            }

            // Remove any messages that no longer exist
            foreach (DashboardStoreItem removedItem in dashboardItems.OfType<DashboardStoreItem>().Where(i => !currentItems.Select(c => c.Identifier).Contains(i.Identifier)).ToList())
            {
                DismissItemAndBar(removedItem);
            }

            SortDashboardItems();
            UpdateLayout();
        }

        /// <summary>
        /// Update the trial display items
        /// </summary>
        private static void UpdateTrialItems()
        {
            Debug.Assert(!Program.MainForm.InvokeRequired);

            // Add in trial information for each store we don't have yet
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                // If it's not enabled, we ignore it
                if (!store.Enabled)
                {
                    continue;
                }

                ShipWorksLicense license = new ShipWorksLicense(store.License);
                if (!license.IsTrial)
                {
                    continue;
                }

                // Freemium installs can be in a weird state of signed up for eBay - but not yet for ELS.  But it's not really a trial (from a user perspective) its just
                // what it is in tango b\c it was the simplest way to implement it until we get unified billing.
                if (EditionSerializer.Restore(store) is FreemiumFreeEdition)
                {
                    continue;
                }

                DashboardTrialItem trialItem = dashboardItems.OfType<DashboardTrialItem>().Where(i => i.TrialDetail.Store.StoreID == store.StoreID).SingleOrDefault();
                if (trialItem == null)
                {
                    ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncLoadTrialDetail),
                        new object[] {
                            store,
                            ApplicationBusyManager.OperationStarting(busyText) });
                }
                // Refresh the UI in case the days has changed or its now expired.
                else
                {
                    trialItem.UpdateTrialDisplay();
                }
            }

            // Go through each trial making sure they are all still valid stores and valid trials
            foreach (DashboardTrialItem trialItem in dashboardItems.OfType<DashboardTrialItem>().ToList())
            {
                StoreEntity store = StoreManager.GetStore(trialItem.TrialDetail.Store.StoreID);
                if (store == null || !store.Enabled)
                {
                    RemoveDashboardItem(trialItem);
                }
                else
                {
                    ShipWorksLicense license = new ShipWorksLicense(store.License);
                    if (!license.IsTrial)
                    {
                        RemoveDashboardItem(trialItem);
                    }
                }
            }
        }

        /// <summary>
        /// Update the day count displayed next to each trial.  For users who leave ShipWorks open all the time this helps them still
        /// see the days count down.
        /// </summary>
        private static void UpdateTrialDaysDisplay()
        {
            if (panel.InvokeRequired)
            {
                panel.BeginInvoke((MethodInvoker) UpdateTrialDaysDisplay);
                return;
            }

            // It may be closed by the time we make it to the UI thread
            if (!IsDashboardOpen)
            {
                return;
            }

            foreach (DashboardTrialItem trialItem in dashboardItems.OfType<DashboardTrialItem>())
            {
                trialItem.UpdateTrialDisplay();
            }

            SortDashboardItems();
        }

        /// <summary>
        /// Load trial information asyncronously
        /// </summary>
        private static void AsyncLoadTrialDetail(object state)
        {
            object[] data = (object[]) state;

            StoreEntity store = (StoreEntity) data[0];
            ApplicationBusyToken token = (ApplicationBusyToken) data[1];

            try
            {
                TrialDetail trialDetail = TangoWebClient.GetTrial(store);

                panel.BeginInvoke((MethodInvoker<TrialDetail>) AsyncLoadTrialDetailComplete, trialDetail);
            }
            catch (ShipWorksLicenseException ex)
            {
                log.Error("Failed to load trial details for store " + store.StoreID, ex);
            }
            catch (TangoException ex)
            {
                log.Error("Failed to load trial details for store " + store.StoreID, ex);
            }
            finally
            {
                token.Dispose();
            }
        }

        /// <summary>
        /// The loading of a trial detail has completed.  This is back on the UI thread.
        /// </summary>
        private static void AsyncLoadTrialDetailComplete(TrialDetail trialDetail)
        {
            // Dashboard may have closed in the meantime
            if (!IsDashboardOpen)
            {
                return;
            }

            DashboardTrialItem existing = dashboardItems.OfType<DashboardTrialItem>().Where(i => i.TrialDetail.Store.StoreID == trialDetail.Store.StoreID).SingleOrDefault();
            if (existing != null)
            {
                return;
            }

            DashboardTrialItem trialItem = new DashboardTrialItem(trialDetail);
            AddDashboardItem(trialItem);
        }

        /// <summary>
        /// Runs on the background thread to check the latest ShipWorks version
        /// </summary>
        private static void AsyncCheckShipWorksVersion()
        {
            log.InfoFormat("Checking ShipWorks version...");

            try
            {
                ShipWorksOnlineVersion online = ShipWorksOnlineVersionChecker.CheckOnlineVersion();
                Version running = Assembly.GetEntryAssembly().GetName().Version;

                // Online is a more recent version
                if (running.CompareTo(online.Version) < 0)
                {
                    // See if this newer version has already been acknowledged
                    Version signoff = ShipWorksOnlineVersionChecker.CheckSignedOffVersion();

                    // If online is still more recent
                    if (signoff.CompareTo(online.Version) < 0)
                    {
                        panel.BeginInvoke((MethodInvoker<DashboardOnlineVersionItem>) CheckShipWorksVersionComplete,
                            new DashboardOnlineVersionItem(online));
                    }
                }
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    log.Error("Failed to check ShipWorks online version.", ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Done checking what the latest version of ShipWorks is
        /// </summary>
        private static void CheckShipWorksVersionComplete(DashboardOnlineVersionItem versionItem)
        {
            // Could have been closed by the time this posted to the UI
            if (!IsDashboardOpen)
            {
                return;
            }

            // See if we already have a version dashboard item
            DashboardOnlineVersionItem existing = dashboardItems.OfType<DashboardOnlineVersionItem>().SingleOrDefault();

            // If we do, just update it
            if (existing != null)
            {
                existing.OnlineVersion = versionItem.OnlineVersion;
            }
            else
            {
                AddDashboardItem(versionItem);
            }
        }

        /// <summary>
        /// Initiate a check of the latest tango server messages
        /// </summary>
        public static void DownloadLatestServerMessages()
        {
            IdleWatcher.RunWorkNow(tangoMessageWorkID);
        }

        /// <summary>
        /// Runs in the background to check the lastest messages from the interapptive server
        /// </summary>
        private static void AsyncCheckLatestServerMessages()
        {
            log.InfoFormat("Checking server messages...");

            try
            {
                // First make sure we have all the latest messages
                ServerMessageManager.CheckLatestServerMessages();

                // When we're done with that, we'll need to refresh the UI
                panel.BeginInvoke(new MethodInvoker(CheckForChanges));
            }
            catch (ServerMessageFeedException ex)
            {
                log.Error("Failed to read server message feed.", ex);
                Debug.Fail(ex.Message, ex.ToString());
            }
        }

        /// <summary>
        /// Check the database for any changes to server messages or other messages tha need displayed
        /// </summary>
        public static void CheckForChanges()
        {
            // By the time we got posted to the UI the dashboard could have closed
            if (!IsDashboardOpen)
            {
                return;
            }

            CheckForServerMessageChanges();
            CheckForEmailChanges();
            CheckForActionChanges();
            CheckForSchedulerServiceStoppedChanges();
            CheckForLicenseDependentChanges();
        }

        /// <summary>
        /// Update the dashboard with license issues
        /// </summary>
        private static void CheckForLicenseDependentChanges()
        {
            DashboardLicenseItem dashboardItem;

            // Resolve the license and get its dashboard item
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                dashboardItem = licenseService.GetLicenses().FirstOrDefault()?.CreateDashboardMessage();
            }

            if (dashboardItem == null)
            {
                // the license returned no dashboard license item so we remove any existing
                // dashboard items of type DashboardLicenseItem
                dashboardItems.OfType<DashboardLicenseItem>().ToList().ForEach(RemoveDashboardItem);
            }
            else
            {
                DashboardLicenseItem existingItem = dashboardItems.OfType<DashboardLicenseItem>().SingleOrDefault();

                if (existingItem == null)
                {
                    // The license returned a valid DashboardLicenseItem, add it to the dashboard
                    AddDashboardItem(dashboardItem);
                }
            }
        }

        /// <summary>
        /// Check for any changes in the database for scheduler service stopped messages
        /// </summary>
        private static void CheckForSchedulerServiceStoppedChanges()
        {
            // Check the database for any changes
            bool areAnyRequiredSchedulersStopped = ServiceStatusManager.GetComputersRequiringShipWorksService().Any();
            List<DashboardSchedulerServiceStoppedItem> existingDashboardItems = dashboardItems.OfType<DashboardSchedulerServiceStoppedItem>().ToList<DashboardSchedulerServiceStoppedItem>();

            // If the message is already there or the notification timer has started we don't have to do anything
            if (areAnyRequiredSchedulersStopped && !existingDashboardItems.Any() && !stoppedSchedulerNotificationTimer.Enabled)
            {
                stoppedSchedulerNotificationTimer.Start();
            }
            else if (!areAnyRequiredSchedulersStopped)
            {
                // We can abandon the notification threshold since all schedulers are now running
                if (stoppedSchedulerNotificationTimer.Enabled)
                {
                    stoppedSchedulerNotificationTimer.Stop();
                }

                // The stopped services are now running, so remove the stopped dashboard item.
                if (existingDashboardItems.Any())
                {
                    existingDashboardItems.ForEach(RemoveDashboardItem);
                }
            }
        }

        /// <summary>
        /// Timer callback that is used to show the schedule stopped notification after a waiting period
        /// </summary>
        private static void OnStoppedSchedulerNotificationTimerTick(object sender, EventArgs eventArgs)
        {
            // Disable the timer
            stoppedSchedulerNotificationTimer.Stop();
            stoppedSchedulerNotificationTimer.Enabled = false;

            // Show the dashboard item
            DashboardSchedulerServiceStoppedItem item = new DashboardSchedulerServiceStoppedItem();
            AddDashboardItem(item);
        }

        /// <summary>
        /// Check for any changes in the database for server messages
        /// </summary>
        private static void CheckForServerMessageChanges()
        {
            // Check the database for any changes
            ServerMessageManager.CheckDatabaseForChanges();

            // Active messages are those that have not in some way been dismissed, and match the criteria of the current user and stores.
            List<ServerMessageEntity> activeMessages = ServerMessageManager.ActiveMessages.ToList();

            // Now we can get the list of messages that we need to display
            foreach (ServerMessageEntity serverMessage in activeMessages)
            {
                DashboardServerMessageItem existing = dashboardItems.OfType<DashboardServerMessageItem>().Where(
                    i => i.ServerMessage.ServerMessageID == serverMessage.ServerMessageID
                    ).SingleOrDefault();

                // If the message is already there we don't have to do anything
                if (existing == null)
                {
                    DashboardServerMessageItem item = new DashboardServerMessageItem(serverMessage);
                    AddDashboardItem(item);
                }
            }

            // We also have to make sure we are no longer displaying messages that are no longer active
            foreach (DashboardServerMessageItem displayedItem in dashboardItems.OfType<DashboardServerMessageItem>().ToList())
            {
                // See if this item we are displaying is still in the active list
                if (!activeMessages.Exists(s => s.ServerMessageID == displayedItem.ServerMessage.ServerMessageID))
                {
                    // It's no longer active, so we've got to get rid of it.
                    RemoveDashboardItem(displayedItem);
                }
            }
        }

        /// <summary>
        /// Check for any changes in the state of our email sending
        /// </summary>
        private static void CheckForEmailChanges()
        {
            int unsent = 0;
            int errors = 0;

            SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -6, "DashboardManager.CheckForEmailChanges");
            sqlAdapterRetry.ExecuteWithRetry(() =>
            {
                // Unsent messages - but only ones that should have been sent by now
                unsent = EmailOutboundCollection.GetCount(SqlAdapter.Default,
                    EmailOutboundFields.SendStatus != (int)EmailOutboundStatus.Sent &
                    (EmailOutboundFields.DontSendBefore == DBNull.Value | EmailOutboundFields.DontSendBefore <= DateTime.UtcNow) &
                    EmailOutboundFields.ComposedDate < DateTime.UtcNow.AddMinutes(-1));

                if (unsent > 0)
                {
                    errors = EmailOutboundCollection.GetCount(SqlAdapter.Default, EmailOutboundFields.SendStatus == (int)EmailOutboundStatus.Failed);
                }
            });

            // See if we already have an email dashboard item
            DashboardEmailItem emailItem = dashboardItems.OfType<DashboardEmailItem>().SingleOrDefault();

            if (unsent > 0)
            {
                // If we are currently emailing or actioning, give them a chance to clear the email queue before showing
                // that there are any.
                if (emailItem == null && !EmailCommunicator.IsEmailing && !ActionProcessingContext.IsProcessing)
                {
                    emailItem = new DashboardEmailItem();
                    AddDashboardItem(emailItem);
                }

                if (emailItem != null)
                {
                    emailItem.UpdateContent(unsent, errors);
                }
            }
            else
            {
                if (emailItem != null)
                {
                    RemoveDashboardItem(emailItem);
                }
            }
        }

        /// <summary>
        /// Check for any changes in the state of failed actions
        /// </summary>
        public static void CheckForActionChanges()
        {
            int errors = 0;

            SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -5, "ActionQueueCollection.GetCount");
            sqlAdapterRetry.ExecuteWithRetry(() => errors = ActionQueueCollection.GetCount(SqlAdapter.Default, ActionQueueFields.Status == (int) ActionQueueStatus.Error));

            // See if we already have the error displayed for actions
            DashboardActionErrorItem actionItem = dashboardItems.OfType<DashboardActionErrorItem>().SingleOrDefault();

            if (errors > 0)
            {
                if (actionItem == null)
                {
                    actionItem = new DashboardActionErrorItem();
                    AddDashboardItem(actionItem);
                }

                actionItem.UpdateContent(errors);
            }
            else
            {
                if (actionItem != null)
                {
                    RemoveDashboardItem(actionItem);
                }
            }
        }

        /// <summary>
        /// Add the given item to the dashboard
        /// </summary>
        private static void AddDashboardItem(DashboardItem item)
        {
            // Create a new bar and initialize the item with it
            DashboardBar bar = new DashboardBar();
            item.Initialize(bar);

            // Start out not visible until we get it sorted to the right location
            bar.Visible = false;
            panel.SuspendLayout();
            panel.Controls.Add(bar);

            // Add the item and sort them
            dashboardItems.Add(item);
            SortDashboardItems();

            // Now make it visible
            panel.ResumeLayout();
            bar.Visible = true;

            UpdateLayout();

            // We need to know when the bar gets dismissed
            bar.Dismissed += new EventHandler(OnDismissLinkClicked);
        }

        /// <summary>
        /// Remove the given item from the dashboard
        /// </summary>
        private static void RemoveDashboardItem(DashboardItem item)
        {
            dashboardItems.Remove(item);
            panel.Controls.Remove(item.DashboardBar);

            // cleanup, or we leak handles
            item.DashboardBar.Dismissed -= new EventHandler(OnDismissLinkClicked);
            item.DashboardBar.Dispose();

            UpdateLayout();
        }

        /// <summary>
        /// Sort the dashboard items
        /// </summary>
        private static void SortDashboardItems()
        {
            panel.SuspendLayout();

            dashboardItems.Sort(DashboardItemSorter);
            dashboardItems.Reverse();

            // Now apply the same relative indices to the acutal controls collection
            for (int i = 0; i < dashboardItems.Count; i++)
            {
                panel.Controls.SetChildIndex(dashboardItems[i].DashboardBar, i);
            }

            panel.ResumeLayout();
        }

        /// <summary>
        /// Responsible for sorting the items in the correct display order for the user
        /// </summary>
        private static int DashboardItemSorter(DashboardItem left, DashboardItem right)
        {
            if (left == right)
            {
                return 0;
            }

            int leftOrder = itemTypeSortOrder.FindIndex(t => t.IsAssignableFrom(left.GetType()));
            int rightOrder = itemTypeSortOrder.FindIndex(t => t.IsAssignableFrom(right.GetType()));

            if (leftOrder == rightOrder)
            {
                if (left is DashboardMessageItem && right is DashboardMessageItem)
                {
                    return -((DashboardMessageItem) left).Timestamp.CompareTo(((DashboardMessageItem) right).Timestamp);
                }
                else if (left is DashboardStoreItem && right is DashboardStoreItem)
                {
                    return ((DashboardStoreItem) left).StoreName.CompareTo(((DashboardStoreItem) right).StoreName);
                }
                else if (left is DashboardTrialItem && right is DashboardTrialItem)
                {
                    return ((DashboardTrialItem) left).TrialDetail.Store.StoreName.CompareTo(((DashboardTrialItem) right).TrialDetail.Store.StoreName);
                }
            }

            return leftOrder.CompareTo(rightOrder);
        }

        /// <summary>
        /// A dashboard item is being dismissed
        /// </summary>
        private static void OnDismissLinkClicked(object sender, EventArgs e)
        {
            DashboardBar bar = (DashboardBar) sender;
            DashboardItem item = dashboardItems.Where(i => i.DashboardBar == bar).Single();

            DismissItemAndBar(item);
        }

        /// <summary>
        /// Dismiss the given item and remove its associated dashboard bar
        /// </summary>
        private static void DismissItemAndBar(DashboardItem item)
        {
            // Give the item a chance to process what it needs to for dismissal
            item.Dismiss();

            // Remove it from our UI
            RemoveDashboardItem(item);
        }
    }
}
