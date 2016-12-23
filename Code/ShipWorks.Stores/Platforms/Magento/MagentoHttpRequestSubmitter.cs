using System.Net;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// HttpRequestSubmitter for Magento
    /// </summary>
    /// <remarks>
    /// This was done to override autoredirect behavior. In base class, AllowAutoRedirect was
    /// only set if verb was not GET. 
    /// </remarks>
    public class MagentoHttpRequestSubmitter : HttpRequestSubmitter
    {
        /// <summary>
        /// Sets the allow automatic redirect
        /// </summary>
        protected override void SetAllowAutoRedirect(HttpWebRequest webRequest)
        {
            webRequest.AllowAutoRedirect = AllowAutoRedirect;
        }
    }
}