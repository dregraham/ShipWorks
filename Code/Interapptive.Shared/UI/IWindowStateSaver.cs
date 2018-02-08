namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Window state saver
    /// </summary>
    public interface IWindowStateSaver
    {
        /// <summary>
        /// State of the window
        /// </summary>
        WindowState State { get; }
    }
}