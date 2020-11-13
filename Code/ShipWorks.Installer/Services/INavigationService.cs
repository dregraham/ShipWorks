using System;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// A navigation service built on top of MVVMLight's implementation that is more friendly toward WPF
    /// </summary>
    public interface INavigationService<T>
    {
        /// <summary>
        /// Optional parameter
        /// </summary>
        object Parameter { get; }

        /// <summary>
        /// Contains the key to the current page. Uses INPC.
        /// </summary>
        T CurrentPageKey { get; }

        /// <summary>
        /// Instructs navigation service to display a new page corresponding to the given key,
        /// provided as an enum.
        /// </summary>
        /// <param name="navigationPage"></param>
        void NavigateTo(T navigationPage);

        /// <summary>
        /// Add or alter a page
        /// </summary>
        void GoBack();

        /// <summary>
        /// Event that's triggered on navigation
        /// </summary>
        event EventHandler<NavigatedEventArgs<T>> Navigated;
    }
}