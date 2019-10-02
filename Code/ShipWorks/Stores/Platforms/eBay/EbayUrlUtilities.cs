using System;
using System.Web;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// A utility class to encapsulate eBay URL management.
    /// </summary>
    public static class EbayUrlUtilities
    {
        /// <summary>
        /// Gets the sandbox endpoint override
        /// </summary>
        public static string SandboxEndpointOverride => InterapptiveOnly.Registry.GetValue("eBayEndpoint", String.Empty);

        /// <summary>
        /// Gets a value indicating whether or not to use the sandbox endpoint override
        /// </summary>
        public static bool UseSandboxEndpointOverride => !UseLiveServer && !String.IsNullOrWhiteSpace(SandboxEndpointOverride);

        /// <summary>
        /// Gets or sets a value indicating whether to use the live server or the test server.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use live server]; otherwise, <c>false</c>.
        /// </value>
        public static bool UseLiveServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("eBayLiveServer", true);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("eBayLiveServer", value);
            }
        }

        /// <summary>
        /// Gets the SOAP URL that requests will be submitted to.
        /// </summary>
        public static string SoapUrl
        {
            get
            {
                if (UseLiveServer)
                {
                    return "https://api.ebay.com/wsapi";
                }
                else
                {
                    return String.IsNullOrEmpty(SandboxEndpointOverride) ? "https://api.sandbox.ebay.com/wsapi" : SandboxEndpointOverride;
                }
            }
        }

        /// <summary>
        /// Returns the URL to eBay item page
        /// </summary>
        public static string GetItemUrl(long itemID)
        {
            if (UseLiveServer)
            {
                return "http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&item=" + itemID;
            }
            else
            {
                return "http://cgi.sandbox.ebay.com/ws/eBayISAPI.dll?ViewItem&item=" + itemID;
            }
        }

        /// <summary>
        /// Returns the URL to the eBay feedback page.
        /// </summary>
        public static string GetFeedbackUrl(string userID)
        {
            if (UseLiveServer)
            {
                return "http://feedback.ebay.com/ws/eBayISAPI.dll?ViewFeedback2&userid=" + userID;
            }
            else
            {
                return "http://cgi.sandbox.ebay.com/aw-cgi/eBayISAPI.dll?ViewFeedback&userid=" + userID;
            }
        }

        /// <summary>
        /// eBay URL to send users to get their auth token
        /// </summary>
        public static string GetTokenUrl()
        {
            // We have to URL encode this twice, b\c ebay url decodes it to pass it back to us.  And then
            // PHP url decodes it again.
            string identifier = HttpUtility.UrlEncode(HttpUtility.UrlEncode(ShipWorksSession.InstanceID.ToString()));
            string tokenParameters = "&runame=interapptive-standard&ruparams=license%3D" + identifier;

            if (UseLiveServer)
            {
                return "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn" + tokenParameters;
            }
            else
            {
                return "https://signin.sandbox.ebay.com/ws/eBayISAPI.dll?SignIn" + tokenParameters;
            }
        }
    }
}
