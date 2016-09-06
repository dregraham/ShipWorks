namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// View Model for DismissableWebBrowser
    /// </summary>
    /// <seealso cref="ShipWorks.UI.Controls.WebBrowser.IWebBrowserDlgViewModel" />
    public interface IDismissableWebBrowswerViewModel : IWebBrowserDlgViewModel
    {
        /// <summary>
        /// Dismisses this instance
        /// </summary>
        void Dismiss();
    }
}