using System;
using System.Windows;
using System.Windows.Interop;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// Creates a WebBrowser
    /// </summary>
    public class WebBrowserFactory : IWebBrowserFactory
    {
        private readonly IWebBrowserDlgViewModel webBrowserDlgViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowserFactory"/> class.
        /// </summary>
        public WebBrowserFactory(IWebBrowserDlgViewModel webBrowserDlgViewModel)
        {
            this.webBrowserDlgViewModel = webBrowserDlgViewModel;
        }

        /// <summary>
        /// Creates a browser dlg with the given parameters
        /// </summary>
        public IDialog Create(Uri uri, string title, IWin32Window owner, Size size)
        {
            // Create the dialog and set the view model
            webBrowserDlgViewModel.Load(uri, title);

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IDialog dialog = scope.ResolveNamed<IDialog>("WebBrowserDlg",
                    new TypedParameter(typeof (IWin32Window), owner));
                dialog.DataContext = webBrowserDlgViewModel;
                dialog.Height = size.Height;
                dialog.Width = size.Width;

                return dialog;
            }
        }
    }
}
