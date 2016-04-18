using System;

namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// ViewModel for WebBrowserDlg
    /// </summary>
    public class WebBrowserDlgViewModel : IWebBrowserDlgViewModel
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        public void Load(Uri url, string title)
        {
            Url = url;
            Title = title;
        }
    }
}
