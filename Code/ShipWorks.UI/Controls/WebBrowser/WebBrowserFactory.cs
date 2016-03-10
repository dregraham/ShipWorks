using System;
using System.Windows;
using System.Windows.Interop;
using ShipWorks.ApplicationCore.Licensing;
using IWin32Window = System.Windows.Forms.IWin32Window;

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
        /// Creates a browser dlg with the given parameters
        /// </summary>
        public IDialog Create(Uri uri, string title, Window owner, Size size)
        {
            IDialog browserDlg = CreateDialog(uri, title, size);

            if (owner != null)
            {
                // Set owner
                browserDlg.Owner = owner;
            }
            
            return browserDlg;
        }

        /// <summary>
        /// Creates a browser dlg with the given parameters
        /// </summary>
        public IDialog Create(Uri uri, string title, IWin32Window owner, Size size)
        {
            IDialog browserDlg = CreateDialog(uri, title, size);

            Window window = browserDlg as Window;
            if (window != null && owner != null)
            {
                new WindowInteropHelper(window) { Owner = owner.Handle };
            }
            
            return browserDlg;
        }

        /// <summary>
        /// Creates the dialog using the given parameters
        /// </summary>
        private IDialog CreateDialog(Uri uri, string title, Size size)
        {
            // Create the dialog and set the view model
            webBrowserDlgViewModel.Load(uri, title);
            IDialog dialog = webBrowserDlgFactory("WebBrowserDlg");
            dialog.DataContext = webBrowserDlgViewModel;

            // Set the dimensions
            Window window = dialog as Window;
            if (window != null)
            {
                window.Height = size.Height;
                window.Width = size.Width;
            }

            return dialog;
        }
    }
}
