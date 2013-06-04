using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// This is a webform displays a browser window with a specific URL.
    /// It listens on a determined localhost port. When it detects a requested URL on 
    /// that port, it sets the InitialURL to the requesting URL and the dialog closes.
    /// </summary>
    public partial class HttpRedirectInterceptorDlg : Form
    {
        private string listenerURL = string.Empty;
        private PortListener listener = null;
        private int port = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpRedirectInterceptorDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The localhost URL requested and detected by the listenner.
        /// </summary>
        public Uri ResultURL { get; set; }

        /// <summary>
        /// The URL displayed when the form is shown
        /// </summary>
        public Uri InitialURL { get; set; }
        
        /// <summary>
        /// Initializes the listenner.
        /// </summary>
        /// <returns>Url of listenner.</returns>
        public Uri StartListening()
        {
            //if listenner is already listenning, throw.
            if (string.IsNullOrEmpty(listenerURL))
            {
                listener = new PortListener();

                port = listener.StartListening();

                listener.BeginRequest += new EventHandler<PortListenerRequestBeginEventArgs>(OnRequestBegin);
                listenerURL = string.Format("http://localhost:{0}/", Port.ToString());
            }
            else
            {                
                throw new InvalidOperationException("In BroswerListener.StartListening, listenerURL is Already Initialized.");
            }

            return new Uri(listenerURL);
        }

        /// <summary>
        /// The Port the listenner is listenning on.
        /// </summary>
        public int Port
        {
            get
            {
                return port;
            }
        }

        /// <summary>
        /// Handles the listener callback and sets the ResultURL.
        /// </summary>
        private void OnRequestBegin(object sender, PortListenerRequestBeginEventArgs e)
        {
            ResultURL = e.RequestedUrl;

            DialogResult = DialogResult.OK;          
        }

        /// <summary>
        /// When the form is shown, navigate to the InitialURL
        /// </summary>
        private void OnBrowserListenerShown(object sender, EventArgs e)
        {
            browser.Navigate(InitialURL);
        }

        /// <summary>
        /// Capture Close event and dispose of the listener.
        /// </summary>
        private void OnBrowserListenerFormClosing(object sender, FormClosingEventArgs e)
        {
            if (listener != null)
            {
                listener.Dispose();
            }
            if (browser != null)
            {
                browser.Stop();
            }
            browser.Dispose();
            Dispose();
        }
    }
}
