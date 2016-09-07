using Interapptive.Shared.UI;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;


namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// Interaction logic for DismissableWebBrowserDlg.xaml
    /// </summary>
    public partial class DismissableWebBrowserDlg : IWin32Window, IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DismissableWebBrowserDlg"/> class.
        /// </summary>
        public DismissableWebBrowserDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether [hide scrollbars].
        /// </summary>
        public bool HideScrollbars { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [suppress popups] - this includes script errors.
        /// </summary>
        public bool SuppressPopups { get; set; } = true;
        
        /// <summary>
        /// Window handle.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Called when [click close].
        /// </summary>
        private void OnClickClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Loads the owner.
        /// </summary>
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            Handle = owner.Handle;

            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }

        /// <summary>
        /// Called when [browser load completed].
        /// </summary>
        void OnBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (HideScrollbars)
            {
                System.Windows.Controls.WebBrowser browser = (System.Windows.Controls.WebBrowser)sender;

                string script = "document.documentElement.style.overflow ='hidden'";
                browser.InvokeScript("execScript", script, "JavaScript");
            }
        }

        /// <summary>
        /// Called when [browser navigated].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        void OnBrowserNavigated(object sender, NavigationEventArgs e)
        {
            if (SuppressPopups)
            {
                System.Windows.Controls.WebBrowser browser = (System.Windows.Controls.WebBrowser) sender;

                FieldInfo webBrowserInfo = browser.GetType()
                    .GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

                object comWebBrowser = webBrowserInfo?.GetValue(browser);

                comWebBrowser?.GetType()
                    .InvokeMember("Silent", BindingFlags.SetProperty, null, comWebBrowser, new object[] {SuppressPopups});
            }
        }
    }
}
