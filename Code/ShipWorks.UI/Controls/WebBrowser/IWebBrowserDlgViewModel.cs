using System;

namespace ShipWorks.UI.Controls.WebBrowser
{
    public interface IWebBrowserDlgViewModel
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        Uri Url { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        void Load(Uri url, string title);
    }
}