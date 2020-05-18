using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Timers;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Settings;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Handles checking for updates during the upgrade window.
    /// </summary>
    [Component]
    public class UpgradeTimeWindow : IUpgradeTimeWindow
    {
        int minutesBetweenWindowChecks = 60;
        private IShipWorksUpgrade shipWorksUpgrade;
        Timer upgradeTimer;
        Timer checkUpgradeWindowTimer;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipWorksUpgrade"></param>
        public UpgradeTimeWindow(IShipWorksUpgrade shipWorksUpgrade, Func<Type, ILog> logFactory)
        {
            log = logFactory(GetType());
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
        public void CallGetUpdateWindow()
        {
            string process = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\swc.exe";
            string arg = "/command=getupdatewindow";
            log.InfoFormat("Executing {0} {1}", process, arg);
            Process.Start(process, arg);
        }

        /// <summary>
        /// Update the window
        /// </summary>
        public void UpdateWindow(UpdateWindowData updateWindowData)
        {
            log.Info("Updating Window");
            log.InfoFormat("Getting update window for Day {0} and Hour {1}",
                updateWindowData.AutoUpdateDayOfWeek,
                updateWindowData.AutoUpdateHourOfDay);

            DateTime dateOfNextUpgrade;

            dateOfNextUpgrade = DateTime.Today.TodayOrNext(updateWindowData.AutoUpdateDayOfWeek);

            log.InfoFormat("Date before adding hours: {0}", dateOfNextUpgrade);

            dateOfNextUpgrade = dateOfNextUpgrade.AddHours(updateWindowData.AutoUpdateHourOfDay);
            dateOfNextUpgrade = dateOfNextUpgrade.AddMinutes(GetStartMinutesFromFile());

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
            {
                try
                {
                    log.InfoFormat("Auto Update Enabled: {0}", AutoUpdateSettings.IsAutoUpdateDisabled);
                    if (!AutoUpdateSettings.IsAutoUpdateDisabled)
                    {
                        await shipWorksUpgrade.Upgrade(updateWindowData.TangoCustomerId);
                    }
                    else
                    {
                        log.Info("Not updating. Auto update was disabled after the timer started.");
                    }
                }
                catch(Exception ex)
                {
                    log.ErrorFormat("AutoUpdate Failed: {0}", ex.Message);
                }
            };

            upgradeTimer.AutoReset = false;
            upgradeTimer.Enabled = true;
        }

        /// <summary>
        /// Attempt to read file to add to the start of the window - For easier testing.
        /// </summary>
        private double GetStartMinutesFromFile()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpgradeWindowMinutes.txt");
                string text = File.ReadAllText(path);
                return double.TryParse(text, out double result) ? result : 0;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Not adjusting time for QA. {0}", ex.Message);
                return 0;
            }
        }
    }
}