using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Infopia.WebServices
{
    /// <summary>
    /// Infopia web service class that disabled KeepAlive
    /// </summary>
    public class InfopiaWebService2 : InfopiaWebService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaWebService2(ApiLogEntry logEntry)
            : base(logEntry)
        {
        }

        /// <summary>
        /// Customize the web request used to communicate with Infopia
        /// </summary>
        protected override WebRequest GetWebRequest(Uri uri)
        {
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(uri);

            // disabling keepalive due to Infopia problems
            webRequest.KeepAlive = false;

            return webRequest;
        }
    }
}
