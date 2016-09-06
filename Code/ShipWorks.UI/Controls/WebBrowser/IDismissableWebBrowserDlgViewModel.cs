using System;
using System.Windows.Input;

namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// Interface for ViewModel for DismissableWebBrowserDlg
    /// </summary>
    public interface IDismissableWebBrowserDlgViewModel
    {
        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        void Load(Uri url, string title, string moreInfoUrl);

        /// <summary>
        /// Gets or sets a value indicating the client doesn't want to see the message again.
        /// </summary>
        bool Dissmissed { get; set; }

        /// <summary>
        /// Command that display moreInfoUrl in default browser
        /// </summary>
        ICommand MoreInfoClickCommand { get; }
    }
}