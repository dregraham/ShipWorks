namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// A navigation service built on top of MVVMLight's implementation that is more friendly toward WPF
    /// </summary>
    public interface INavigationService<T> : GalaSoft.MvvmLight.Views.INavigationService
    {
        /// <summary>
        /// Optional parameter
        /// </summary>
        object Parameter { get; }

        /// <summary>
        /// Instructs navigation service to display a new page corresponding to the given key,
        /// provided as an enum.
        /// </summary>
        /// <param name="navigationPage"></param>
        void NavigateTo(T navigationPage);
    }
}