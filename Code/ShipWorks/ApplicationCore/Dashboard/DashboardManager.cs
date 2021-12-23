using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Email;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// Responsible for maintaining the display of the panel
    /// </summary>
    public static class DashboardManager
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(DashboardManager));

        // Timer for the stopped scheduler notification threshold
        private static readonly System.Windows.Forms.Timer stoppedSchedulerNotificationTimer = new System.Windows.Forms.Timer();

        // Wait 10 minutes between when a scheduler is noticed to be not running and actually notifying the user about it
        private const int stoppedScheduleNotificationThreshold = 600000;

        // The panel that the controller uses to display the panel items
        private static Panel panel;

        // List of dashboard items
        private static List<DashboardItem> dashboardItems = new List<DashboardItem>();

        // The priority of types in how they should be sorted
        private static List<Type> itemTypeSortOrder = new List<Type>
            {
                typeof(DashboardActionErrorItem),
                typeof(DashboardSchedulerServiceStoppedItem),
                typeof(DashboardEmailItem),
                typeof(DashboardStoreItem),
                typeof(DashboardMessageItem),
                typeof(DashboardAccountTrialItem),
                typeof(DashboardLegacyStoreTrialItem),
                typeof(DashboardOnlineVersionItem),
                typeof(DashboardOneBalancePromoItem)
            };

        // The text to display when the dashboard is working in the background
        private static readonly string busyText = "updating dashboard";

        // Work ID for checking tango messages and checking the sw version
        private static Guid checkVersionWorkID;

        // keep track of any local rating issues
        private static ILocalRateValidationResult validationResult;
        private static bool validatingLocalRates;

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

            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                if (lifetimeScope.Resolve<IConfigurationData>().IsArchive())
                {
                    return;
                }
            }

            panel.Visible = true;

            // Don't wait for idle - do these right away
            IdleWatcher.RunWorkNow(checkVersionWorkID);
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
        public static void ShowLocalMessage(string identifier, DashboardLocalMessageDetails options, params DashboardAction[] actions)
        {
            // Dismiss it in case it already exists
            DismissLocalMessage(identifier);

            // Add it back in with the new values
            DashboardLocalMessageItem messageItem = new DashboardLocalMessageItem(identifier, options.ImageType, options.PrimaryText, options.SecondaryText, actions);
            messageItem.UseFriendlyDateTime = options.UseFriendlyDateTime;
            AddDashboardItem(messageItem);
        }

        /// <summary>
        /// Add the given information message to the dashboard
        /// </summary>
        public static void ShowLocalMessage(string identifier, DashboardMessageImageType imageType, string primaryText, string secondaryText, params DashboardAction[] actions) =>
            ShowLocalMessage(identifier, new DashboardLocalMessageDetails { ImageType = imageType, PrimaryText = primaryText, SecondaryText = secondaryText }, actions);

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
        public static void UpdateStoreTypeDependentItems()
        {
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
        public static void UpdateTrialItems()
        {
            Debug.Assert(!Program.MainForm.InvokeRequired);

            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var licenseService = lifetimeScope.Resolve<ILicenseService>();

                if (licenseService.IsLegacy)
                {
                    UpdateLegacyStoreTrialItems(licenseService);
                }
                else
                {
                    UpdateAccountTrialItem(licenseService);
                }
            }
        }

        #region WebReg Account Trial

        /// <summary>
        /// Update the account trial item
        /// </summary>
        private static void UpdateAccountTrialItem(ILicenseService licenseService)
        {
            var license = licenseService.GetLicense(null);
            DashboardAccountTrialItem accountTrialItem = dashboardItems.OfType<DashboardAccountTrialItem>().SingleOrDefault();

            if (accountTrialItem == null)
            {
                if (license.TrialDetails.IsInTrial)
                {
                    ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncLoadAccountTrialDetail),
                        new object[]
                        {
                            license.TrialDetails,
                            ApplicationBusyManager.OperationStarting(busyText)
                        });
                }
            }
            // Refresh the UI in case the number of days has changed, its now expired, or the account is no longer in trial.
            else
            {
                if (license.TrialDetails.IsInTrial)
                {
                    accountTrialItem.UpdateTrialDisplay();
                }
                else
                {
                    RemoveDashboardItem(accountTrialItem);
                }
            }
        }

        /// <summary>
        /// Load account trial information asynchronously
        /// </summary>
        private static void AsyncLoadAccountTrialDetail(object state)
        {
            object[] data = (object[]) state;

            TrialDetails trialDetails = (TrialDetails) data[0];
            ApplicationBusyToken token = (ApplicationBusyToken) data[1];

            try
            {
                panel.BeginInvoke((MethodInvoker<TrialDetails>) AsyncLoadAccountTrialDetailComplete, trialDetails);
            }
            catch (Exception ex) when (ex is ShipWorksLicenseException || ex is TangoException)
            {
                log.Error("Failed to load trial details for account", ex);
            }
            finally
            {
                token.Dispose();
            }
        }

        /// <summary>
        /// The loading of a trial detail has completed. This is back on the UI thread.
        /// </summary>
        private static void AsyncLoadAccountTrialDetailComplete(TrialDetails trialDetails)
        {
            // Dashboard may have closed in the meantime
            if (!IsDashboardOpen)
            {
                return;
            }

            DashboardAccountTrialItem existing = dashboardItems.OfType<DashboardAccountTrialItem>().SingleOrDefault();
            if (existing != null)
            {
                return;
            }

            DashboardAccountTrialItem accountTrialItem = new DashboardAccountTrialItem(trialDetails);
            AddDashboardItem(accountTrialItem);
        }

        #endregion

        #region Legacy Store Trials

        /// <summary>
        /// Update trial items for legacy customer stores
        /// </summary>
        private static void UpdateLegacyStoreTrialItems(ILicenseService licenseService)
        {
            // Add in trial information for each store we don't have yet
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                // If it's not enabled, we ignore it
                if (!store.Enabled)
                {
                    continue;
                }

                var license = licenseService.GetLicense(store);
                if (!license.TrialDetails.IsInTrial)
                {
                    continue;
                }

                // Freemium installs can be in a weird state of signed up for eBay - but not yet for ELS.  But it's not really a trial (from a user perspective) its just
                // what it is in tango b\c it was the simplest way to implement it until we get unified billing.
                if (EditionSerializer.Restore(store) is FreemiumFreeEdition)
                {
                    continue;
                }

                DashboardLegacyStoreTrialItem legacyStoreTrialItem = dashboardItems.OfType<DashboardLegacyStoreTrialItem>()
                    .SingleOrDefault(i => i.Store.StoreID == store.StoreID);
                if (legacyStoreTrialItem == null)
                {
                    ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncLoadLegacyStoreTrialDetail),
                        new object[]
                        {
                            store,
                            license.TrialDetails,
                            ApplicationBusyManager.OperationStarting(busyText)
                        });
                }
                // Refresh the UI in case the days has changed, its now expired, or the store name changed
                else
                {
                    legacyStoreTrialItem.UpdateTrialDisplay(store);
                }
            }

            // Go through each trial making sure they are all still valid stores and valid trials
            foreach (DashboardLegacyStoreTrialItem trialItem in dashboardItems.OfType<DashboardLegacyStoreTrialItem>().ToList())
            {
                StoreEntity store = StoreManager.GetStore(trialItem.Store.StoreID);
                if (store == null || !store.Enabled)
                {
                    RemoveDashboardItem(trialItem);
                }
                else
                {
                    var license = licenseService.GetLicense(store);
                    if (!license.TrialDetails.IsInTrial)
                    {
                        RemoveDashboardItem(trialItem);
                    }
                }
            }
        }

        /// <summary>
        /// Load trial information asynchronously
        /// </summary>
        private static void AsyncLoadLegacyStoreTrialDetail(object state)
        {
            object[] data = (object[]) state;

            StoreEntity store = (StoreEntity) data[0];
            TrialDetails trialDetails = (TrialDetails) data[1];
            ApplicationBusyToken token = (ApplicationBusyToken) data[2];

            try
            {
                panel.BeginInvoke((MethodInvoker<IStoreEntity, TrialDetails>) AsyncLoadLegacyStoreTrialDetailComplete, store, trialDetails);
            }
            catch (Exception ex) when (ex is ShipWorksLicenseException || ex is TangoException)
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
        private static void AsyncLoadLegacyStoreTrialDetailComplete(IStoreEntity storeEntity, TrialDetails trialDetails)
        {
            // Dashboard may have closed in the meantime
            if (!IsDashboardOpen)
            {
                return;
            }

            DashboardLegacyStoreTrialItem existing = dashboardItems.OfType<DashboardLegacyStoreTrialItem>().SingleOrDefault(i => i.Store.StoreID == storeEntity.StoreID);
            if (existing != null)
            {
                return;
            }

            DashboardLegacyStoreTrialItem legacyStoreTrialItem = new DashboardLegacyStoreTrialItem(storeEntity, trialDetails);
            AddDashboardItem(legacyStoreTrialItem);
        }

        #endregion

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

            dashboardItems.OfType<DashboardAccountTrialItem>().SingleOrDefault()?.UpdateTrialDisplay();

            foreach (DashboardLegacyStoreTrialItem legacyStoreTrialItem in dashboardItems.OfType<DashboardLegacyStoreTrialItem>())
            {
                legacyStoreTrialItem.UpdateTrialDisplay();
            }

            SortDashboardItems();
        }

        /// <summary>
        /// Runs on the background thread to check the latest ShipWorks version
        /// </summary>
        private static void AsyncCheckShipWorksVersion()
        {
            log.InfoFormat("Checking ShipWorks version...");

            try
            {
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var now = DateTime.Now;
                    var nextUpdateWindow = lifetimeScope.Resolve<IConfigurationData>().GetNextUpdateWindow(now);
                    var timeUntilUpdateWindow = nextUpdateWindow.Subtract(now);

                    if (Math.Round(timeUntilUpdateWindow.TotalDays) <= 7 && timeUntilUpdateWindow.TotalHours > 0)
                    {
                        var tangoCustomerId = lifetimeScope.Resolve<ITangoWebClient>().GetTangoCustomerId();
                        var currentVersion = lifetimeScope.Resolve<ISqlSchemaUpdater>().GetBuildVersion();

                        lifetimeScope.Resolve<ITangoGetReleaseByUserRequest>()
                            .GetReleaseInfo(tangoCustomerId, currentVersion)
                            .Map(x => x.ReleaseVersion)
                            .Filter(x => x > currentVersion)
                            .Filter(x => x > ShipWorksOnlineVersionChecker.CheckSignedOffVersion())
                            .Do(x => panel.BeginInvoke((MethodInvoker<DashboardOnlineVersionItem>) CheckShipWorksVersionComplete,
                                new DashboardOnlineVersionItem(x, nextUpdateWindow)));
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
                existing.CopyFrom(versionItem);
            }
            else
            {
                AddDashboardItem(versionItem);
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

            CheckForEmailChanges();
            CheckForActionChanges();
            CheckForSchedulerServiceStoppedChanges();
            CheckForLicenseDependentChanges();
            CheckForOneBalanceChanges();
            ValidateLocalRates();
            CheckQuickStartNeeded();
        }

        /// <summary>
        /// If no store set up, show quick start
        /// </summary>
        private static void CheckQuickStartNeeded()
        {
            const string identifier = "QuickStart";
            var existingDashboardItem = dashboardItems.OfType<DashboardLocalMessageItem>().SingleOrDefault(i => i.Identifier == identifier);
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IQuickStart quickStart = lifetimeScope.Resolve<IQuickStart>();
                bool shouldShow = quickStart.ShouldShow;

                if (existingDashboardItem == null && shouldShow)
                {
                    var quickStartDashboardItem =
                        new DashboardLocalMessageItem(identifier,
                                                      DashboardMessageImageType.LightBulb,
                                                      "Quick Start",
                                                      "Finish setting up ShipWorks.",
                                                      new DashboardActionMethod(
                                                          "[link]Quick Start[/link]", ShowQuickStart))
                        { ShowTime = false };

                    AddDashboardItem(quickStartDashboardItem);
                    quickStartDashboardItem.DashboardBar.CanUserDismiss = false;
                }

                if (existingDashboardItem != null && !shouldShow)
                {
                    RemoveDashboardItem(existingDashboardItem);
                }
            }
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        private static void ShowQuickStart()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                lifetimeScope.Resolve<IQuickStart>().ShowDialog();
            }
        }

        /// <summary>
        /// Validate local rates
        /// </summary>
        private static void ValidateLocalRates()
        {
            if (validationResult == null && !validatingLocalRates)
            {
                validatingLocalRates = true;
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IUpsLocalRateValidator rateValidator = lifetimeScope.Resolve<IUpsLocalRateValidator>();

                    Task.Run(() =>
                    {
                        validationResult = rateValidator.ValidateRecentShipments();
                        DashboardLocalMessageItem localRateDashboardMessage = validationResult.CreateDashboardMessage() as DashboardLocalMessageItem;

                        if (localRateDashboardMessage != null)
                        {
                            panel.BeginInvoke(new MethodInvoker(() =>
                            {
                                DismissLocalMessage(localRateDashboardMessage.Identifier);
                                AddDashboardItem(localRateDashboardMessage);
                            }));
                        }
                    });
                }
                validatingLocalRates = false;
            }
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
                else
                {
                    // There's already a dashboard item; confirm whether it needs to be swapped out based
                    // on the delta of the IsUnderShipmentLimit property, so we get the appropriate messaging
                    if (existingItem.IsUnderShipmentLimit != dashboardItem.IsUnderShipmentLimit)
                    {
                        dashboardItems.OfType<DashboardLicenseItem>().ToList().ForEach(RemoveDashboardItem);
                        AddDashboardItem(dashboardItem);
                    }
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
                    EmailOutboundFields.SendStatus != (int) EmailOutboundStatus.Sent &
                    (EmailOutboundFields.DontSendBefore == DBNull.Value | EmailOutboundFields.DontSendBefore <= DateTime.UtcNow) &
                    EmailOutboundFields.ComposedDate < DateTime.UtcNow.AddMinutes(-1));

                if (unsent > 0)
                {
                    errors = EmailOutboundCollection.GetCount(SqlAdapter.Default, EmailOutboundFields.SendStatus == (int) EmailOutboundStatus.Failed);
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
        /// Check for any changes in the state of the One Balance account
        /// </summary>
        private static void CheckForOneBalanceChanges()
        {
            var oneBalanceItem = dashboardItems.OfType<DashboardOneBalancePromoItem>().SingleOrDefault();
            if (UspsAccountManager.UspsAccountsReadOnly.Any(e => !string.IsNullOrEmpty(e.ShipEngineCarrierId)))
            {
                if (oneBalanceItem != null)
                {
                    RemoveDashboardItem(oneBalanceItem);
                }
            }
        }

        /// <summary>
        /// Show the One Balance promo in the dashboard
        /// </summary>
        public static void ShowOneBalancePromo()
        {
            using (var scope = IoC.BeginLifetimeScope())
            {
                var licenses = scope.Resolve<ILicenseService>().GetLicenses();

                // Show the promo if the customer isn't CTP
                var oneBalanceItem = dashboardItems.OfType<DashboardOneBalancePromoItem>().SingleOrDefault();
                if (oneBalanceItem == null && !licenses.Any(x => x.IsCtp))
                {
                    oneBalanceItem = new DashboardOneBalancePromoItem();
                    AddDashboardItem(oneBalanceItem);
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
                else if (left is DashboardLegacyStoreTrialItem && right is DashboardLegacyStoreTrialItem)
                {
                    return ((DashboardLegacyStoreTrialItem) left).Store.StoreName.CompareTo(((DashboardLegacyStoreTrialItem) right).Store.StoreName);
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
