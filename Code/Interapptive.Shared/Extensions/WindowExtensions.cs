using System.Windows;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions on the WPF Window class
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Get the desktop bounds of a window
        /// </summary>
        public static Rect DesktopBounds(this Window window) =>
            new Rect(window.Left, window.Top, window.Width, window.Height);

        /// <summary>
        /// Get the desktop bounds of a window
        /// </summary>
        public static Window SetDesktopBounds(this Window window, Rect bounds)
        {
            window.Left = bounds.Left;
            window.Top = bounds.Top;
            window.Width = bounds.Width;
            window.Height = bounds.Height;

            return window;
        }
    }
}
