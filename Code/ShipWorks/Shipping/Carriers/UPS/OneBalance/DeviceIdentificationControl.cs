using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    /// <summary>
    /// A Control to generate a device ID for UPS account creation
    /// </summary>
    /// <remarks>
    /// Using iOvation API: https://developers.powerreviews.com/Content/Write%20API/iOvation.htm
    /// </remarks>
    public partial class DeviceIdentificationControl : UserControl
    {
        private const string iOvationUrl = "https://mpsnare.iesnare.com/snare.js";

        /// <summary>
        /// The Identity required by UPS account registration
        /// </summary>
        public string Identity { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceIdentificationControl()
        {
            InitializeComponent();
            webBrowser.DocumentCompleted += OnComplete;
        }

        /// <summary>
        /// Generate the html required to get a device id.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            webBrowser.DocumentText =
                "<!DOCTYPE html><html><head>" +
                $"<script language='javascript' type='text/javascript' src='{iOvationUrl}'></script>" +
                "<script>function GetIdentifier(){return ioGetBlackbox().blackbox;}</script>" +
                "</head></html>";
        }

        /// <summary>
        /// Sets the identifier
        /// </summary>
        private void OnComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string functionName = "GetIdentifier";

            // null check so it doesn't bomb
            Identity = webBrowser.Document.InvokeScript(functionName)?.ToString();
        }
    }
}
