using System;
using System.Reflection;

namespace ShipWorks.UI.Controls.WebBrowser
{
    public interface IWebBrowserDlgViewModel
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        [Obfuscation(Exclude = true)]
        Uri Url { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        [Obfuscation(Exclude = true)]
        string Title { get; }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        void Load(Uri url, string title);
    }
}