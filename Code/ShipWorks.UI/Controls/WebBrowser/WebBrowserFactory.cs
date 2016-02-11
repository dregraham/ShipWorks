using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <param name="uri">The URI to navigate to.</param>
        /// <param name="title">The title of the dialog window.</param>
        public IDialog Create(Uri uri, string title)
        {
            webBrowserDlgViewModel.Load(uri, title);
            IDialog browserDlg = webBrowserDlgFactory("WebBrowserDlg");
            browserDlg.DataContext = webBrowserDlgViewModel;

            return browserDlg;
        }
    }
}
