using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Timers;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Handles checking for updates during the upgrade window.
    /// </summary>
    [Component]
    public class UpgradeTimeWindow : IUpgradeTimeWindow
    {
        int minutesBetweenWindowChecks = 15;
        private IShipWorksUpgrade shipWorksUpgrade;
        Timer upgradeTimer;
        Timer checkUpgradeWindowTimer;
        private static readonly ILog log = LogManager.GetLogger(typeof(UpgradeTimeWindow));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipWorksUpgrade"></param>
        public UpgradeTimeWindow(IShipWorksUpgrade shipWorksUpgrade)
        {
            log.Info("Constructing UpgradeTimeWindow");
            this.shipWorksUpgrade = shipWorksUpgrade;
            checkUpgradeWindowTimer = new Timer(TimeSpan.FromMinutes(minutesBetweenWindowChecks).TotalMilliseconds);
            checkUpgradeWindowTimer.Elapsed += OnCheckUpgradeWindowTimerElapsed;
            checkUpgradeWindowTimer.AutoReset = true;
            checkUpgradeWindowTimer.Enabled = true;
        }

        /// <summary>
        /// Call GetUpdateWindow when timmer elapses
        /// </summary>
        private void OnCheckUpgradeWindowTimerElapsed(object sender, ElapsedEventArgs e)
        {
            log.Info("CheckUpgradeWindowTimerElapsed");
            CallGetUpdateWindow();
        }

        /// <summary>
        /// Call swc.exe with getupdatewindow parameter
        /// </summary>
        public static void CallGetUpdateWindow()
        {

            string process = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\swc.exe";
            string arg = "/command=getupdatewindow";
            log.InfoFormat("Executing {0} {1}", process, arg);
            Process.Start(process, arg);
        }

        /// <summary>
        /// Update the window
        /// </summary>
        internal void UpdateWindow(UpdateWindowData updateWindowData)
        {
            log.Info("Updating Window");
            log.InfoFormat("Getting update window for Day {0} and Hour {1}",
                updateWindowData.AutoUpdateDayOfWeek,
                updateWindowData.AutoUpdateHourOfDay);

            DateTime dateOfNextUpgrade;

            dateOfNextUpgrade = DateTime.Today.TodayOrNext(updateWindowData.AutoUpdateDayOfWeek);
            
            log.InfoFormat("Date before adding hours: {0}", dateOfNextUpgrade);

            dateOfNextUpgrade = dateOfNextUpgrade.AddHours(updateWindowData.AutoUpdateHourOfDay);

            log.InfoFormat("After updating hours: {0}", dateOfNextUpgrade);

            if (dateOfNextUpgrade < DateTime.Now)
            {
                dateOfNextUpgrade = dateOfNextUpgrade.AddDays(7);
            }

            log.InfoFormat("Resetting for next week (if needed): {0}", dateOfNextUpgrade);

            upgradeTimer?.Dispose();
            double millisecondsToOpenWindow = (dateOfNextUpgrade - DateTime.Now).TotalMilliseconds;

            log.InfoFormat("Setting for {0} milliseconds", millisecondsToOpenWindow);
            upgradeTimer = new Timer(millisecondsToOpenWindow);
     
            upgradeTimer.Elapsed += async (sender, e) => 
                await shipWorksUpgrade.Upgrade(updateWindowData.TangoCustomerId);

            upgradeTimer.AutoReset = false;
            upgradeTimer.Enabled = true;
        }        
    }
}