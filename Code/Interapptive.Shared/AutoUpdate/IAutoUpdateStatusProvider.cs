using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Represents the AutoUpdateStatusProvider
    /// </summary>
    public interface IAutoUpdateStatusProvider
    {
        /// <summary>
        /// show the Splash Screen
        /// </summary>
        void ShowSplashScreen(string instanceId);

        /// <summary>
        /// update the status on the Splash Screen
        /// </summary>
        void UpdateStatus(string status);
    }
}