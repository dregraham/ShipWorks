using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

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
        // content from: https://mpsnare.iesnare.com/snare.js
        private static string deviceHtml = ResourceUtility.ReadString("ShipWorks.Shipping.Carriers.UPS.OneBalance.DiviceIdentification.html");
        private static string functionName = "GetIdentifier";

        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceIdentificationControl()
        {
            InitializeComponent();
            webBrowser.DocumentCompleted += OnComplete;
        }

        /// <summary>
        /// The Identity required by UPS account registration
        /// </summary>
        public string Identity { get; private set; }

        /// <summary>
        /// Generate the html required to get a device id.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            webBrowser.DocumentText = deviceHtml;
        }

        /// <summary>
        /// Sets the identifier
        /// </summary>
        private void OnComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // null check so it doesn't bomb
            Identity = webBrowser.Document.InvokeScript(functionName)?.ToString();
        }
    }
}
