using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Amazon.WebServices.SellerCentral;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Amazon.Dime
{
    /// <summary>
    /// DIME aware, Amazon web service client
    /// </summary>
    public class SellerCentralDimeService : merchantinterfacedime
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerCentralDimeService(string logName)
            : base(new ApiLogEntry(ApiLogSource.Amazon, logName))
        {
        }

        // Collection of DIME attachments to be sent
        List<DimeAttachment> attachments = new List<DimeAttachment>();

        /// <summary>
        /// Adds an attachment to the request
        /// </summary>
        public void AddAttachment(string id, string fileToAttach, string mediaType)
        {
            attachments.Add(new DimeAttachment(id, fileToAttach, mediaType));
        }

        /// <summary>
        /// Gets the web request to service this Web Service call.  This is where we'll inject our own DIME-aware web request.
        /// </summary>
        protected override WebRequest GetWebRequest(Uri uri)
        {
            // Create a DIME request wrapping the typical WebRequest
            return new DimeWebRequest(base.GetWebRequest(uri), attachments);
        }
    }
}
