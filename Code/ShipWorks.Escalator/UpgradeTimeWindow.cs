using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Utility;

namespace ShipWorks.Escalator
{
    internal class UpgradeTimeWindow
    {
        private ShipWorksUpgrade shipWorksUpgrade;
        Timer upgradeTimer;
        Timer checkUpgradeWindowTimer;

        public UpgradeTimeWindow(ShipWorksUpgrade shipWorksUpgrade)
        {
            this.shipWorksUpgrade = shipWorksUpgrade;
            checkUpgradeWindowTimer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            checkUpgradeWindowTimer.Elapsed += OnCheckUpgradeWindowTimerElapsed;
            checkUpgradeWindowTimer.AutoReset = true;
            checkUpgradeWindowTimer.Enabled = true;
        }

        private void OnCheckUpgradeWindowTimerElapsed(object sender, ElapsedEventArgs e)
        {
            CallGetUpdateWindow();
        }

        /// <summary>
        /// Call swc.exe with getupdatewindow parameter
        /// </summary>
        private static void CallGetUpdateWindow() => 
            Process.Start("swc.exe", "/command=getupdatewindow");

        /// <summary>
        /// Update the window
        /// </summary>
        internal void UpdateWindow(UpdateWindowData updateWindowData)
        {
            DateTime dateOfNextUpgrade;
            if (DateTime.Now.Hour >= updateWindowData.AutoUpdateHourOfDay)
            {
                dateOfNextUpgrade = DateTime.Today.Next(updateWindowData.AutoUpdateDayOfWeek);
            }
            else
            {
                dateOfNextUpgrade = DateTime.Today.TodayOrNext(updateWindowData.AutoUpdateDayOfWeek);
            }
            dateOfNextUpgrade.AddHours(updateWindowData.AutoUpdateHourOfDay);

            upgradeTimer?.Dispose();
            double millisecondsToOpenWindow = (dateOfNextUpgrade - DateTime.Now).Milliseconds;
            upgradeTimer = new Timer(millisecondsToOpenWindow);
     
            upgradeTimer.Elapsed += async (sender, e) => 
                await shipWorksUpgrade.Upgrade(updateWindowData.TangoCustomerId);

            upgradeTimer.AutoReset = false;
            upgradeTimer.Enabled = true;
        }        
    }
}