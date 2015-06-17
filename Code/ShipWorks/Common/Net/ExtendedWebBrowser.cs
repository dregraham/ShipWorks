using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Common.Net
{
    /// <summary>
    /// Extends Web Browser to support custom UserAgent.
    /// </summary>
    public class ExtendedWebBrowser : WebBrowser
    {
        bool renavigating = false;

        public string UserAgent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedWebBrowser"/> class.
        /// </summary>
        public ExtendedWebBrowser()
        {
            DocumentCompleted += SetupBrowser;

            //this will cause SetupBrowser to run (we need a document object)
            Navigate("about:blank");
        }

        /// <summary>
        /// Setups the browser.
        /// </summary>
        void SetupBrowser(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            DocumentCompleted -= SetupBrowser;
            SHDocVw.WebBrowser xBrowser = (SHDocVw.WebBrowser)ActiveXInstance;
            xBrowser.BeforeNavigate2 += BeforeNavigate;
        }

        /// <summary>
        /// Befores the navigate.
        /// </summary>
        void BeforeNavigate(object pDisp, ref object url, ref object flags, ref object targetFrameName,
            ref object postData, ref object headers, ref bool cancel)
        {
            if (!string.IsNullOrEmpty(UserAgent))
            {
                if (!renavigating)
                {
                    headers += string.Format("User-Agent: {0}\r\n", UserAgent);
                    renavigating = true;
                    cancel = true;
                    Navigate((string)url, (string)targetFrameName, (byte[])postData, (string)headers);
                }
                else
                {
                    renavigating = false;
                }
            }
        }
    }
}
