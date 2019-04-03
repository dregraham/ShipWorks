using System.Windows;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Main grid header
    /// </summary>
    public interface IMainGridHeader
    {
        /// <summary>
        /// Get the actual control
        /// </summary>
        UIElement Control { get; }

        /// <summary>
        /// View model used by the control
        /// </summary>
        MainGridHeaderViewModel ViewModel { get; set; }
    }
}