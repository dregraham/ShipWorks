using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Represents the AutoUpdateStatusProvider
    /// </summary>
    public interface IAutoUpdateStatusProvider
    {
        /// <summary>
        /// close the Splash Screen
        /// </summary>
        void CloseSplashScreen();

        /// <summary>
        /// show the Splash Screen
        /// </summary>
        void ShowSplashScreen();

        /// <summary>
        /// update the status on the Splash Screen
        /// </summary>
        void UpdateStatus(string status);
    }
}