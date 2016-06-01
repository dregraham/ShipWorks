using System;
using System.Reflection;

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
        [Obfuscation(Exclude = true)]
        public Uri Url { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        [Obfuscation(Exclude = true)]
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
