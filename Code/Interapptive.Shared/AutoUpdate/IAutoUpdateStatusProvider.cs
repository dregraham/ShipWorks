using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Represents the AutoUpdateStatusProvider
    /// </summary>
    public interface IAutoUpdateStatusProvider
    {
        /// <summary>
        /// Show the Splash Screen
        /// </summary>
        void ShowSplashScreen(string instanceId);

        /// <summary>
        /// Close the splash screen
        /// </summary>
        void CloseSplashScreen();

        /// <summary>
        /// Update the status on the Splash Screen
        /// </summary>
        void UpdateStatus(string status);
    }
}