using System;

namespace Interapptive.Shared.Net
{

    /// <summary>
    /// EventArgs used when PortListener receives a request.
    /// </summary>
    public class PortListenerRequestBeginEventArgs:EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requrestedURL">URL Requested</param>
        public PortListenerRequestBeginEventArgs(Uri requestedUrl)
        {
            RequestedUrl = requestedUrl;
        }
        
        /// <summary>
        /// URL Requested
        /// </summary>
        public Uri RequestedUrl { get; set; }
    }
}
