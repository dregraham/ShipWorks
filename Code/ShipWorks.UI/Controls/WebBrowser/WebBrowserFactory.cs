using System;
using System.Windows;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// Creates a WebBrowser
    /// </summary>
    public class WebBrowserFactory : IWebBrowserFactory
    {
        private readonly Func<string, IDialog> webBrowserDlgFactory;
        private readonly IWebBrowserDlgViewModel webBrowserDlgViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowserFactory"/> class.
        /// </summary>
        public WebBrowserFactory(Func<string, IDialog> webBrowserDlgFactory, IWebBrowserDlgViewModel webBrowserDlgViewModel)
        {
            this.webBrowserDlgFactory = webBrowserDlgFactory;
            this.webBrowserDlgViewModel = webBrowserDlgViewModel;
        }

        /// <summary>
        /// Creates the specified URI.
        /// </summary>
        public IDialog Create(Uri uri, string title, Window owner)
        {
            webBrowserDlgViewModel.Load(uri, title);
            IDialog browserDlg = webBrowserDlgFactory("WebBrowserDlg");

            // If not coming from WPF, this makes it a lot easier...
            if (owner == null)
            {
                browserDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                browserDlg.Topmost = true;
            }
            else
            {
                // Set owner
                browserDlg.Owner = owner;
            }

            browserDlg.DataContext = webBrowserDlgViewModel;

            return browserDlg;
        }
    }
}
