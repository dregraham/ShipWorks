using System;
using System.Windows;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// Interface for WebBrowserFactory
    /// </summary>
    public interface IWebBrowserFactory
    {
        /// <summary>
        /// Creates an IDialog with the given URI and title
        /// </summary>
        IDialog Create(Uri uri, string title, Window owner);
    }
}