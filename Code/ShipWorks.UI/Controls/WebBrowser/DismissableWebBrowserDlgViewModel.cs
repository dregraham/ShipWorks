using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Net;
using System;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace ShipWorks.UI.Controls.WebBrowser
{
    /// <summary>
    /// ViewModel for DismissableWebBrowserDlg
    /// </summary>
    /// <seealso cref="ShipWorks.UI.Controls.WebBrowser.WebBrowserDlgViewModel" />
    public class DismissableWebBrowserDlgViewModel : WebBrowserDlgViewModel, IDismissableWebBrowserDlgViewModel
    {
        private readonly IWin32Window owner;
        private string moreInfoUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="DismissableWebBrowserDlgViewModel"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public DismissableWebBrowserDlgViewModel(IWin32Window owner)
        {
            this.owner = owner;

            MoreInfoClickCommand = new RelayCommand(MoreInfo);
        }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        public void Load(Uri url, string title, string moreInfoUrl)
        {
            this.moreInfoUrl = moreInfoUrl;
            Load(url, title);
        }

        /// <summary>
        /// Gets or sets a value indicating the client doesn't want to see the message again.
        /// </summary>
        [Obfuscation(Exclude=true)]
        public bool Dissmissed { get; set; }

        /// <summary>
        /// Command that display moreInfoUrl in default browser
        /// </summary>
        [Obfuscation(Exclude=true)]
        public ICommand MoreInfoClickCommand { get; }

        /// <summary>
        /// Displays moreInfoUrl in default browser
        /// </summary>
        private void MoreInfo()
        {
            WebHelper.OpenUrl(moreInfoUrl, owner);
        }
    }
}